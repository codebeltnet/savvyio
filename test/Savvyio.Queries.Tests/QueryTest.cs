using Cuemon.Extensions.Reflection;
using Codebelt.Extensions.Xunit;
using Savvyio.Queries.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Queries
{
    public class QueryTest : Test
    {
        public QueryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DefaultQuery_Ensure_Initialization_Defaults()
        {
            var sut = new DefaultQuery<string>();

            Assert.IsAssignableFrom<Query<string>>(sut);
            Assert.IsAssignableFrom<IQuery>(sut);
            Assert.IsAssignableFrom<IQuery<string>>(sut);
            Assert.IsAssignableFrom<Request>(sut);
            Assert.IsAssignableFrom<IRequest>(sut);
            Assert.IsAssignableFrom<IMetadata>(sut);
            Assert.Collection(sut.Metadata, pair =>
            {
                Assert.Equal(MetadataDictionary.MemberType, pair.Key);
                Assert.Equal(typeof(DefaultQuery<string>).ToFullNameIncludingAssemblyName(), pair.Value);
            });
        }
    }
}
