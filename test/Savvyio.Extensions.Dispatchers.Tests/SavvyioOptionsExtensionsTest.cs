using System.Linq;
using Codebelt.Extensions.Xunit;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Queries;
using Xunit;

namespace Savvyio.Extensions
{
    public class SavvyioOptionsExtensionsTest : Test
    {
        public SavvyioOptionsExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void UseAutomaticDispatcherDiscovery_ShouldUseCallingAssembly_WhenBruteAssemblyScanningIsFalse()
        {
            var options = new SavvyioOptions().UseAutomaticDispatcherDiscovery();

            Assert.Empty(options.DispatcherServiceTypes);
            Assert.Empty(options.DispatcherImplementationTypes);
        }

        [Fact]
        public void UseAutomaticHandlerDiscovery_ShouldUseCallingAssembly_WhenBruteAssemblyScanningIsFalse()
        {
            var options = new SavvyioOptions().UseAutomaticHandlerDiscovery();

            Assert.Collection(options.HandlerServiceTypes.OrderBy(type => type.Name),
                type => Assert.Equal(typeof(ICommandHandler), type),
                type => Assert.Equal(typeof(IDomainEventHandler), type),
                type => Assert.Equal(typeof(IIntegrationEventHandler), type),
                type => Assert.Equal(typeof(IQueryHandler), type));
            Assert.Contains(options.HandlerImplementationTypes, type => type.Name == "TestMediatorCommandHandler");
            Assert.Contains(options.HandlerImplementationTypes, type => type.Name == "TestMediatorDomainEventHandler");
            Assert.Contains(options.HandlerImplementationTypes, type => type.Name == "TestMediatorIntegrationEventHandler");
            Assert.Contains(options.HandlerImplementationTypes, type => type.Name == "TestMediatorQueryHandler");
        }
    }
}
