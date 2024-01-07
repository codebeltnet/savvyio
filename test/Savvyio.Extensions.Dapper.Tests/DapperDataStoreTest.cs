using System;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Microsoft.Data.Sqlite;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Extensions.Dapper.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Dapper
{
    public class DapperDataStoreTest : Test
    {
        public DapperDataStoreTest(ITestOutputHelper output) : base(output)
        {
            
        }

        [Fact]
        public async Task DapperDataAccessObject_ShouldCreateObject()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";

            var options = new DapperDataSourceOptions()
            {
				ConnectionFactory = () => new SqliteConnection("Data Source=:memory:").SetDefaults().AddAccountTable()
			};

            var sut1 = new DapperDataSource(options);

            var sut2 = new AccountData(sut1);
            await sut2.CreateAsync(new Account(id, name, email));
            
            var sut3 = await sut2.FindAllAsync(o => o.CommandText = "SELECT * FROM AccountProjection WHERE Id = 1").SingleOrDefaultAsync();

            sut2.Dispose();

            Assert.True(sut1.Disposed, "sut1.Disposed");
            Assert.True(sut2.Disposed, "sut2.Disposed");
            Assert.Equal(name, sut3.FullName);
            Assert.Equal(email, sut3.EmailAddress);
        }

        [Fact]
        public async Task DapperDataAccessObject_ShouldRemoveObject()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";
            var dto = new Account(id, name, email);

            var options = new DapperDataSourceOptions()
            {
	            ConnectionFactory = () => new SqliteConnection("Data Source=:memory:").SetDefaults().AddAccountTable()
            };

			var sut1 = new DapperDataSource(options);

            var sut2 = new AccountData(sut1);
            await sut2.CreateAsync(dto);
            
            var sut3 = await sut2.FindAllAsync(o => o.CommandText = "SELECT * FROM AccountProjection WHERE Id = 1").SingleOrDefaultAsync();

            await sut2.DeleteAsync(sut3);

            var sut4 = await sut2.FindAllAsync(o => o.CommandText = "SELECT * FROM AccountProjection");

            sut2.Dispose();

            Assert.True(sut1.Disposed, "sut1.Disposed");
            Assert.True(sut2.Disposed, "sut2.Disposed");
            Assert.NotNull(sut3);
            Assert.Empty(sut4);
        }

        [Fact]
        public async Task DapperDataAccessObject_ShouldUpdateObject()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var newName = "Unit Test";
            var email = "test@unit.test";
            var dto = new Account(id, name, email);

            var sut1 = new DapperDataSource(new DapperDataSourceOptions()
            {
                ConnectionFactory = () => new SqliteConnection("Data Source=:memory:").SetDefaults().AddAccountTable()
            });

            var sut2 = new AccountData(sut1);
            await sut2.CreateAsync(dto);
            
            var sut3 = await sut2.FindAllAsync(o => o.CommandText = "SELECT * FROM AccountProjection WHERE Id = 1").SingleOrDefaultAsync();

            sut3.ChangeFullName(newName);

            await sut2.UpdateAsync(sut3);

            var sut4 = await sut2.FindAllAsync(o =>
            {
                o.Parameters = new { sut3.Id };
                o.CommandText = "SELECT * FROM AccountProjection WHERE Id = @Id";
            }).SingleOrDefaultAsync();

            sut2.Dispose();

            Assert.True(sut1.Disposed, "sut1.Disposed");
            Assert.True(sut2.Disposed, "sut2.Disposed");
            Assert.NotEqual(name, sut3.FullName);
            Assert.Equal(newName, sut4.FullName);
        }
    }
}
