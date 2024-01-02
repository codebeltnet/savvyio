using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.EventDriven.Messaging.CloudEvents.Cryptography;
using Savvyio.EventDriven.Messaging.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Text.Json.EventDriven.Messaging.CloudEvents.Cryptography
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
              .Sign(new JsonSerializerContext(), o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });

            var json = new JsonSerializerContext().Serialize(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var sut4 = new JsonSerializerContext().Deserialize<ISignedCloudEvent<MemberCreated>>(json);

            Assert.Equivalent(sut2, sut4, true);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Assert.Equal("""
                             {
                               "id": "2d4030d32a254ee8a27046e5bafe696a",
                               "source": "https://fancy.api/members",
                               "type": "MemberCreated",
                               "time": "2023-11-16T23:24:17.8414532Z",
                               "data": {
                                 "name": "Jane Doe",
                                 "emailAddress": "jd@office.com",
                                 "metadata": {
                                   "memberType": "Savvyio.Assets.EventDriven.MemberCreated, Savvyio.Assets.Tests",
                                   "eventId": "69bccf3b1117425397c5ed9ed757bb0f",
                                   "timestamp": "2023-11-16T23:24:17.8414532Z"
                                 }
                               },
                               "specVersion": "1.0",
                               "signature": "d353adcaff291ef9f902c40614e4ad901981461f86c69b371467edc8878255de"
                             }
                             """.ReplaceLineEndings(), jsonString);
            }
            else
            {
                Assert.Equal("""
                             {
                               "id": "2d4030d32a254ee8a27046e5bafe696a",
                               "source": "https://fancy.api/members",
                               "type": "MemberCreated",
                               "time": "2023-11-16T23:24:17.8414532Z",
                               "data": {
                                 "name": "Jane Doe",
                                 "emailAddress": "jd@office.com",
                                 "metadata": {
                                   "memberType": "Savvyio.Assets.EventDriven.MemberCreated, Savvyio.Assets.Tests",
                                   "eventId": "69bccf3b1117425397c5ed9ed757bb0f",
                                   "timestamp": "2023-11-16T23:24:17.8414532Z"
                                 }
                               },
                               "specVersion": "1.0",
                               "signature": "1f62f3e69a1062697972acf23d783e576be77bcf64748dbf6d04ac8c0d015617"
                             }
                             """.ReplaceLineEndings(), jsonString);
            }
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
              .Sign(new JsonSerializerContext(), o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });

            var json = new JsonSerializerContext().Serialize(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var sut4 = new JsonSerializerContext().Deserialize<SignedCloudEvent<MemberCreated>>(json);

            Assert.Equivalent(sut2, sut4, true);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Assert.Equal("""
                             {
                               "id": "2d4030d32a254ee8a27046e5bafe696a",
                               "source": "https://fancy.api/members",
                               "type": "MemberCreated",
                               "time": "2023-11-16T23:24:17.8414532Z",
                               "data": {
                                 "name": "Jane Doe",
                                 "emailAddress": "jd@office.com",
                                 "metadata": {
                                   "memberType": "Savvyio.Assets.EventDriven.MemberCreated, Savvyio.Assets.Tests",
                                   "eventId": "69bccf3b1117425397c5ed9ed757bb0f",
                                   "timestamp": "2023-11-16T23:24:17.8414532Z"
                                 }
                               },
                               "specVersion": "1.0",
                               "signature": "d353adcaff291ef9f902c40614e4ad901981461f86c69b371467edc8878255de"
                             }
                             """.ReplaceLineEndings(), jsonString);
            }
            else
            {
                Assert.Equal("""
                             {
                               "id": "2d4030d32a254ee8a27046e5bafe696a",
                               "source": "https://fancy.api/members",
                               "type": "MemberCreated",
                               "time": "2023-11-16T23:24:17.8414532Z",
                               "data": {
                                 "name": "Jane Doe",
                                 "emailAddress": "jd@office.com",
                                 "metadata": {
                                   "memberType": "Savvyio.Assets.EventDriven.MemberCreated, Savvyio.Assets.Tests",
                                   "eventId": "69bccf3b1117425397c5ed9ed757bb0f",
                                   "timestamp": "2023-11-16T23:24:17.8414532Z"
                                 }
                               },
                               "specVersion": "1.0",
                               "signature": "1f62f3e69a1062697972acf23d783e576be77bcf64748dbf6d04ac8c0d015617"
                             }
                             """.ReplaceLineEndings(), jsonString);
            }
        }
    }
}
