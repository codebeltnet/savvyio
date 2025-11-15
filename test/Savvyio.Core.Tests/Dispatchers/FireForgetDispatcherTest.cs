using System;
using System.Linq;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Commands;
using Savvyio.Commands;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Handlers;
using Xunit;

namespace Savvyio.Dispatchers
{
    public class FireForgetDispatcherTest : Test
    {
        public FireForgetDispatcherTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Dispatch_ShouldFailWithOrphanedHandlerException()
        {
            var sc = new ServiceCollection().AddSavvyIO(o => o.AddAssemblyToScan(typeof(FireForgetDispatcherTest).Assembly));
            var sp = sc.BuildServiceProvider();
            var sut = new CommandDispatcher(sp.GetRequiredService<IServiceLocator>());

            Assert.Throws<OrphanedHandlerException>(() => sut.Commit(new FakeCommand()));
        }

        [Fact]
        public void Dispatch_ShouldDispatchCommandToDesignatedHandler()
        {
            var sc = new ServiceCollection()
                .AddSavvyIO(o => o.AddAssemblyRangeToScan(typeof(FireForgetDispatcherTest).Assembly, typeof(ICommand).Assembly).EnableDispatcherDiscovery().EnableHandlerDiscovery())
                .AddScoped<ITestStore<ICommand>, InMemoryTestStore<ICommand>>();

            var sp = sc.BuildServiceProvider();
            var sut = new CommandDispatcher(sp.GetRequiredService<IServiceLocator>());
            var ca = new CreateAccount(Guid.NewGuid(), "Michael Mortensen", "root@gimlichael.dev");
            var cs = sp.GetRequiredService<ITestStore<ICommand>>();

            sut.Commit(ca);

            Assert.Equal(ca.FullName, cs.QueryFor<CreateAccount>().Single().FullName);
            Assert.Equal(ca.EmailAddress, cs.QueryFor<CreateAccount>().Single().EmailAddress);
            Assert.Equal(ca.PlatformProviderId, cs.QueryFor<CreateAccount>().Single().PlatformProviderId);
        }

        [Fact]
        public async Task DispatchAsync_ShouldFailWithOrphanedHandlerExceptionAsync()
        {
            var sc = new ServiceCollection().AddSavvyIO(o => o.AddAssemblyToScan(typeof(FireForgetDispatcherTest).Assembly));
            var sp = sc.BuildServiceProvider();
            var sut = new CommandDispatcher(sp.GetRequiredService<IServiceLocator>());

            await Assert.ThrowsAsync<OrphanedHandlerException>(async () => await sut.CommitAsync(new FakeCommand()));
        }

        [Fact]
        public async Task DispatchAsync_ShouldDispatchCommandToDesignatedHandlerAsync()
        {
            var sc = new ServiceCollection()
                .AddSavvyIO(o => o.AddAssemblyRangeToScan(typeof(FireForgetDispatcherTest).Assembly, typeof(ICommand).Assembly).EnableDispatcherDiscovery().EnableHandlerDiscovery())
                .AddScoped<ITestStore<ICommand>, InMemoryTestStore<ICommand>>();

            var sp = sc.BuildServiceProvider();
            var sut = new CommandDispatcher(sp.GetRequiredService<IServiceLocator>());
            var ca = new CreateAccount(Guid.NewGuid(), "Michael Mortensen", "root@gimlichael.dev");
            var cs = sp.GetRequiredService<ITestStore<ICommand>>();

            await sut.CommitAsync(ca);

            Assert.Equal(ca.FullName, cs.QueryFor<CreateAccount>().Single().FullName);
            Assert.Equal(ca.EmailAddress, cs.QueryFor<CreateAccount>().Single().EmailAddress);
            Assert.Equal(ca.PlatformProviderId, cs.QueryFor<CreateAccount>().Single().PlatformProviderId);
        }
    }
}
