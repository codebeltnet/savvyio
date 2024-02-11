using System;
using System.Globalization;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Newtonsoft.Json.EventDriven.Messaging
{
    public class IntegrationEventExtensionsTest : Test
    {
        public IntegrationEventExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact]
        public void ToMessage_ShouldSerializeAndDeserialize_MemberCreated_UsingInterface()
        {
            var utc = DateTime.Parse("2023-11-16T23:24:17.8414532Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com").SetEventId("69bccf3b1117425397c5ed9ed757bb0f").SetTimestamp(utc);
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), nameof(MemberCreated), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utc;
            });

            var json = new NewtonsoftJsonMarshaller().Serialize(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var sut4 = new NewtonsoftJsonMarshaller().Deserialize<IMessage<MemberCreated>>(json);

            Assert.Equivalent(sut2, sut4, true);

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
                           }
                         }
                         """.ReplaceLineEndings(), jsonString);
        }

        [Fact]
        public void ToMessage_ShouldSerializeAndDeserialize_MemberCreated_UsingConcreteType()
        {
            var utcNow = DateTime.UtcNow;
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com").SetEventId("69bccf3b1117425397c5ed9ed757bb0f").SetTimestamp(utcNow);
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), nameof(MemberCreated), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utcNow;
            });

            var json = new NewtonsoftJsonMarshaller(o => o.Settings.DateFormatString = "O").Serialize(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);
            
            TestOutput.WriteLine(jsonString);

            var sut4 = new NewtonsoftJsonMarshaller(o => o.Settings.DateFormatString = "O").Deserialize<Message<MemberCreated>>(json);

            Assert.Equivalent(sut2, sut4, true);

            Assert.Equal($$"""
                           {
                             "id": "2d4030d32a254ee8a27046e5bafe696a",
                             "source": "https://fancy.api/members",
                             "type": "MemberCreated",
                             "time": "{{utcNow:O}}",
                             "data": {
                               "name": "Jane Doe",
                               "emailAddress": "jd@office.com",
                               "metadata": {
                                 "memberType": "Savvyio.Assets.EventDriven.MemberCreated, Savvyio.Assets.Tests",
                                 "eventId": "69bccf3b1117425397c5ed9ed757bb0f",
                                 "timestamp": "{{utcNow:O}}"
                               }
                             }
                           }
                           """.ReplaceLineEndings(), jsonString);
        }
    }
}
