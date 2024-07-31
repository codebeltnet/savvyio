using System;
using Cuemon.Extensions;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.EventDriven;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.EventDriven.Messaging.CloudEvents
{
    public class CloudEventTest : Test
    {
        public CloudEventTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CloudEventsMessage_Initialization_Success()
        {
            // Arrange
            var source = "https://savvyio.net/members".ToUri();
            var data = new AccountCreated(123347285, "John Doe", "jd@savvyio.net");
            var type = "net.savvyio.members.account-created";
            var time = DateTime.UtcNow;
            var message = data.ToMessage(source, type);

            // Act
            var cloudEventsMessage = message.ToCloudEvent();

            // Assert
            Assert.Equal(message.Id, cloudEventsMessage.Id);
            Assert.Equal(source.OriginalString, cloudEventsMessage.Source);
            Assert.Equal(type, cloudEventsMessage.Type);
            Assert.InRange(cloudEventsMessage.Time!.Value, time, time.AddSeconds(1));
            Assert.Equal(data, cloudEventsMessage.Data);
            Assert.Equal("1.0", cloudEventsMessage.SpecVersion);
        }
    }
}
