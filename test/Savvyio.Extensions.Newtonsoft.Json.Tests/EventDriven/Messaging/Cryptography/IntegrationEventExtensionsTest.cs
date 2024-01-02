﻿using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.EventDriven.Messaging.Cryptography;
using Savvyio.Messaging.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Newtonsoft.Json.EventDriven.Messaging.Cryptography
{
    public class IntegrationEventExtensionsTest : Test
    {
        public IntegrationEventExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToMessage_Sign_ShouldSerializeAndDeserialize_MemberCreated_UsingInterface()
        {
            var utc = DateTime.Parse("2023-11-16T23:24:17.8414532Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com").SetEventId("69bccf3b1117425397c5ed9ed757bb0f").SetTimestamp(utc);
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), nameof(MemberCreated), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utc;
            }).Sign(new NewtonsoftJsonSerializerContext(), o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });
            var json = JsonFormatter.SerializeObject(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var sut4 = new NewtonsoftJsonSerializerContext().Deserialize<ISignedMessage<MemberCreated>>(json);

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
                               "signature": "f1aa38c6af66575aec152472e74e1d70d49da3c4e27a82a20eb94320c9cd22a4"
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
                               "signature": "40f2e00e66dd02014ba535e0bbff9fa04954bcbcf2f3fce69f688bd64583ee50"
                             }
                             """.ReplaceLineEndings(), jsonString);
            }
        }

        [Fact]
        public void ToMessage_Sign_ShouldSerializeAndDeserialize_MemberCreated_UsingConcreteType()
        {
            var utc = DateTime.Parse("2023-11-16T23:24:17.8414532Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com").SetEventId("69bccf3b1117425397c5ed9ed757bb0f").SetTimestamp(utc);
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), nameof(MemberCreated), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utc;
            }).Sign(new NewtonsoftJsonSerializerContext(), o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });
            var json = JsonFormatter.SerializeObject(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var sut4 = new NewtonsoftJsonSerializerContext().Deserialize<SignedMessage<MemberCreated>>(json);

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
                               "signature": "f1aa38c6af66575aec152472e74e1d70d49da3c4e27a82a20eb94320c9cd22a4"
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
                               "signature": "40f2e00e66dd02014ba535e0bbff9fa04954bcbcf2f3fce69f688bd64583ee50"
                             }
                             """.ReplaceLineEndings(), jsonString);
            }
        }
    }
}
