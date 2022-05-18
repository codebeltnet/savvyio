using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Queries;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Handlers;
using Savvyio.Queries;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Dispatchers
{
    public class RequestReplyDispatcherTest : Test
    {
        public RequestReplyDispatcherTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Dispatch_ShouldFailWithOrphanedHandlerException()
        {
            var sc = new ServiceCollection().AddSavvyIO(o => o.AddAssemblyToScan(typeof(RequestReplyDispatcherTest).Assembly));
            var sp = sc.BuildServiceProvider();
            var sut = new QueryDispatcher(sp.GetRequiredService<IServiceLocator>());

            Assert.Throws<OrphanedHandlerException>(() => sut.Query(new FakeQuery()));
        }

        [Fact]
        public void Dispatch_ShouldDispatchCommandToDesignatedHandler()
        {
            var sc = new ServiceCollection()
                .AddSavvyIO(o => o.AddAssemblyRangeToScan(typeof(RequestReplyDispatcherTest).Assembly, typeof(IQuery).Assembly).EnableAutomaticDispatcherDiscovery().EnableAutomaticHandlerDiscovery())
                .AddScoped<ITestStore<IQuery>, InMemoryTestStore<IQuery>>();
            
            var sp = sc.BuildServiceProvider();
            var sut = new QueryDispatcher(sp.GetRequiredService<IServiceLocator>());
            var ga = new GetAccount(2313);
            var cs = sp.GetRequiredService<ITestStore<IQuery>>();
            
            var result = sut.Query(ga);

            Assert.NotNull(result);
            Assert.Equal(ga.Id, result.Id);
            Assert.Equal(ga.Id, cs.QueryFor<GetAccount>().Single().Id);
        }

        [Fact]
        public async Task DispatchAsync_ShouldFailWithOrphanedHandlerExceptionAsync()
        {
            var sc = new ServiceCollection().AddSavvyIO(o => o.AddAssemblyToScan(typeof(RequestReplyDispatcherTest).Assembly));
            var sp = sc.BuildServiceProvider();
            var sut = new QueryDispatcher(sp.GetRequiredService<IServiceLocator>());

            await Assert.ThrowsAsync<OrphanedHandlerException>(async () => await sut.QueryAsync(new FakeQuery()));
        }

        [Fact]
        public async Task DispatchAsync_ShouldDispatchCommandToDesignatedHandlerAsync()
        {
            var sc = new ServiceCollection()
                .AddSavvyIO(o => o.AddAssemblyRangeToScan(typeof(RequestReplyDispatcherTest).Assembly, typeof(IQuery).Assembly).EnableAutomaticDispatcherDiscovery().EnableAutomaticHandlerDiscovery())
                .AddScoped<ITestStore<IQuery>, InMemoryTestStore<IQuery>>();
            
            var sp = sc.BuildServiceProvider();
            var sut = new QueryDispatcher(sp.GetRequiredService<IServiceLocator>());
            var ga = new GetAccount(74893297432);
            var cs = sp.GetRequiredService<ITestStore<IQuery>>();
            
            var result = await sut.QueryAsync(ga);

            Assert.NotNull(result);
            Assert.StartsWith(ga.Id.ToString(), result.EmailAddress);
            Assert.Equal(ga.Id, cs.QueryFor<GetAccount>().Single().Id);
        }
    }
}
