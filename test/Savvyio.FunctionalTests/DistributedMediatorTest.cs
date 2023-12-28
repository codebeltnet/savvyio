using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Cuemon.Extensions;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.EventDriven;
using Savvyio.Assets.Queries;
using Savvyio.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.EventDriven;
using Savvyio.Extensions;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Dapper;
using Savvyio.Extensions.DependencyInjection.DapperExtensions;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService;
using Savvyio.Extensions.Newtonsoft.Json.Converters;
using Savvyio.Extensions.SimpleQueueService;
using Savvyio.Messaging;
using Xunit;
using Xunit.Priority;

namespace Savvyio
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class DistributedMediatorTest : Test
    {
        public DistributedMediatorTest()
        {
            NewtonsoftJsonFormatterOptions.DefaultConverters += list => list.Add(new ValueObjectConverter());
        }

        [Fact, Priority(0)]
        public async Task EmulateWebApi_Controller_SendCommand()
        {
            using (var host = WebApplicationTestFactory.CreateWithHostBuilderContext((context, services) =>
                   {
                       services.AddAmazonCommandQueue(o =>
                       {
                           o.Credentials = new BasicAWSCredentials(context.Configuration["AWS:IAM:AccessKey"], context.Configuration["AWS:IAM:SecretKey"]);
                           o.Endpoint = RegionEndpoint.EUWest1;
                           o.SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{context.Configuration["AWS:CallerIdentity"]}/distribute-mediator-test");
                       });

                       AmazonResourceNameOptions.DefaultAccountId = context.Configuration["AWS:CallerIdentity"];
                   }))
            {
                var correlationId = Guid.NewGuid().ToString("N");
                var platformProviderId = Guid.NewGuid();
                var fullName = "Michael Amazortensen";
                var emailAddress = "root@aws.dev";

                var createAccount = new CreateAccount(platformProviderId, fullName, emailAddress).SetCorrelationId(correlationId);

                var commandQueue = host.ServiceProvider.GetRequiredService<IPointToPointChannel<ICommand>>();

                await commandQueue.SendAsync(createAccount.ToMessage("urn:command:create-account".ToUri())).ConfigureAwait(false);
            }
        }

        [Fact, Priority(1)]
        public async Task EmulateWorker_ReceiveCommand()
        {
            using (var host = WebApplicationTestFactory.CreateWithHostBuilderContext((context, services) =>
                   {
                       services.AddEfCoreAggregateDataSource<Account>(o =>
                       {
                           o.ContextConfigurator = b => b.UseInMemoryDatabase($"AWS{nameof(Account)}").EnableDetailedErrors().LogTo(Console.WriteLine);
                           o.ModelConstructor = mb => mb.AddAccount();
                       }).AddEfCoreRepository<Account, long, Account>();

                       services.AddEfCoreAggregateDataSource<PlatformProvider>(o =>
                       {
                           o.ContextConfigurator = b => b.UseInMemoryDatabase($"AWS{nameof(PlatformProvider)}").EnableDetailedErrors().LogTo(Console.WriteLine);
                           o.ModelConstructor = mb => mb.AddPlatformProvider();
                       }).AddEfCoreRepository<PlatformProvider, Guid, PlatformProvider>();

                       services.AddDapperDataSource(o => o.ConnectionFactory = () => new SqliteConnection().SetDefaults().AddAccountTable().AddPlatformProviderTable(), o => o.Lifetime = ServiceLifetime.Scoped)
                           .AddDapperDataStore<AccountData, AccountProjection>()
                           .AddDapperExtensionsDataStore<PlatformProviderProjection>();

                       services.AddAmazonCommandQueue(o =>
                       {
                           o.Credentials = new BasicAWSCredentials(context.Configuration["AWS:IAM:AccessKey"], context.Configuration["AWS:IAM:SecretKey"]);
                           o.Endpoint = RegionEndpoint.EUWest1;
                           o.SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{context.Configuration["AWS:CallerIdentity"]}/distribute-mediator-test");
                       });

                       services.AddAmazonEventBus(o =>
                       {
                           o.Credentials = new BasicAWSCredentials(context.Configuration["AWS:IAM:AccessKey"], context.Configuration["AWS:IAM:SecretKey"]);
                           o.Endpoint = RegionEndpoint.EUWest1;
                           o.SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{context.Configuration["AWS:CallerIdentity"]}/distribute-mediator-test.fifo");
                       });

                       services.AddSavvyIO(o =>
                       {
                           o.EnableHandlerServicesDescriptor().UseAutomaticDispatcherDiscovery().UseAutomaticHandlerDiscovery().AddMediator<Mediator>();
                       });

                       AmazonResourceNameOptions.DefaultAccountId = context.Configuration["AWS:CallerIdentity"];
                   }))
            {

                await Task.Delay(TimeSpan.FromSeconds(5));

                var commandQueue = host.ServiceProvider.GetRequiredService<IPointToPointChannel<ICommand>>();
                var mediator = host.ServiceProvider.GetRequiredService<IMediator>();
                var createAccountMessage = await commandQueue.ReceiveAsync().SingleOrDefaultAsync().ConfigureAwait(false);

                Assert.IsType<CreateAccount>(createAccountMessage.Data);

                await mediator.CommitAsync(createAccountMessage.Data);

                var accounts = host.ServiceProvider.GetRequiredService<IPersistentRepository<Account, long, Account>>();

                var entity = await accounts.FindAllAsync(account => account.EmailAddress == "root@aws.dev").SingleOrDefaultAsync();

                var dao = await mediator.QueryAsync(new GetAccount(entity.Id)).ConfigureAwait(false);

                Assert.Equal(entity.Id, dao.Id);
                Assert.Equal(entity.EmailAddress, dao.EmailAddress);
                Assert.Equal(entity.FullName, dao.FullName);

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        [Fact, Priority(2)]
        public async Task EmulateAnotherWorker_SubscribingToAccountCreated()
        {
            using (var host = WebApplicationTestFactory.CreateWithHostBuilderContext((context, services) =>
                   {
                       services.AddAmazonEventBus(o =>
                       {
                           o.Credentials = new BasicAWSCredentials(context.Configuration["AWS:IAM:AccessKey"], context.Configuration["AWS:IAM:SecretKey"]);
                           o.Endpoint = RegionEndpoint.EUWest1;
                           o.SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{context.Configuration["AWS:CallerIdentity"]}/distribute-mediator-test.fifo");
                       });

                       AmazonResourceNameOptions.DefaultAccountId = context.Configuration["AWS:CallerIdentity"];
                   }))
            {
                var eventBus = host.ServiceProvider.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent>>();

                var invocationCount = 0;
                await eventBus.SubscribeAsync((message, token) =>
                {
                    invocationCount++;
                    Assert.IsType<AccountCreated>(message.Data);
                    return Task.CompletedTask;
                }).ConfigureAwait(false);
                Assert.Equal(1, invocationCount);
            }
        }
    }
}
