using Cuemon.Extensions.Xunit;
using Savvyio.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Queries
{
    public class SavvyioOptionsExtensionsTest : Test
    {
        public SavvyioOptionsExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddHandlers_ShouldNotAddAnyHandlersFromCurrentDomain()
        {
            var sut = new SavvyioOptions().AddHandlers();
            Assert.Empty(sut.HandlerServiceTypes);
            Assert.Empty(sut.HandlerImplementationTypes);
        }

        [Fact]
        public void AddQueryHandler_ShouldAddQueryHandler()
        {
            var sut = new SavvyioOptions().AddQueryHandler<AccountQueryHandler>();

            Assert.NotEmpty(sut.HandlerServiceTypes);
            Assert.NotEmpty(sut.HandlerImplementationTypes);
            Assert.Collection(sut.HandlerServiceTypes, type => Assert.Equal(typeof(IQueryHandler), type));
            Assert.Collection(sut.HandlerImplementationTypes, type => Assert.Equal(typeof(AccountQueryHandler), type));
        }

        [Fact]
        public void AddQueryDispatcher_ShouldAddQueryDispatcher()
        {
            var sut = new SavvyioOptions().AddQueryDispatcher();

            Assert.NotEmpty(sut.DispatcherServiceTypes);
            Assert.NotEmpty(sut.DispatcherImplementationTypes);
            Assert.Collection(sut.DispatcherServiceTypes, type => Assert.Equal(typeof(IQueryDispatcher), type));
            Assert.Collection(sut.DispatcherImplementationTypes, type => Assert.Equal(typeof(QueryDispatcher), type));
        }
    }
}
