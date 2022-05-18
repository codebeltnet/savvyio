using System;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Extensions.EFCore;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Storage
{
    public class EfCoreDataAccessObjectTest : Test
    {
        public EfCoreDataAccessObjectTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task EfCoreDataAccessObject_ShouldCreateObject()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";

            var sut1 = new EfCoreDataStore(o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = mb => mb.AddAccount();
            });

            var sut2 = new EfCoreDataAccessObject<Account>(sut1);
            await sut2.CreateAsync(new Account(id, name, email));

            var sut3 = await sut2.ReadAsync(o => o.Predicate = a => a.PlatformProviderId == id);

            Assert.Equal(id, sut3.PlatformProviderId);
            Assert.Equal(name, sut3.FullName);
            Assert.Equal(email, sut3.EmailAddress);
        }

        [Fact]
        public async Task EfCoreDataAccessObject_ShouldRemoveObject()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";
            var dto = new Account(id, name, email);

            var sut1 = new EfCoreDataStore(o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = mb => mb.AddAccount();
            });

            var sut2 = new EfCoreDataAccessObject<Account>(sut1);
            await sut2.CreateAsync(dto);

            var sut3 = await sut2.ReadAsync(o => o.Predicate = a => a.PlatformProviderId == id);

            await sut2.DeleteAsync(dto);

            var sut4 = await sut2.ReadAsync(o => o.Predicate = a => a.PlatformProviderId == id);

            Assert.NotNull(sut3);
            Assert.Null(sut4);
        }

        [Fact]
        public async Task EfCoreDataAccessObject_ShouldUpdateObject()
        {
            var id = Guid.NewGuid();
            var name = "Test";
            var newName = "Unit Test";
            var email = "test@unit.test";
            var dto = new Account(id, name, email);

            var sut1 = new EfCoreDataStore(o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = mb => mb.AddAccount();
            });

            var sut2 = new EfCoreDataAccessObject<Account>(sut1);
            await sut2.CreateAsync(dto);

            var sut3 = await sut2.ReadAsync(o => o.Predicate = a => a.PlatformProviderId == id);

            sut3.ChangeFullName(newName);

            await sut2.UpdateAsync(sut3);

            var sut4 = await sut2.ReadAsync(o => o.Predicate = a => a.PlatformProviderId == id);

            Assert.NotEqual(name, sut3.FullName);
            Assert.Equal(newName, sut4.FullName);
        }
    }
}
