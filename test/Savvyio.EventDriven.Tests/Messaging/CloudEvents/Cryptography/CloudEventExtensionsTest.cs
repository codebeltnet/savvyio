using System;
using Cuemon.Extensions;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.EventDriven;
using Savvyio.Extensions.Text.Json;
using Xunit;

namespace Savvyio.EventDriven.Messaging.CloudEvents.Cryptography
{
    public class CloudEventExtensionsTest : Test
    {
        public CloudEventExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void SignCloudEvent_ShouldCreateSignedCloudEvent_WithVerifiableSignature()
        {
            var marshaller = new JsonMarshaller();
            var cloudEvent = CreateCloudEvent();

            var signed = cloudEvent.SignCloudEvent(marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            Assert.Equal(cloudEvent.Id, signed.Id);
            Assert.Equal(cloudEvent.Source, signed.Source);
            Assert.Equal(cloudEvent.Type, signed.Type);
            Assert.Equal(cloudEvent.Time, signed.Time);
            Assert.Equal(cloudEvent.Data, signed.Data);
            Assert.Equal(cloudEvent.Specversion, signed.Specversion);
            Assert.False(string.IsNullOrWhiteSpace(signed.Signature));

            signed.CheckCloudEventSignature(marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });
        }

        [Fact]
        public void CheckCloudEventSignature_ShouldThrow_WhenSecretDoesNotMatch()
        {
            var marshaller = new JsonMarshaller();
            var signed = CreateCloudEvent().SignCloudEvent(marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => signed.CheckCloudEventSignature(marshaller, o => o.SignatureSecret = new byte[] { 3, 2, 1 }));

            Assert.Equal(signed.Signature, exception.ActualValue);
        }

        private static CloudEvent<MemberCreated> CreateCloudEvent()
        {
            var utc = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var @event = new MemberCreated("Jane Doe", "jd@office.com")
                .SetEventId("event-123")
                .SetTimestamp(utc);
            var message = @event.ToMessage("https://api.example.com/members".ToUri(), nameof(MemberCreated), o =>
            {
                o.MessageId = "message-123";
                o.Time = utc;
            });

            return (CloudEvent<MemberCreated>)message.ToCloudEvent();
        }
    }
}
