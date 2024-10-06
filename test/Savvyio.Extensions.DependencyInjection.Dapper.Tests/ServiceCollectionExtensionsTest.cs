using Codebelt.Extensions.Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Extensions.Dapper;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.DependencyInjection.Dapper
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddDapperDataSource_ShouldAddDefaultImplementation()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDapperDataSource(o => o.ConnectionFactory = () => new SqliteConnection("Data Source=:memory:"));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DapperDataSource>(sut2.GetRequiredService<IDapperDataSource>());
            Assert.IsType<DapperDataSource>(sut2.GetRequiredService<IDataSource>());
            Assert.Same(sut2.GetRequiredService<IDapperDataSource>(), sut2.GetRequiredService<IDataSource>());
        }

        [Fact]
        public void AddDapperDataSource_ShouldAddDefaultImplementationWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDapperDataSource<DbMarker>(o => o.ConnectionFactory = () => new SqliteConnection("Data Source=:memory:"));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DapperDataSource<DbMarker>>(sut2.GetRequiredService<IDapperDataSource<DbMarker>>());
            Assert.IsType<DapperDataSource<DbMarker>>(sut2.GetRequiredService<IDataSource<DbMarker>>());
            Assert.Same(sut2.GetRequiredService<IDapperDataSource<DbMarker>>(), sut2.GetRequiredService<IDataSource<DbMarker>>());
        }
    }
}
