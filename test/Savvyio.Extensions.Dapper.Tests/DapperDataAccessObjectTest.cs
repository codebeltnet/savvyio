using System;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Dapper;
using Microsoft.Data.Sqlite;
using Savvyio.Assets.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Dapper
{
    public class DapperDataAccessObjectTest : Test
    {
        public DapperDataAccessObjectTest(ITestOutputHelper output) : base(output)
        {
            SqlMapper.AddTypeHandler(new GuidHandler());
        }

        [Fact]
        public async Task DapperDataAccessObject_ShouldCreateObject()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";

            var sut1 = new DapperDataStore(o =>
            {
                o.ConnectionFactory = () =>
                {
                    var cnn = new SqliteConnection("Data Source=:memory:");
                    cnn.Open();
                    cnn.ExecuteAsync("CREATE TABLE Account (Id INTEGER PRIMARY KEY, PlatformProviderId VARCHAR(36), FullName VARCHAR(255), EmailAddress VARCHAR(512))");
                    return cnn;
                };
            });

            var sut2 = new DapperDataAccessObject<Account>(sut1);
            await sut2.CreateAsync(new Account(id, name, email), o => o.CommandText = "INSERT INTO Account (PlatformProviderId, FullName, EmailAddress) VALUES (@PlatformProviderId, @FullName, @EmailAddress)");
            
            var sut3 = await sut2.ReadAsync(a => a.PlatformProviderId == id, o => o.CommandText = "SELECT * FROM Account");

            sut2.Dispose();

            Assert.True(sut1.Disposed, "sut1.Disposed");
            Assert.True(sut2.Disposed, "sut2.Disposed");
            Assert.Equal(id, sut3.PlatformProviderId);
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

            var sut1 = new DapperDataStore(o =>
            {
                o.ConnectionFactory = () =>
                {
                    var cnn = new SqliteConnection("Data Source=:memory:");
                    cnn.Open();
                    cnn.ExecuteAsync("CREATE TABLE Account (Id INTEGER PRIMARY KEY, PlatformProviderId VARCHAR(36), FullName VARCHAR(255), EmailAddress VARCHAR(512))");
                    return cnn;
                };
            });

            var sut2 = new DapperDataAccessObject<Account>(sut1);
            await sut2.CreateAsync(dto, o => o.CommandText = "INSERT INTO Account (PlatformProviderId, FullName, EmailAddress) VALUES (@PlatformProviderId, @FullName, @EmailAddress)");
            
            var sut3 = await sut2.ReadAsync(a => a.PlatformProviderId == id, o => o.CommandText = "SELECT * FROM Account");

            await sut2.DeleteAsync(sut3, o => o.CommandText = "DELETE FROM Account WHERE Id = @Id");

            var sut4 = await sut2.ReadAsync(a => a.PlatformProviderId == id, o => o.CommandText = "SELECT * FROM Account");

            sut2.Dispose();

            Assert.True(sut1.Disposed, "sut1.Disposed");
            Assert.True(sut2.Disposed, "sut2.Disposed");
            Assert.NotNull(sut3);
            Assert.Null(sut4);
        }

        [Fact]
        public async Task DapperDataAccessObject_ShouldUpdateObject()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var newName = "Unit Test";
            var email = "test@unit.test";
            var dto = new Account(id, name, email);

            var sut1 = new DapperDataStore(o =>
            {
                o.ConnectionFactory = () =>
                {
                    var cnn = new SqliteConnection("Data Source=:memory:");
                    cnn.Open();
                    cnn.ExecuteAsync("CREATE TABLE Account (Id INTEGER PRIMARY KEY, PlatformProviderId VARCHAR(36), FullName VARCHAR(255), EmailAddress VARCHAR(512))");
                    return cnn;
                };
            });

            var sut2 = new DapperDataAccessObject<Account>(sut1);
            await sut2.CreateAsync(dto, o => o.CommandText = "INSERT INTO Account (PlatformProviderId, FullName, EmailAddress) VALUES (@PlatformProviderId, @FullName, @EmailAddress)");
            
            var sut3 = await sut2.ReadAsync(a => a.PlatformProviderId == id, o => o.CommandText = "SELECT * FROM Account");

            sut3.ChangeFullName(newName);

            await sut2.UpdateAsync(sut3, o => o.CommandText = "UPDATE Account SET FullName = @FullName");

            var sut4 = await sut2.ReadAsync(a => a.PlatformProviderId == id, o => o.CommandText = "SELECT * FROM Account");

            sut2.Dispose();

            Assert.True(sut1.Disposed, "sut1.Disposed");
            Assert.True(sut2.Disposed, "sut2.Disposed");
            Assert.NotEqual(name, sut3.FullName);
            Assert.Equal(newName, sut4.FullName);
        }
    }
}
