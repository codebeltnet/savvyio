using System;
using Cuemon.Extensions;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Messaging.Cryptography
{
    public class SignedMessageTest : Test
    {
        public SignedMessageTest(ITestOutputHelper helper) : base(helper)
        {
        }

        [Fact]
        public void Constructor_WhenMessageIsNull_ShouldThrowArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new SignedMessage<CreateAccount>(null, "signature"));

            TestOutput.WriteLine(ex.Message);

            Assert.Equal("Value cannot be null. (Parameter 'message')", ex.Message);
        }

        [Fact]
        public void Constructor_WhenSignatureIsNull_ShouldThrowArgumentNullException()
        {
            var uuid = Guid.NewGuid();
            var fullName = "John Doe";
            var email = "jdoe@me.com";
            var message = new Message<CreateAccount>("MyId", "uri:source".ToUri(), new CreateAccount(uuid, fullName, email));
            var ex = Assert.Throws<ArgumentNullException>(() => new SignedMessage<CreateAccount>(message, null));

            TestOutput.WriteLine(ex.Message);

            Assert.Equal("Value cannot be null. (Parameter 'signature')", ex.Message);
        }

        [Fact]
        public void Constructor_WhenSignatureIsEmpty_ShouldThrowArgumentException()
        {
            var uuid = Guid.NewGuid();
            var fullName = "John Doe";
            var email = "jdoe@me.com";
            var message = new Message<CreateAccount>("MyId", "uri:source".ToUri(), new CreateAccount(uuid, fullName, email));
            var ex = Assert.Throws<ArgumentException>(() => new SignedMessage<CreateAccount>(message, string.Empty));
            
            TestOutput.WriteLine(ex.Message);

            Assert.Equal("Value cannot be empty. (Parameter 'signature')", ex.Message);
        }

        [Fact]
        public void Constructor_WhenSignatureIsWhitespace_ShouldThrowArgumentException()
        {
            var uuid = Guid.NewGuid();
            var fullName = "John Doe";
            var email = "jdoe@me.com";
            var message = new Message<CreateAccount>("MyId", "uri:source".ToUri(), new CreateAccount(uuid, fullName, email));
            var ex = Assert.Throws<ArgumentException>(() => new SignedMessage<CreateAccount>(message, " "));

            TestOutput.WriteLine(ex.Message);

            Assert.Equal("Value cannot consist only of white-space characters. (Parameter 'signature')", ex.Message);
        }

        [Fact]
        public void Constructor_WhenAllPropertiesAreValid_ShouldCreateInstance()
        {
            var uuid = Guid.NewGuid();
            var fullName = "John Doe";
            var email = "jdoe@me.com";
            var message = new Message<CreateAccount>("MyId", "uri:source".ToUri(), new CreateAccount(uuid, fullName, email));
            var signature = "signature";
            var signedMessage = new SignedMessage<CreateAccount>(message, signature);

            Assert.NotNull(signedMessage);
            Assert.Equal(message.Id, signedMessage.Id);
            Assert.Equal(message.Source, signedMessage.Source);
            Assert.Equal(message.Type, signedMessage.Type);
            Assert.Equal(message.Time, signedMessage.Time);
            Assert.Equal(message.Data, signedMessage.Data);
            Assert.Equal(signature, signedMessage.Signature);
        }
    }
}
