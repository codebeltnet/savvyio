using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions
{
    public class SavvyioDependencyInjectionOptionsTest : Test
    {
        public SavvyioDependencyInjectionOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void SavvyioDependencyInjectionOptions_Ensure_Initialization_Defaults()
        {
            var sut = new SavvyioDependencyInjectionOptions();

            Assert.False(sut.AutomaticDispatcherDiscovery);
            Assert.False(sut.AutomaticHandlerDiscovery);
            Assert.False(sut.IncludeHandlerServicesDescriptor);
            Assert.Equal(ServiceLifetime.Transient, sut.HandlerServicesLifetime);
            Assert.Equal(ServiceLifetime.Scoped, sut.DispatcherServicesLifetime);
            Assert.Empty(sut.DispatcherImplementationTypes);
            Assert.Empty(sut.DispatcherServiceTypes);
            Assert.Empty(sut.HandlerImplementationTypes);
            Assert.Empty(sut.HandlerServiceTypes);
            Assert.Null(sut.AssembliesToScan);
        }
    }
}
