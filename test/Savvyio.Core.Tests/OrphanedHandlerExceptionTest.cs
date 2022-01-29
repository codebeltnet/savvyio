using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

            TestOutput.WriteLine(sut.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                bf.Serialize(ms, sut);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                ms.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var desEx = bf.Deserialize(ms) as OrphanedHandlerException;
#pragma warning restore SYSLIB0011 // Type or member is obsolete 
                Assert.Equal(sut.Message, desEx.Message);
                Assert.Equal(sut.ToString(), desEx.ToString());
            }

        }
    }
}
