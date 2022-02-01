using Cuemon.Extensions.Xunit;
using Savvyio.Extensions.EFCore;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Storage
{
    public class EfCoreDataStoreOptionsTest : Test
    {
        public EfCoreDataStoreOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EfCoreDataStoreOptions_Ensure_Initialization_Defaults()
        {
            var sut = new EfCoreDataStoreOptions();

            Assert.Null(sut.ContextConfigurator);
            Assert.Null(sut.ConventionsConfigurator);
            Assert.Null(sut.ModelConstructor);
        }
    }
}
