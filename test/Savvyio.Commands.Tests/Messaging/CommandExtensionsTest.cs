using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Commands;
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
        public void EncloseToMessage_CreateMemberCommand_ShouldBeWrappedInMessage()
        {
            var sut1 = new CreateMemberCommand("Jane Doe", 21, "jd@office.com");
            var sut2 = "https://fancy.api/members".ToUri();
            var sut3 = "com.example.someevent";
            var sut4 = sut1.ToMessage(sut2, sut3);

            Assert.Equal(sut1.EmailAddress, sut4.Data.EmailAddress);
            Assert.Equal(sut1.Age, sut4.Data.Age);
            Assert.Equal(sut1.Name, sut4.Data.Name);
            Assert.NotNull(sut4.Id);
            Assert.NotNull(sut4.Time);
            Assert.Equal(sut1.GetType().ToFullNameIncludingAssemblyName(), sut4.Data.GetMemberType());
            Assert.Equal(sut2.OriginalString, sut4.Source);
            Assert.Equal(sut3, sut4.Type);
        }
    }
}
