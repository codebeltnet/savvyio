using System;
using System.Globalization;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Messaging.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Text.Json.Commands.Messaging.Cryptography
{
    public class MessageExtensionsTest : Test
    {
        public MessageExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EncloseToSignedMessage_ShouldSerialize_WithSignature()
        {
            var utc = DateTime.Parse("2023-11-16T23:24:17.8414532Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var sut1 = new CreateMemberCommand("Jane Doe", 21, "jd@office.com").SetCorrelationId("3eefdef050c340bfba100bd49c58c181");
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), nameof(CreateMemberCommand), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utc;
            }).Sign(JsonMarshaller.Default, o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });

            var json = JsonMarshaller.Default.Serialize(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            sut2.CheckSignature(JsonMarshaller.Default, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            var signatureException = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                sut2.CheckSignature(JsonMarshaller.Default, o => o.SignatureSecret = new byte[] { 3, 2, 1 });
            });
            Assert.StartsWith("The signature of the message does not match the cryptographically calculated value. Either you are using an incorrect secret and/or algorithm or the message has been tampered with.", signatureException.Message);

            var sut4 = JsonMarshaller.Default.Deserialize<ISignedMessage<CreateMemberCommand>>(json);

            Assert.Equivalent(sut2, sut4, true);


            Assert.Equal("""{"id":"2d4030d32a254ee8a27046e5bafe696a","source":"https://fancy.api/members","type":"CreateMemberCommand","time":"2023-11-16T23:24:17.8414532Z","data":{"name":"Jane Doe","age":21,"emailAddress":"jd@office.com","metadata":{"memberType":"Savvyio.Assets.Commands.CreateMemberCommand, Savvyio.Assets.Tests","correlationId":"3eefdef050c340bfba100bd49c58c181"}},"signature":"76df78c14ee668b2812a4c8c8ab58738de6b595623938e9c8aa893e9630ff5ad"}""", jsonString);
        }
    }
}
