using System;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Savvyio.Extensions.DependencyInjection
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddHandlerServicesDescriptor_ShouldAddHandlerServiceDescriptorAsIHandlerServicesDescriptor()
        {
            using var test = HostTestFactory.Create(
                services =>
                {
                    services.AddXunitTestLogging(TestOutput);
                    services.AddSavvyIO(o => o.EnableHandlerServicesDescriptor().UseAutomaticDispatcherDiscovery(true).UseAutomaticHandlerDiscovery(true));
                    services.AddHandlerServicesDescriptor();
                });

            test.Host.Services.WriteHandlerDiscoveriesToLog<ServiceCollectionExtensionsTest>();

            var loggerEntry = test.Host.Services.GetRequiredService<ILogger<ServiceCollectionExtensionsTest>>()
                .GetTestStore()
                .Query(entry => entry.Severity == LogLevel.Information && entry.Message.Contains("Discovered 2 ICommandHandler implementations"))
                .SingleOrDefault();

            Assert.NotNull(loggerEntry);
            Assert.NotNull(test.Host.Services.GetService<IHandlerServicesDescriptor>());
            Assert.IsType<HandlerServicesDescriptor>(test.Host.Services.GetService<IHandlerServicesDescriptor>());
            Assert.Equal("""
                         Information: Discovered 2 ICommandHandler implementations covering a total of 5 ICommand methods
                         
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
                         """.ReplaceLineEndings(), loggerEntry.Message);
        }

        [Fact]
        public void AddHandlerServicesDescriptor_ShouldThrowInvalidOperationException_SinceEnableHandlerServicesDescriptorWasNotSetupInAddSavvyIO()
        {
            using var test = HostTestFactory.Create(
                services =>
                {
                    services.AddXunitTestLogging(TestOutput);
                    services.AddSavvyIO(o => o.UseAutomaticDispatcherDiscovery(true).UseAutomaticHandlerDiscovery(true));
                    services.AddHandlerServicesDescriptor();
                });

            Assert.Throws<InvalidOperationException>(() => test.Host.Services.WriteHandlerDiscoveriesToLog<ServiceCollectionExtensionsTest>());
            Assert.Null(test.Host.Services.GetService<IHandlerServicesDescriptor>());
        }
    }
}
