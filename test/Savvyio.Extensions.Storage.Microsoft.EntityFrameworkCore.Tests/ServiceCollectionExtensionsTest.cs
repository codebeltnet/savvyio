using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Savvyio.Assets.Storage;
using Savvyio.Domain;
using Savvyio.Storage;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Storage
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddEfCoreDataStore_ShouldAddImplementationOfEfCoreDataStoreWithDefaultOptions()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataStore<EfCoreDataStore>();
            sut1.Configure<EfCoreDataStoreOptions>(_ => {});
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataStore>(sut2.GetRequiredService<IEfCoreDataStore>());
            Assert.IsType<EfCoreDataStore>(sut2.GetRequiredService<IUnitOfWork>());
            Assert.Same(sut2.GetRequiredService<IEfCoreDataStore>(), sut2.GetRequiredService<IUnitOfWork>());
            Assert.Null(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions>>().Value.ContextConfigurator);
            Assert.Null(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions>>().Value.ModelConstructor);
            Assert.Null(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions>>().Value.ConventionsConfigurator);
        }
    }
}
