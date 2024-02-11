using Cuemon.Extensions.IO;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets;
using Savvyio.Commands;
using Savvyio.Handlers;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio
{
    public class OrphanedHandlerExceptionTest : Test
    {
        public OrphanedHandlerExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldBeSerializableAndHaveCorrectDefaultMessage()
        {
            var sut = OrphanedHandlerException.Create<ICommand, ICommandHandler>(new FakeCommand(), "fake");

            using (var stream = JsonFormatter.SerializeObject(sut))
            {
                TestOutput.WriteLine(stream.ToEncodedString(o => o.LeaveOpen = true));

                var desEx = JsonFormatter.DeserializeObject<OrphanedHandlerException>(stream);

                Assert.Equal(sut.Message, desEx.Message);
                Assert.Equal(sut.ToString(), desEx.ToString());
            }

        }
    }
}
