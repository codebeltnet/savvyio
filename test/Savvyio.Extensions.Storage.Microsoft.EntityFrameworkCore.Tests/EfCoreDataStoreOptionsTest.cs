using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Storage
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
