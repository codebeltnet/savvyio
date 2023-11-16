using System;
using System.Globalization;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Savvyio.EventDriven.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.EventDriven.Messaging.Cryptography
{
    public class IntegrationEventExtensionsTest : Test
    {
        public IntegrationEventExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EncloseToSignedMessage_ShouldSerialize_WithSignature()
        {
            var utc = DateTime.Parse("2023-11-16T23:24:17.8414532Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com").SetEventId("69bccf3b1117425397c5ed9ed757bb0f").SetTimestamp(utc);
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utc;
            }).Sign(o =>
            {
                o.SerializerFactory = message => JsonFormatter.SerializeObject(message);
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });
            var json = JsonFormatter.SerializeObject(sut2);
            var jsonString = json.ToEncodedString();
            TestOutput.WriteLine(jsonString);

            Assert.Equal("""
                         {
                           "id": "2d4030d32a254ee8a27046e5bafe696a",
                           "source": "https://fancy.api/members",
                           "type": "Savvyio.EventDriven.Assets.MemberCreated, Savvyio.EventDriven.Tests",
                           "time": "2023-11-16T23:24:17.8414532Z",
                           "data": {
                             "name": "Jane Doe",
                             "emailAddress": "jd@office.com",
                             "metadata": {
                               "eventId": "69bccf3b1117425397c5ed9ed757bb0f",
                               "timestamp": "2023-11-16T23:24:17.8414532Z"
                             }
                           },
                           "signature": "ce716b47cb6405e24be4200b12a5fc75d7e453189998352c8df5fadc81640018"
                         }
                         """, jsonString);
        }
    }
}
