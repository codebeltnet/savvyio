using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Xunit;
using Savvyio.EventDriven.Messaging.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.EventDriven.Messaging
{
    public class IntegrationEventExtensionsTest : Test
    {
        public IntegrationEventExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Enclose_CreateMemberCommand_ShouldBeWrappedInMessage()
        {
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com");
            var sut2 = "urn:member-events:1".ToUri();
            var sut3 = sut1.EncloseToMessage(sut2);

            Assert.Equal(sut1.EmailAddress, sut3.Data.EmailAddress);
            Assert.Equal(sut1.Name, sut3.Data.Name);
            Assert.NotNull(sut3.Id);
            Assert.NotNull(sut3.Time);
            Assert.Equal(sut1.GetType().ToFullNameIncludingAssemblyName(), sut3.Type);
            Assert.Equal(sut2.OriginalString, sut3.Source);
        }
    }
}
