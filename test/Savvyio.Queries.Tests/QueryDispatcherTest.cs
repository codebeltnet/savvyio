using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Dispatchers;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Handlers;
using Savvyio.Queries.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Queries
{
    public class QueryDispatcherTest : Test
    {
        public QueryDispatcherTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void QueryDispatcher_Ensure_Initialization_Defaults()
        {
            var sp = new ServiceCollection().AddServiceLocator().BuildServiceProvider();
            var sut = new QueryDispatcher(sp.GetRequiredService<IServiceLocator>());

            Assert.Throws<OrphanedHandlerException>(() => sut.Query(new DefaultQuery<string>()));
            Assert.ThrowsAsync<OrphanedHandlerException>(() => sut.QueryAsync(new DefaultQuery<string>()));
            Assert.IsAssignableFrom<RequestReplyDispatcher>(sut);
            Assert.IsAssignableFrom<IQueryDispatcher>(sut);
            Assert.IsAssignableFrom<Dispatcher>(sut);
            Assert.IsAssignableFrom<IDispatcher>(sut);
        }

        [Fact]
        public async Task QueryDispatcher_ShouldQuery_DefaultQuery()
        {
            var sp = new ServiceCollection().AddServiceLocator().AddTransient<IQueryHandler, DefaultQueryHandler>().AddSingleton<ITestStore<string>, InMemoryTestStore<string>>().BuildServiceProvider();
            var sut = new QueryDispatcher(sp.GetRequiredService<IServiceLocator>());
            var ts = sp.GetRequiredService<ITestStore<string>>();

            var qr = sut.Query(new DefaultQuery<string>());
            var aqr = await sut.QueryAsync(new DefaultQuery<string>());

            Assert.Equal(qr, nameof(DefaultQuery<string>));
            Assert.Equal(aqr, nameof(DefaultQuery<string>) + "Async");
        }
    }
}
