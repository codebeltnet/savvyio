using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.EventDriven;
using Xunit;

namespace Savvyio.EventDriven.Messaging
{
    public class IntegrationEventExtensionsTest : Test
    {
        public IntegrationEventExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EncloseToMessage_CreateMemberCommand_ShouldBeWrappedInMessage()
        {
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com");
            var sut2 = "urn:member-events:1".ToUri();
            var sut3 = sut1.ToMessage(sut2, nameof(MemberCreated));

            Assert.Equal(sut1.EmailAddress, sut3.Data.EmailAddress);
            Assert.Equal(sut1.Name, sut3.Data.Name);
            Assert.NotNull(sut3.Id);
            Assert.NotNull(sut3.Time);
            Assert.Equal(sut1.GetType().ToFullNameIncludingAssemblyName(), sut3.Data.GetMemberType());
            Assert.Equal(nameof(MemberCreated), sut3.Type);
            Assert.Equal(sut2.OriginalString, sut3.Source);
        }
    }
}
