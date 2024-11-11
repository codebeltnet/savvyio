using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using DapperExtensions;
using DapperExtensions.Sql;
using Microsoft.Data.Sqlite;
using Savvyio.Assets;
using Savvyio.Assets.Queries;
using Savvyio.Extensions.Dapper;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.DapperExtensions
{
    public class DapperExtensionsDataStoreTest : Test
    {
        public DapperExtensionsDataStoreTest(ITestOutputHelper output) : base(output)
        {
            DapperAsyncExtensions.SqlDialect = new SqliteDialect();
        }

        [Fact]
        public async Task DataStore_ShouldCreateObject()
        {
            var name = "Test";
            var email = "test@unit.test";

            var options = new DapperDataSourceOptions()
            {
                ConnectionFactory = () => new SqliteConnection("Data Source=:memory:").SetDefaults().AddAccountTable()
            };

            var sut1 = new DapperDataSource(options);

            var sut2 = new DapperExtensionsDataStore<AccountProjection>(sut1);
            await sut2.CreateAsync(new AccountProjection(name, email));

            var sut3 = await sut2.GetByIdAsync(1);

            sut2.Dispose();

            Assert.True(sut1.Disposed, "sut1.Disposed");
            Assert.True(sut2.Disposed, "sut2.Disposed");
            Assert.Equal(name, sut3.FullName);
            Assert.Equal(email, sut3.EmailAddress);
        }

        [Fact]
        public async Task DataStore_ShouldRemoveObject()
        {
            var name = "Test";
            var email = "test@unit.test";
            var dto = new AccountProjection(name, email);

            var sut1 = new DapperDataSource(new DapperDataSourceOptions()
            {
                ConnectionFactory = () => new SqliteConnection("Data Source=:memory:").SetDefaults().AddAccountTable()
            });

            var sut2 = new DapperExtensionsDataStore<AccountProjection>(sut1);
            await sut2.CreateAsync(dto);

            var sut3 = await sut2.GetByIdAsync(1);

            await sut2.DeleteAsync(sut3);

            var sut4 = await sut2.FindAllAsync();

            var sut5 = await sut2.GetByIdAsync(1);

            sut2.Dispose();

            Assert.True(sut1.Disposed, "sut1.Disposed");
            Assert.True(sut2.Disposed, "sut2.Disposed");
            Assert.NotNull(sut3);
            Assert.Empty(sut4);
            Assert.Null(sut5);
        }

        [Fact]
        public async Task DataStore_ShouldUpdateObject()
        {
            var name = "Test";
            var newName = "Unit Test";
            var email = "test@unit.test";
            var dto = new AccountProjection(name, email);

            var sut1 = new DapperDataSource(new DapperDataSourceOptions()
            {
                ConnectionFactory = () => new SqliteConnection("Data Source=:memory:").SetDefaults().AddAccountTable()
            });

            var sut2 = new DapperExtensionsDataStore<AccountProjection>(sut1);
            await sut2.CreateAsync(dto);

            var sut3 = await sut2.GetByIdAsync(1);

            sut3.FullName = newName;

            await sut2.UpdateAsync(sut3);

            var sut4 = await sut2.FindAllAsync(o =>
            {
                o.Predicate = a => a.Id;
                o.Value = sut3.Id;
            }).SingleOrDefaultAsync();

            sut2.Dispose();

            Assert.True(sut1.Disposed, "sut1.Disposed");
            Assert.True(sut2.Disposed, "sut2.Disposed");
            Assert.NotEqual(name, sut3.FullName);
            Assert.Equal(newName, sut4.FullName);
        }
    }
}
