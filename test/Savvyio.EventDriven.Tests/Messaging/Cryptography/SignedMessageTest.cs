using System;
using Cuemon.Extensions;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Savvyio.Commands.Messaging;
using Xunit;

namespace Savvyio.Messaging.Cryptography
{
    public class SignedMessageTest : Test
    {
        public SignedMessageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenMessageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SignedMessage<CreateMemberCommand>(null, "sig"));
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenSignatureIsNull()
        {
            var message = new CreateMemberCommand("Jane", 21, "j@j.com").ToMessage("https://api.example.com".ToUri(), "test");

            Assert.Throws<ArgumentNullException>(() => new SignedMessage<CreateMemberCommand>(message, null));
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentException_WhenSignatureIsEmpty()
        {
            var message = new CreateMemberCommand("Jane", 21, "j@j.com").ToMessage("https://api.example.com".ToUri(), "test");

            Assert.Throws<ArgumentException>(() => new SignedMessage<CreateMemberCommand>(message, string.Empty));
        }

        [Fact]
        public void Ctor_ShouldCopyMessageProperties()
        {
            var utc = DateTime.UtcNow;
            var message = new CreateMemberCommand("Jane", 21, "j@j.com").ToMessage("https://api.example.com".ToUri(), "test", o =>
            {
                o.MessageId = "abc123";
                o.Time = utc;
            });

            var signed = new SignedMessage<CreateMemberCommand>(message, "mysig");

            Assert.Equal(message.Id, signed.Id);
            Assert.Equal(message.Source, signed.Source);
            Assert.Equal(message.Type, signed.Type);
            Assert.Equal(message.Time, signed.Time);
            Assert.Equal(message.Data, signed.Data);
            Assert.Equal("mysig", signed.Signature);
        }
    }
}
