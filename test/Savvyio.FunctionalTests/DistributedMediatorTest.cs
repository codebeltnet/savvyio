using Amazon;
using Amazon.Runtime;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;
using Cuemon.Extensions.Text.Json.Formatters;
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
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Extensions.Newtonsoft.Json.Converters;
using Savvyio.Extensions.SimpleQueueService;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.v3.Priority;

namespace Savvyio
{
    [TestCaseOrderer(typeof(PriorityOrderer))]
    public class DistributedMediatorTest : Test
    {
        private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        private static readonly string UniqueId = $"{Guid.NewGuid():N}";

        public DistributedMediatorTest()
        {
            NewtonsoftJsonFormatterOptions.DefaultConverters += list => list.Add(new ValueObjectConverter());
        }

        [Fact, Priority(0)]
        public async Task EmulateWebApi_Controller_SendCommand()
        {
            await using var test = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
            {
                services.AddMarshaller<NewtonsoftJsonMarshaller>();
                services.AddAmazonCommandQueue(o =>
                {
                    o.Credentials = new BasicAWSCredentials(context.Configuration["AWS:IAM:AccessKey"], context.Configuration["AWS:IAM:SecretKey"]);
                    o.Endpoint = RegionEndpoint.EUWest1;
                    o.SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{context.Configuration["AWS:CallerIdentity"]}/distribute-mediator-test");
                });

                AmazonResourceNameOptions.DefaultAccountId = context.Configuration["AWS:CallerIdentity"];
            });
            var correlationId = Guid.NewGuid().ToString("N");
            var platformProviderId = Guid.NewGuid();
            var fullName = "Michael Amazortensen";
            var emailAddress = IsLinux ? $"linux-{UniqueId}@aws.dev" : $"windows-{UniqueId}@aws.dev";

            var createAccount = new CreateAccount(platformProviderId, fullName, emailAddress).SetCorrelationId(correlationId);

            var commandQueue = test.Host.Services.GetRequiredService<IPointToPointChannel<ICommand>>();

            await commandQueue.SendAsync(createAccount.ToMessage("urn:command:create-account".ToUri(), nameof(createAccount)).Yield()).ConfigureAwait(false);
        }

        [Fact, Priority(1)]
        public async Task EmulateWorker_ReceiveCommand()
        {
            await using var test = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
            {
                services.AddMarshaller<JsonMarshaller>();
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

                services.AddDapperDataSource(o => o.ConnectionFactory = () => new SqliteConnection().SetDefaults().AddAccountTable().AddPlatformProviderTable())
                    .AddDapperDataStore<AccountData, AccountProjection>()
                    .AddDapperExtensionsDataStore<PlatformProviderProjection>();

                services.AddAmazonCommandQueue(o =>
                {
                    o.Credentials = new BasicAWSCredentials(context.Configuration["AWS:IAM:AccessKey"], context.Configuration["AWS:IAM:SecretKey"]);
                    o.Endpoint = RegionEndpoint.EUWest1;
                    o.SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{context.Configuration["AWS:CallerIdentity"]}/distribute-mediator-test");
                    o.ReceiveContext.AssumeMessageProcessed = false;
                    o.ReceiveContext.VisibilityTimeout = TimeSpan.FromSeconds(5);
                });

                services.AddAmazonEventBus(o =>
                {
                    o.Credentials = new BasicAWSCredentials(context.Configuration["AWS:IAM:AccessKey"], context.Configuration["AWS:IAM:SecretKey"]);
                    o.Endpoint = RegionEndpoint.EUWest1;
                    o.SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{context.Configuration["AWS:CallerIdentity"]}/distribute-mediator-test.fifo");
                });

                services.AddSavvyIO(o =>
                {
                    o.EnableHandlerServicesDescriptor().UseAutomaticDispatcherDiscovery(true).UseAutomaticHandlerDiscovery(true).AddMediator<Mediator>();
                });

                AmazonResourceNameOptions.DefaultAccountId = context.Configuration["AWS:CallerIdentity"];
            });
            await Task.Delay(TimeSpan.FromSeconds(5));

            using var scope = test.Host.Services.GetService<IServiceScopeFactory>().CreateScope();

            var commandQueue = scope.ServiceProvider.GetRequiredService<IPointToPointChannel<ICommand>>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            IMessage<ICommand> createAccountMessage = null;
            var cmdDeadline = DateTime.UtcNow.AddSeconds(90);
            while (createAccountMessage == null && DateTime.UtcNow < cmdDeadline)
            {
                await foreach (var msg in commandQueue.ReceiveAsync())
                {
                    if (msg.Data is CreateAccount ca && ca.EmailAddress.Contains(UniqueId))
                    {
                        await msg.AcknowledgeAsync().ConfigureAwait(false);
                        createAccountMessage = msg;
                    }
                }
            }

            Assert.NotNull(createAccountMessage);
            var receivedCommand = Assert.IsType<CreateAccount>(createAccountMessage.Data);

            await mediator.CommitAsync(receivedCommand);

            var accounts = scope.ServiceProvider.GetRequiredService<IPersistentRepository<Account, long, Account>>();

            var entity = await accounts.FindAllAsync(account => account.EmailAddress == receivedCommand.EmailAddress).SingleOrDefaultAsync();

            var dao = await mediator.QueryAsync(new GetAccount(entity.Id)).ConfigureAwait(false);

            Assert.Equal(entity.Id, dao.Id);
            Assert.Equal(entity.EmailAddress, dao.EmailAddress);
            Assert.Equal(entity.FullName, dao.FullName);

            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        [Fact, Priority(2)]
        public async Task EmulateAnotherWorker_SubscribingToAccountCreated()
        {
            await using var test = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
            {
                services.Configure<JsonFormatterOptions>(o => o.Settings.Converters.AddMessageConverter().AddMetadataDictionaryConverter());
                services.AddMarshaller<JsonMarshaller>();
                services.AddAmazonEventBus(o =>
                {
                    o.Credentials = new BasicAWSCredentials(context.Configuration["AWS:IAM:AccessKey"], context.Configuration["AWS:IAM:SecretKey"]);
                    o.Endpoint = RegionEndpoint.EUWest1;
                    o.SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{context.Configuration["AWS:CallerIdentity"]}/distribute-mediator-test.fifo");
                    o.ReceiveContext.AssumeMessageProcessed = false;
                    o.ReceiveContext.VisibilityTimeout = TimeSpan.FromSeconds(5);
                });

                AmazonResourceNameOptions.DefaultAccountId = context.Configuration["AWS:CallerIdentity"];
            });
            var eventBus = test.Host.Services.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent>>();

            var invocationCount = 0;
            var deadline = DateTime.UtcNow.AddSeconds(90);
            while (invocationCount == 0 && DateTime.UtcNow < deadline)
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                await eventBus.SubscribeAsync(async (message, token) =>
                {
                    if (message.Data is AccountCreated ac && ac.EmailAddress.Contains(UniqueId))
                    {
                        invocationCount++;
                        await message.AcknowledgeAsync().ConfigureAwait(false);
                        cts.Cancel();
                    }
                }, o =>
                {
                    o.CancellationToken = cts.Token;
                    o.ThrowIfCancellationWasRequested = false;
                }).ConfigureAwait(false);
            }
            Assert.Equal(1, invocationCount);
        }
    }
}
