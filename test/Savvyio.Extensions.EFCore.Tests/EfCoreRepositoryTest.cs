using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.EFCore
{
    public class EfCoreRepositoryTest : Test
    {
        public EfCoreRepositoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EfCoreRepository_ShouldFailWithInvalidModel()
        {
            var sut1 = new EfCoreDataSource<Account>(new EfCoreDataSourceOptions<Account>()
            {
	            ContextConfigurator = b => b.UseInMemoryDatabase("Dummy")
            });
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

            var sut1 = new EfCoreDataSource(new EfCoreDataSourceOptions()
            {
                ContextConfigurator = b => b.UseInMemoryDatabase("Dummy"),
                ModelConstructor = mb => mb.AddAccount()
            });

            var sut2 = new EfCoreRepository<Account, long>(sut1);
            sut2.Add(new Account(id, name, email));
            await sut1.SaveChangesAsync();

            var sut3 = await sut2.FindAllAsync(a => a.PlatformProviderId == id).SingleOrDefaultAsync();

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

            var sut1 = new EfCoreDataSource(new EfCoreDataSourceOptions()
            {
                ContextConfigurator = b => b.UseInMemoryDatabase("Dummy"),
                ModelConstructor = mb => mb.AddAccount()
            });

            var sut2 = new EfCoreRepository<Account, long>(sut1);
            sut2.Add(entity);
            await sut1.SaveChangesAsync();
            
            var sut3 = await sut2.FindAllAsync(a => a.PlatformProviderId == id).SingleOrDefaultAsync();

            sut2.Remove(entity);
            await sut1.SaveChangesAsync();

            var sut4 = await sut2.FindAllAsync(a => a.PlatformProviderId == id).SingleOrDefaultAsync();

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

            var sut1 = new EfCoreDataSource(new EfCoreDataSourceOptions()
            {
                ContextConfigurator = b => b.UseInMemoryDatabase("Dummy"),
                ModelConstructor = mb => mb.AddAccount()
            });

            var events = new List<string>();

            sut1.DbContext.ChangeTracker.StateChanged += (sender, args) =>
            {
                var trace = $"{sender}: {args.OldState} --> {args.NewState}";
                events.Add(trace);
                TestOutput.WriteLine(trace);
            };

            var sut2 = new EfCoreRepository<Account, long>(sut1);
            
            sut2.Add(entity);
            
            await sut1.SaveChangesAsync();
            
            var sut3 = await sut2.FindAllAsync(a => a.PlatformProviderId == id).SingleOrDefaultAsync();

            Assert.Equal(entity, sut3);

            sut3.ChangeFullName(newName);

            await sut1.SaveChangesAsync(); // modified

            var sut4 = await sut2.FindAllAsync(a => a.PlatformProviderId == id).SingleOrDefaultAsync();

            Assert.NotEqual(name, sut3.FullName);
            Assert.Equal(newName, sut4.FullName);
            Assert.Collection(events,
                s => Assert.Equal("Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker: Added --> Unchanged", s),
                s => Assert.Equal("Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker: Unchanged --> Modified", s),
                s => Assert.Equal("Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker: Modified --> Unchanged", s));
        }
    }
}
