using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio
{
    public class SavvyioOptionsTest : Test
    {
        public SavvyioOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void SavvyioOptions_Ensure_Initialization_Defaults()
        {
            var sut = new SavvyioOptions();

            Assert.False(sut.AllowDispatcherDiscovery);
            Assert.False(sut.AllowHandlerDiscovery);
            Assert.False(sut.IncludeHandlerServicesDescriptor);
            Assert.Empty(sut.DispatcherImplementationTypes);
            Assert.Empty(sut.DispatcherServiceTypes);
            Assert.Empty(sut.HandlerImplementationTypes);
            Assert.Empty(sut.HandlerServiceTypes);
        }
    }
}
