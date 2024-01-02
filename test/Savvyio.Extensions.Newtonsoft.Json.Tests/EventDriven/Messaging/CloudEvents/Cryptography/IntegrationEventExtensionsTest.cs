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
              .Sign(new NewtonsoftJsonMarshaller(), o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });

            var json = new NewtonsoftJsonMarshaller().Serialize(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var sut4 = new NewtonsoftJsonMarshaller().Deserialize<ISignedCloudEvent<MemberCreated>>(json);

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
                               "signature": "90c32cb865e0e4a84619bfd8b484a4539189e649654f348fc41e2ba1a121e915"
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
                               "signature": "ebadf74de69dc8f5c6812ee28c005531d5c5714277cd92e14d52f11e74589817"
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
              .Sign(new NewtonsoftJsonMarshaller(), o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });

            var json = new NewtonsoftJsonMarshaller().Serialize(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var sut4 = new NewtonsoftJsonMarshaller().Deserialize<SignedCloudEvent<MemberCreated>>(json);

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
                               "signature": "90c32cb865e0e4a84619bfd8b484a4539189e649654f348fc41e2ba1a121e915"
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
                               "signature": "ebadf74de69dc8f5c6812ee28c005531d5c5714277cd92e14d52f11e74589817"
                             }
                             """.ReplaceLineEndings(), jsonString);
            }
        }
    }
}
