using Codebelt.Extensions.Xunit;
using Savvyio.Commands.Assets;
using Xunit;

namespace Savvyio.Commands
{
    public class CommandTest : Test
    {
        public CommandTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DefaultCommand_Ensure_Initialization_Defaults()
        {
            var sut = new DefaultCommand();

            Assert.IsAssignableFrom<Command>(sut);
            Assert.IsAssignableFrom<ICommand>(sut);
            Assert.IsAssignableFrom<Request>(sut);
            Assert.IsAssignableFrom<IRequest>(sut);
            Assert.IsAssignableFrom<IMetadata>(sut);
            Assert.Contains(sut.Metadata, pair => pair.Key == MetadataDictionary.CorrelationId);
        }
    }
}
