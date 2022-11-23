using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Xunit;
using Savvyio.Commands.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Commands.Messaging
{
    public class CommandExtensionsTest : Test
    {
        public CommandExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Enclose_CreateMemberCommand_ShouldBeWrappedInMessage()
        {
            var sut1 = new CreateMemberCommand("Jane Doe", 21, "jd@office.com");
            var sut2 = "https://fancy.api/members".ToUri();
            var sut3 = sut1.EncloseToMessage(sut2);

            Assert.Equal(sut1.EmailAddress, sut3.Data.EmailAddress);
            Assert.Equal(sut1.Age, sut3.Data.Age);
            Assert.Equal(sut1.Name, sut3.Data.Name);
            Assert.NotNull(sut3.Id);
            Assert.NotNull(sut3.Time);
            Assert.Equal(sut1.GetType().ToFullNameIncludingAssemblyName(), sut3.Type);
            Assert.Equal(sut2.OriginalString, sut3.Source);
        }
    }
}
