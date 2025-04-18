using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit.Hosting;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Queries;
using Savvyio.Data;
using Savvyio.Extensions;
using Savvyio.Extensions.Dapper;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Dapper;
using Savvyio.Extensions.DependencyInjection.DapperExtensions;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Extensions.Text.Json;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;
using YamlDotNet.Serialization.NamingConventions;

namespace Savvyio
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class MediatorTest : HostTest<ManagedHostFixture>
    {
        private IServiceProvider _provider;


        public MediatorTest(ManagedHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
        {
            _provider = hostFixture.Host.Services.CreateScope().ServiceProvider;
        }

        [Fact, Priority(0)]
        public async Task RegisterNewUser()
        {
            var correlationId = Guid.NewGuid().ToString("N");
            var caPpId = Guid.NewGuid();
            var caFullName = "Michael Mortensen";
            var caEmailAddress = "root@gimlichael.dev";

            var mediator = _provider.GetRequiredService<IMediator>();

            var hsd = _provider.GetRequiredService<IHandlerServicesDescriptor>();
            var plaintext = hsd.ToString();

            var mappings = hsd.GenerateHandlerDiscoveries();
            var json = JsonMarshaller.Create().Serialize(mappings).ToEncodedString(o => o.LeaveOpen = true);
            var newtonsoftJson = NewtonsoftJsonMarshaller.Create().Serialize(mappings).ToEncodedString(o => o.LeaveOpen = true);
            var yaml = YamlFormatter.SerializeObject(mappings, o => o.Settings.NamingConvention = CamelCaseNamingConvention.Instance).ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(json);

            TestOutput.WriteLine(System.Environment.NewLine);
            TestOutput.WriteLine("---");
            TestOutput.WriteLine(System.Environment.NewLine);

            TestOutput.WriteLine(yaml);

            var accountRepo = _provider.GetRequiredService<ISearchableRepository<Account, long, Account>>();

            await mediator.CommitAsync(new CreateAccount(caPpId, caFullName, caEmailAddress).SetCorrelationId(correlationId));

            var entity = await accountRepo.FindAllAsync(a => a.Metadata.Contains(new KeyValuePair<string, object>(MetadataDictionary.CorrelationId, correlationId))).SingleOrDefaultAsync();

            var dao = await mediator.QueryAsync(new GetAccount(entity.Id)).ConfigureAwait(false);

            Assert.True(Match("""
                              Discovered 2 ICommandHandler implementations covering a total of 5 ICommand methods
                              
                              Assembly: Savvyio.Assets.Tests
                              Namespace: Savvyio.Assets
                              
                              <AccountCommandHandler>
                              	*UpdateAccount --> &<RegisterDelegates>b__4_0
                              	*CreateAccount --> &CreateAccountAsync
                              
                              <PlatformProviderHandler>
                              	*CreatePlatformProvider --> &<Savvyio.Handlers.IFireForgetHandler<Savvyio.Commands.ICommand>.get_Delegates>b__8_1
                              	*UpdatePlatformProvider --> &<Savvyio.Handlers.IFireForgetHandler<Savvyio.Commands.ICommand>.get_Delegates>b__8_2
                              	*UpdatePlatformProviderAccountPolicy --> &<Savvyio.Handlers.IFireForgetHandler<Savvyio.Commands.ICommand>.get_Delegates>b__8_3
                              
                              -----------------------------------------------------------------------------------
                              
                              Discovered 2 IDomainEventHandler implementations covering a total of 11 IDomainEvent methods
                              
                              Assembly: Savvyio.Assets.Tests
                              Namespace: Savvyio.Assets.Domain.Handlers
                              
                              <AccountDomainEventHandler>
                              	*AccountEmailAddressChanged --> &OnInProcAccountEmailAddressChanged
                              	*AccountFullNameChanged --> &OnInProcAccountFullNameChanged
                              	*AccountInitiated --> &OnInProcAccountInitiated
                              	*AccountInitiatedChained --> &OnInProcAccountInitiatedChained
                              	*TracedAccountEmailAddressChanged --> &OnInProcTracedAccountEmailAddressChanged
                              	*TracedAccountInitiated --> &OnInProcTracedAccountInitiated
                              
                              <PlatformProviderDomainEventHandler>
                              	*PlatformProviderAccountPolicyChanged --> &<RegisterDelegates>b__3_0
                              	*PlatformProviderInitiated --> &<RegisterDelegates>b__3_1
                              	*PlatformProviderDescriptionChanged --> &<RegisterDelegates>b__3_2
                              	*PlatformProviderNameChanged --> &<RegisterDelegates>b__3_3
                              	*PlatformProviderThirdLevelDomainNameChanged --> &<RegisterDelegates>b__3_4
                              
                              --------------------------------------------------------------------------------------------
                              
                              Discovered 2 IIntegrationEventHandler implementations covering a total of 4 IIntegrationEvent methods
                              
                              Assembly: Savvyio.Assets.Tests
                              Namespace: Savvyio.Assets
                              
                              <PlatformProviderHandler>
                              	*PlatformProviderCreated --> &<Savvyio.Handlers.IFireForgetHandler<Savvyio.EventDriven.IIntegrationEvent>.get_Delegates>b__6_1
                              	*PlatformProviderUpdated --> &<Savvyio.Handlers.IFireForgetHandler<Savvyio.EventDriven.IIntegrationEvent>.get_Delegates>b__6_2
                              
                              <AccountEventHandler>
                              	*AccountCreated --> &OnOutProcAccountCreated
                              	*AccountUpdated --> &OnOutProcAccountUpdated
                              
                              -----------------------------------------------------------------------------------------------------
                              
                              Discovered 1 IQueryHandler implementation covering a total of 4 IQuery methods
                              
                              Assembly: Savvyio.Assets.Tests
                              Namespace: Savvyio.Assets
                              
                              <AccountQueryHandler>
                              	*GetAccount --> &<RegisterDelegates>b__2_0
                              	*GetFakeAccount --> &<RegisterDelegates>b__2_1
                              	*GetAccount --> &GetAccountAsync
                              	*GetFakeAccount --> &GetFakeAccountAsync
                              
                              ------------------------------------------------------------------------------
                              """.ReplaceLineEndings(), plaintext, o => o.ThrowOnNoMatch = true));
            Assert.Equal(json, newtonsoftJson);
            Assert.True(Match("""
                              [
                                {
                                  "abstractionType": "ICommandHandler",
                                  "implementationsCount": 2,
                                  "delegateType": "ICommand",
                                  "delegatesCount": 5,
                                  "assemblies": [
                                    {
                                      "name": "Savvyio.Assets.Tests",
                                      "namespace": "Savvyio.Assets",
                                      "implementations": [
                                        {
                                          "name": "AccountCommandHandler",
                                          "delegates": [
                                            {
                                              "type": "UpdateAccount",
                                              "handler": "<RegisterDelegates>b__4_0"
                                            },
                                            {
                                              "type": "CreateAccount",
                                              "handler": "CreateAccountAsync"
                                            }
                                          ]
                                        },
                                        {
                                          "name": "PlatformProviderHandler",
                                          "delegates": [
                                            {
                                              "type": "CreatePlatformProvider",
                                              "handler": "<Savvyio.Handlers.IFireForgetHandler<Savvyio.Commands.ICommand>.get_Delegates>b__8_1"
                                            },
                                            {
                                              "type": "UpdatePlatformProvider",
                                              "handler": "<Savvyio.Handlers.IFireForgetHandler<Savvyio.Commands.ICommand>.get_Delegates>b__8_2"
                                            },
                                            {
                                              "type": "UpdatePlatformProviderAccountPolicy",
                                              "handler": "<Savvyio.Handlers.IFireForgetHandler<Savvyio.Commands.ICommand>.get_Delegates>b__8_3"
                                            }
                                          ]
                                        }
                                      ]
                                    }
                                  ]
                                },
                                {
                                  "abstractionType": "IDomainEventHandler",
                                  "implementationsCount": 2,
                                  "delegateType": "IDomainEvent",
                                  "delegatesCount": 11,
                                  "assemblies": [
                                    {
                                      "name": "Savvyio.Assets.Tests",
                                      "namespace": "Savvyio.Assets.Domain.Handlers",
                                      "implementations": [
                                        {
                                          "name": "AccountDomainEventHandler",
                                          "delegates": [
                                            {
                                              "type": "AccountEmailAddressChanged",
                                              "handler": "OnInProcAccountEmailAddressChanged"
                                            },
                                            {
                                              "type": "AccountFullNameChanged",
                                              "handler": "OnInProcAccountFullNameChanged"
                                            },
                                            {
                                              "type": "AccountInitiated",
                                              "handler": "OnInProcAccountInitiated"
                                            },
                                            {
                                              "type": "AccountInitiatedChained",
                                              "handler": "OnInProcAccountInitiatedChained"
                                            },
                                            {
                                              "type": "TracedAccountEmailAddressChanged",
                                              "handler": "OnInProcTracedAccountEmailAddressChanged"
                                            },
                                            {
                                              "type": "TracedAccountInitiated",
                                              "handler": "OnInProcTracedAccountInitiated"
                                            }
                                          ]
                                        },
                                        {
                                          "name": "PlatformProviderDomainEventHandler",
                                          "delegates": [
                                            {
                                              "type": "PlatformProviderAccountPolicyChanged",
                                              "handler": "<RegisterDelegates>b__3_0"
                                            },
                                            {
                                              "type": "PlatformProviderInitiated",
                                              "handler": "<RegisterDelegates>b__3_1"
                                            },
                                            {
                                              "type": "PlatformProviderDescriptionChanged",
                                              "handler": "<RegisterDelegates>b__3_2"
                                            },
                                            {
                                              "type": "PlatformProviderNameChanged",
                                              "handler": "<RegisterDelegates>b__3_3"
                                            },
                                            {
                                              "type": "PlatformProviderThirdLevelDomainNameChanged",
                                              "handler": "<RegisterDelegates>b__3_4"
                                            }
                                          ]
                                        }
                                      ]
                                    }
                                  ]
                                },
                                {
                                  "abstractionType": "IIntegrationEventHandler",
                                  "implementationsCount": 2,
                                  "delegateType": "IIntegrationEvent",
                                  "delegatesCount": 4,
                                  "assemblies": [
                                    {
                                      "name": "Savvyio.Assets.Tests",
                                      "namespace": "Savvyio.Assets",
                                      "implementations": [
                                        {
                                          "name": "PlatformProviderHandler",
                                          "delegates": [
                                            {
                                              "type": "PlatformProviderCreated",
                                              "handler": "<Savvyio.Handlers.IFireForgetHandler<Savvyio.EventDriven.IIntegrationEvent>.get_Delegates>b__6_1"
                                            },
                                            {
                                              "type": "PlatformProviderUpdated",
                                              "handler": "<Savvyio.Handlers.IFireForgetHandler<Savvyio.EventDriven.IIntegrationEvent>.get_Delegates>b__6_2"
                                            }
                                          ]
                                        },
                                        {
                                          "name": "AccountEventHandler",
                                          "delegates": [
                                            {
                                              "type": "AccountCreated",
                                              "handler": "OnOutProcAccountCreated"
                                            },
                                            {
                                              "type": "AccountUpdated",
                                              "handler": "OnOutProcAccountUpdated"
                                            }
                                          ]
                                        }
                                      ]
                                    }
                                  ]
                                },
                                {
                                  "abstractionType": "IQueryHandler",
                                  "implementationsCount": 1,
                                  "delegateType": "IQuery",
                                  "delegatesCount": 4,
                                  "assemblies": [
                                    {
                                      "name": "Savvyio.Assets.Tests",
                                      "namespace": "Savvyio.Assets",
                                      "implementations": [
                                        {
                                          "name": "AccountQueryHandler",
                                          "delegates": [
                                            {
                                              "type": "GetAccount",
                                              "handler": "<RegisterDelegates>b__2_0"
                                            },
                                            {
                                              "type": "GetFakeAccount",
                                              "handler": "<RegisterDelegates>b__2_1"
                                            },
                                            {
                                              "type": "GetAccount",
                                              "handler": "GetAccountAsync"
                                            },
                                            {
                                              "type": "GetFakeAccount",
                                              "handler": "GetFakeAccountAsync"
                                            }
                                          ]
                                        }
                                      ]
                                    }
                                  ]
                                }
                              ]
                              """.ReplaceLineEndings(), json, o => o.ThrowOnNoMatch = true));

            Assert.True(Match("""
                              - abstractionType: ICommandHandler
                                implementationsCount: 2
                                delegateType: ICommand
                                delegatesCount: 5
                                assemblies:
                                  - name: Savvyio.Assets.Tests
                                    namespace: Savvyio.Assets
                                    implementations:
                                      - name: AccountCommandHandler
                                        delegates:
                                          - type: UpdateAccount
                                            handler: <RegisterDelegates>b__4_0
                                          - type: CreateAccount
                                            handler: CreateAccountAsync
                                      - name: PlatformProviderHandler
                                        delegates:
                                          - type: CreatePlatformProvider
                                            handler: <Savvyio.Handlers.IFireForgetHandler<Savvyio.Commands.ICommand>.get_Delegates>b__8_1
                                          - type: UpdatePlatformProvider
                                            handler: <Savvyio.Handlers.IFireForgetHandler<Savvyio.Commands.ICommand>.get_Delegates>b__8_2
                                          - type: UpdatePlatformProviderAccountPolicy
                                            handler: <Savvyio.Handlers.IFireForgetHandler<Savvyio.Commands.ICommand>.get_Delegates>b__8_3
                              - abstractionType: IDomainEventHandler
                                implementationsCount: 2
                                delegateType: IDomainEvent
                                delegatesCount: 11
                                assemblies:
                                  - name: Savvyio.Assets.Tests
                                    namespace: Savvyio.Assets.Domain.Handlers
                                    implementations:
                                      - name: AccountDomainEventHandler
                                        delegates:
                                          - type: AccountEmailAddressChanged
                                            handler: OnInProcAccountEmailAddressChanged
                                          - type: AccountFullNameChanged
                                            handler: OnInProcAccountFullNameChanged
                                          - type: AccountInitiated
                                            handler: OnInProcAccountInitiated
                                          - type: AccountInitiatedChained
                                            handler: OnInProcAccountInitiatedChained
                                          - type: TracedAccountEmailAddressChanged
                                            handler: OnInProcTracedAccountEmailAddressChanged
                                          - type: TracedAccountInitiated
                                            handler: OnInProcTracedAccountInitiated
                                      - name: PlatformProviderDomainEventHandler
                                        delegates:
                                          - type: PlatformProviderAccountPolicyChanged
                                            handler: <RegisterDelegates>b__3_0
                                          - type: PlatformProviderInitiated
                                            handler: <RegisterDelegates>b__3_1
                                          - type: PlatformProviderDescriptionChanged
                                            handler: <RegisterDelegates>b__3_2
                                          - type: PlatformProviderNameChanged
                                            handler: <RegisterDelegates>b__3_3
                                          - type: PlatformProviderThirdLevelDomainNameChanged
                                            handler: <RegisterDelegates>b__3_4
                              - abstractionType: IIntegrationEventHandler
                                implementationsCount: 2
                                delegateType: IIntegrationEvent
                                delegatesCount: 4
                                assemblies:
                                  - name: Savvyio.Assets.Tests
                                    namespace: Savvyio.Assets
                                    implementations:
                                      - name: PlatformProviderHandler
                                        delegates:
                                          - type: PlatformProviderCreated
                                            handler: <Savvyio.Handlers.IFireForgetHandler<Savvyio.EventDriven.IIntegrationEvent>.get_Delegates>b__6_1
                                          - type: PlatformProviderUpdated
                                            handler: <Savvyio.Handlers.IFireForgetHandler<Savvyio.EventDriven.IIntegrationEvent>.get_Delegates>b__6_2
                                      - name: AccountEventHandler
                                        delegates:
                                          - type: AccountCreated
                                            handler: OnOutProcAccountCreated
                                          - type: AccountUpdated
                                            handler: OnOutProcAccountUpdated
                              - abstractionType: IQueryHandler
                                implementationsCount: 1
                                delegateType: IQuery
                                delegatesCount: 4
                                assemblies:
                                  - name: Savvyio.Assets.Tests
                                    namespace: Savvyio.Assets
                                    implementations:
                                      - name: AccountQueryHandler
                                        delegates:
                                          - type: GetAccount
                                            handler: <RegisterDelegates>b__2_0
                                          - type: GetFakeAccount
                                            handler: <RegisterDelegates>b__2_1
                                          - type: GetAccount
                                            handler: GetAccountAsync
                                          - type: GetFakeAccount
                                            handler: GetFakeAccountAsync
                              
                              """.ReplaceLineEndings(), yaml, o => o.ThrowOnNoMatch = true));

            Assert.Equal(entity.Id, dao.Id);
            Assert.Equal(entity.EmailAddress, dao.EmailAddress);
            Assert.Equal(entity.FullName, dao.FullName);

        }

        [Fact, Priority(1)]
        public async Task RegisterNewUser_ShouldFailWithValidationException_BecauseOfUniqueEmailAddress()
        {
            var correlationId = Guid.NewGuid().ToString("N");
            var caPpId = Guid.NewGuid();
            var caFullName = "El Presidente";
            var caEmailAddress = "root@gimlichael.dev";

            var mediator = _provider.GetRequiredService<IMediator>();

            await Assert.ThrowsAsync<ValidationException>(() => mediator.CommitAsync(new CreateAccount(caPpId, caFullName, caEmailAddress).SetCorrelationId(correlationId)));
        }

        [Fact, Priority(2)]
        public async Task RegisterAnotherUser()
        {
            var caPpId = Guid.NewGuid();
            var caFullName = "El Presidento";
            var caEmailAddress = "makemyday@us.gov";

            var mediator = _provider.GetRequiredService<IMediator>();

            await mediator.CommitAsync(new CreateAccount(caPpId, caFullName, caEmailAddress));
        }

        [Fact, Priority(3)]
        public async Task VerifyUsers()
        {
            var accountRepo = _provider.GetRequiredService<ISearchableRepository<Account, long, Account>>();
            var accountDao = _provider.GetRequiredService<ISearchableDataStore<AccountProjection, DapperQueryOptions>>();
            var daos = new List<AccountProjection>(await accountDao.FindAllAsync().ConfigureAwait(false));
            var entities = new List<Account>(await accountRepo.FindAllAsync().ConfigureAwait(false));
            foreach (var entity in entities)
            {
                TestOutput.WriteLine(entity.EmailAddress);
                Assert.Equal(entity.Id, daos.Single(ac => ac.Id == entity.Id).Id);
                Assert.Equal(entity.EmailAddress, daos.Single(ac => ac.Id == entity.Id).EmailAddress);
                Assert.Equal(entity.FullName, daos.Single(ac => ac.Id == entity.Id).FullName);
            }

            Assert.Equal(2, entities.Count);
            Assert.Equal(2, daos.Count);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddEfCoreAggregateDataSource<Account>(o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(Account)).EnableDetailedErrors().LogTo(Console.WriteLine);
                o.ModelConstructor = mb => mb.AddAccount();
            }).AddEfCoreRepository<Account, long, Account>();

            services.AddEfCoreAggregateDataSource<PlatformProvider>(o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(PlatformProvider)).EnableDetailedErrors().LogTo(Console.WriteLine);
                o.ModelConstructor = mb => mb.AddPlatformProvider();
            }).AddEfCoreRepository<PlatformProvider, Guid, PlatformProvider>();

            services.AddDapperDataSource(o => o.ConnectionFactory = () => new SqliteConnection().SetDefaults().AddAccountTable().AddPlatformProviderTable())
                .AddDapperDataStore<AccountData, AccountProjection>()
                .AddDapperExtensionsDataStore<PlatformProviderProjection>();

            services.AddSavvyIO(o =>
            {
                o.EnableHandlerServicesDescriptor().UseAutomaticDispatcherDiscovery(true).UseAutomaticHandlerDiscovery(true).AddMediator<Mediator>();
            })
            .AddHandlerServicesDescriptor();
        }
    }
}
