using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Queries;
using Savvyio.Queries.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Queries
{
    public class QueryHandlerTest : Test
    {
        public QueryHandlerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task QueryHandler_Ensure_Initialization_Defaults()
        {
            var sut = new DefaultQueryHandler();

            Assert.IsAssignableFrom<QueryHandler>(sut);
            Assert.IsAssignableFrom<IQueryHandler>(sut);
            Assert.True(sut.Delegates.TryInvoke<string>(new DefaultQuery<string>(), out _));
            Assert.True((await sut.Delegates.TryInvokeAsync<string>(new DefaultQuery<string>())).Succeeded);
            Assert.False(sut.Delegates.TryInvoke<string>(new GetAccount(1), out _));
            Assert.False((await sut.Delegates.TryInvokeAsync<string>(new GetAccount(1))).Succeeded);
        }
    }
}
