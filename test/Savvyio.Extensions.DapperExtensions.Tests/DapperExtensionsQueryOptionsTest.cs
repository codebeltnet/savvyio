using Cuemon.Extensions.Xunit;
using DapperExtensions.Predicate;
using Savvyio.Assets.Queries;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.DapperExtensions
{
    public class DapperExtensionsQueryOptionsTest : Test
    {
        public DapperExtensionsQueryOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DapperOptions_Ensure_Initialization_Defaults()
        {
            var sut = new DapperExtensionsQueryOptions<AccountProjection>();

            Assert.Null(sut.Value);
            Assert.Equal(Operator.Eq, sut.Op);
            Assert.Equal(DatabaseFunction.None, sut.Function);
            Assert.Null(sut.FunctionParameters);
            Assert.Null(sut.Predicate);
            Assert.Equal(default, sut.CancellationToken);
            Assert.False(sut.Not);
            Assert.True(sut.UseColumnPrefix);

        }
    }
}
