using System;
using System.Globalization;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.EventDriven.Messaging.CloudEvents.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Newtonsoft.Json.EventDriven.Messaging.CloudEvents.Cryptography
{
    public class IntegrationEventExtensionsTest : Test
    {
        public IntegrationEventExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToMessage_ToCloudEvent_Sign_ShouldSerializeAndDeserialize_MemberCreated_UsingInterface()
        {
            var utc = DateTime.Parse("2023-11-16T23:24:17.8414532Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com").SetEventId("69bccf3b1117425397c5ed9ed757bb0f").SetTimestamp(utc);
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), nameof(MemberCreated), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utc;
            }).ToCloudEvent()
              .Sign(NewtonsoftJsonMarshaller.Default, o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });

            var json = NewtonsoftJsonMarshaller.Default.Serialize(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            sut2.CheckSignature(NewtonsoftJsonMarshaller.Default, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            var signatureException = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                sut2.CheckSignature(NewtonsoftJsonMarshaller.Default, o => o.SignatureSecret = new byte[] { 3, 2, 1 });
            });
            Assert.StartsWith("The signature of the cloud event does not match the cryptographically calculated value. Either you are using an incorrect secret and/or algorithm or the message has been tampered with.", signatureException.Message);

            var sut4 = NewtonsoftJsonMarshaller.Default.Deserialize<ISignedCloudEvent<MemberCreated>>(json);

            Assert.Equivalent(sut2, sut4, true);
            Assert.Equal("""{"id":"2d4030d32a254ee8a27046e5bafe696a","source":"https://fancy.api/members","type":"MemberCreated","time":"2023-11-16T23:24:17.8414532Z","data":{"name":"Jane Doe","emailAddress":"jd@office.com","metadata":{"memberType":"Savvyio.Assets.EventDriven.MemberCreated, Savvyio.Assets.Tests","eventId":"69bccf3b1117425397c5ed9ed757bb0f","timestamp":"2023-11-16T23:24:17.8414532Z"}},"specVersion":"1.0","signature":"15526d247706632c2b6e753f6aae55ad2a694c2bbcc63f701f4ef7f1a63a65b3"}""", jsonString);
        }

        [Fact]
        public void ToMessage_ToCloudEvent_Sign_ShouldSerializeAndDeserialize_MemberCreated_UsingConcreteType()
        {
            var utc = DateTime.Parse("2023-11-16T23:24:17.8414532Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com").SetEventId("69bccf3b1117425397c5ed9ed757bb0f").SetTimestamp(utc);
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), nameof(MemberCreated), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utc;
            }).ToCloudEvent()
              .Sign(NewtonsoftJsonMarshaller.Default, o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });

            var json = NewtonsoftJsonMarshaller.Default.Serialize(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            sut2.CheckSignature(NewtonsoftJsonMarshaller.Default, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            var signatureException = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                sut2.CheckSignature(NewtonsoftJsonMarshaller.Default, o => o.SignatureSecret = new byte[] { 3, 2, 1 });
            });
            Assert.StartsWith("The signature of the cloud event does not match the cryptographically calculated value. Either you are using an incorrect secret and/or algorithm or the message has been tampered with.", signatureException.Message);

            var sut4 = NewtonsoftJsonMarshaller.Default.Deserialize<SignedCloudEvent<MemberCreated>>(json);

            Assert.Equivalent(sut2, sut4, true);
            Assert.Equal("""{"id":"2d4030d32a254ee8a27046e5bafe696a","source":"https://fancy.api/members","type":"MemberCreated","time":"2023-11-16T23:24:17.8414532Z","data":{"name":"Jane Doe","emailAddress":"jd@office.com","metadata":{"memberType":"Savvyio.Assets.EventDriven.MemberCreated, Savvyio.Assets.Tests","eventId":"69bccf3b1117425397c5ed9ed757bb0f","timestamp":"2023-11-16T23:24:17.8414532Z"}},"specVersion":"1.0","signature":"15526d247706632c2b6e753f6aae55ad2a694c2bbcc63f701f4ef7f1a63a65b3"}""", jsonString);
        }
    }
}
