using System;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.EFCore
{
    public class EfCoreDataStoreTest : Test
    {
        public EfCoreDataStoreTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task EfCoreDataStore_ShouldCreateObject()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";

            var sut1 = new EfCoreDataSource(new EfCoreDataSourceOptions()
            {
                ContextConfigurator = b => b.UseInMemoryDatabase("Dummy"),
                ModelConstructor = mb => mb.AddAccount()
            });

            var sut2 = new EfCoreDataStore<Account>(sut1);
            await sut2.CreateAsync(new Account(id, name, email));

            var sut3 = await sut2.FindAllAsync(o => o.Predicate = a => a.PlatformProviderId == id).SingleOrDefaultAsync();

            Assert.Equal(id, sut3.PlatformProviderId);
            Assert.Equal(name, sut3.FullName);
            Assert.Equal(email, sut3.EmailAddress);
        }

        [Fact]
        public async Task EfCoreDataStore_ShouldRemoveObject()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";
            var dto = new Account(id, name, email);

            var sut1 = new EfCoreDataSource(new EfCoreDataSourceOptions()
            {
                ContextConfigurator = b => b.UseInMemoryDatabase("Dummy"),
                ModelConstructor = mb => mb.AddAccount()
            });

            var sut2 = new EfCoreDataStore<Account>(sut1);
            await sut2.CreateAsync(dto);

            var sut3 = await sut2.FindAllAsync(o => o.Predicate = a => a.PlatformProviderId == id).SingleOrDefaultAsync();

            await sut2.DeleteAsync(dto);

            var sut4 = await sut2.FindAllAsync(o => o.Predicate = a => a.PlatformProviderId == id).SingleOrDefaultAsync();

            Assert.NotNull(sut3);
            Assert.Null(sut4);
        }

        [Fact]
        public async Task EfCoreDataStore_ShouldUpdateObject()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var newName = "Unit Test";
            var email = "test@unit.test";
            var dto = new Account(id, name, email);

            var sut1 = new EfCoreDataSource(new EfCoreDataSourceOptions()
            {
                ContextConfigurator = b => b.UseInMemoryDatabase("Dummy"),
                ModelConstructor = mb => mb.AddAccount()
            });

            var sut2 = new EfCoreDataStore<Account>(sut1);
            await sut2.CreateAsync(dto);

            var sut3 = await sut2.FindAllAsync(o => o.Predicate = a => a.PlatformProviderId == id).SingleOrDefaultAsync();

            sut3.ChangeFullName(newName);

            await sut2.UpdateAsync(sut3);

            var sut4 = await sut2.FindAllAsync(o => o.Predicate = a => a.PlatformProviderId == id).SingleOrDefaultAsync();

            Assert.NotEqual(name, sut3.FullName);
            Assert.Equal(newName, sut4.FullName);
        }
    }
}
