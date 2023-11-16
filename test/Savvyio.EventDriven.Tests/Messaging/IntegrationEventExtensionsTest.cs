using System;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Savvyio.EventDriven.Assets;
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
        public void EncloseToMessage_CreateMemberCommand_ShouldBeWrappedInMessage()
        {
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com");
            var sut2 = "urn:member-events:1".ToUri();
            var sut3 = sut1.ToMessage(sut2);

            Assert.Equal(sut1.EmailAddress, sut3.Data.EmailAddress);
            Assert.Equal(sut1.Name, sut3.Data.Name);
            Assert.NotNull(sut3.Id);
            Assert.NotNull(sut3.Time);
            Assert.Equal(sut1.GetType().ToFullNameIncludingAssemblyName(), sut3.Type);
            Assert.Equal(sut2.OriginalString, sut3.Source);
        }

        [Fact]
        public void EncloseToSignedMessage_ShouldSerialize_WithoutSignature()
        {
            var utcNow = DateTime.UtcNow;
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com").SetEventId("69bccf3b1117425397c5ed9ed757bb0f").SetTimestamp(utcNow);
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utcNow;
            });
            var json = JsonFormatter.SerializeObject(sut2);
            var jsonString = json.ToEncodedString();
            TestOutput.WriteLine(jsonString);

            Assert.Equal($$"""
                           {
                             "id": "2d4030d32a254ee8a27046e5bafe696a",
                             "source": "https://fancy.api/members",
                             "type": "Savvyio.EventDriven.Assets.MemberCreated, Savvyio.EventDriven.Tests",
                             "time": "{{utcNow:O}}",
                             "data": {
                               "name": "Jane Doe",
                               "emailAddress": "jd@office.com",
                               "metadata": {
                                 "eventId": "69bccf3b1117425397c5ed9ed757bb0f",
                                 "timestamp": "{{utcNow:O}}"
                               }
                             }
                           }
                           """, jsonString);
        }
    }
}
