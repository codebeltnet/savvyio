using System;
using System.Collections.Generic;
using System.Globalization;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.Text.Json.Converters
{
    public class MessageConverterTest : Test
    {
        public MessageConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void MessageConverter_ShouldConvertOnlyMessages()
        {
            var sut = new MessageConverter();

            Assert.True(sut.CanConvert(typeof(Message<MemberCreated>)));
            Assert.False(sut.CanConvert(typeof(MemberCreated)));
            Assert.False(sut.CanConvert(typeof(string)));
        }

        [Fact]
        public void MessageConverter_ShouldRoundtripCloudEventExtensionAttributes()
        {
            var utc = DateTime.Parse("2023-11-16T23:24:17.8414532Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var cloudEvent = new MemberCreated("Jane Doe", "jd@office.com").SetEventId("69bccf3b1117425397c5ed9ed757bb0f").SetTimestamp(utc)
                .ToMessage("https://fancy.api/members".ToUri(), nameof(MemberCreated), o =>
                {
                    o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                    o.Time = utc;
                }).ToCloudEvent();

            ((IDictionary<string, object>)cloudEvent).Add("traceparent", "00-abc-def-01");

            var marshaller = new JsonMarshaller();
            var json = marshaller.Serialize(cloudEvent);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var result = marshaller.Deserialize<ICloudEvent<MemberCreated>>(json);

            Assert.Contains("traceparent", jsonString);
            Assert.True(((IDictionary<string, object>)result).ContainsKey("traceparent"));
        }

        [Fact]
        public void MessageConverter_ShouldRoundtripDataWithWritableMetadata()
        {
            var utc = DateTime.Parse("2023-11-16T23:24:17.8414532Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var data = new WritableMetadataRequest { Name = "Jane Doe" };
            data.Metadata.Add("custom", "value");
            var message = new Message<WritableMetadataRequest>("2d4030d32a254ee8a27046e5bafe696a", "https://fancy.api/members".ToUri(), nameof(WritableMetadataRequest), data, utc);

            var marshaller = new JsonMarshaller();
            var json = marshaller.Serialize(message);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var result = marshaller.Deserialize<Message<WritableMetadataRequest>>(json);

            Assert.NotNull(result);
            Assert.Equal("Jane Doe", result.Data.Name);
            Assert.NotNull(result.Data.Metadata);
            Assert.True(result.Data.Metadata.ContainsKey("custom"));
            Assert.Equal("value", result.Data.Metadata["custom"].ToString());
        }

        private sealed record WritableMetadataRequest : IRequest, IMetadata
        {
            public string Name { get; set; }

            public IMetadataDictionary Metadata { get; set; } = new MetadataDictionary();
        }
    }
}
