using System;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Storage
{
    public class EfCoreRepositoryTest : Test
    {
        public EfCoreRepositoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EfCoreRepository_ShouldFailWithInvalidModel()
        {
            var sut1 = new EfCoreDataStore<Account>(null, o => o.ContextConfigurator = b => b.UseInMemoryDatabase("Dummy"));
            var sut2 = new EfCoreRepository<Account, long, Account>(sut1);
            var ex = Assert.Throws<InvalidOperationException>(() => sut2.Add(new Account(1)));
            Assert.StartsWith("Cannot create a DbSet for 'Account' because this type is not included in the model for the context.", ex.Message);
        }

        [Fact]
        public async Task EfCoreRepository_ShouldAddEntity()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";

            var sut1 = new EfCoreDataStore(null, o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = mb => mb.AddAccount();
            });

            var sut2 = new EfCoreRepository<Account, long>(sut1);
            sut2.Add(new Account(id, name, email));
            
            var sut3 = await sut2.FindAsync(a => a.PlatformProviderId == id);

            Assert.Equal(id, sut3.PlatformProviderId);
            Assert.Equal(name, sut3.FullName);
            Assert.Equal(email, sut3.EmailAddress);
        }

        [Fact]
        public async Task EfCoreRepository_ShouldRemoveEntity()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";
            var entity = new Account(id, name, email);

            var sut1 = new EfCoreDataStore(null, o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = mb => mb.AddAccount();
            });

            var sut2 = new EfCoreRepository<Account, long>(sut1);
            sut2.Add(entity);
            
            var sut3 = await sut2.FindAsync(a => a.PlatformProviderId == id);

            sut2.Remove(entity);

            var sut4 = await sut2.FindAsync(a => a.PlatformProviderId == id);

            Assert.NotNull(sut3);
            Assert.Null(sut4);
        }

        [Fact]
        public async Task EfCoreRepository_ShouldUpdateEntity()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var newName = "Unit Test";
            var email = "test@unit.test";
            var entity = new Account(id, name, email);

            var sut1 = new EfCoreDataStore(null, o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = mb => mb.AddAccount();
            });

            var sut2 = new EfCoreRepository<Account, long>(sut1);
            sut2.Add(entity);
            
            var sut3 = await sut2.FindAsync(a => a.PlatformProviderId == id);

            sut3.ChangeFullName(newName);

            sut2.Add(sut3);

            var sut4 = await sut2.FindAsync(a => a.PlatformProviderId == id);

            Assert.NotEqual(name, sut3.FullName);
            Assert.Equal(newName, sut4.FullName);
        }
    }
}
