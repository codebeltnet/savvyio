using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Messaging.Cryptography
{
    public class SignedMessageOptionsTest : Test
    {
        public SignedMessageOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ValidateOptions_ShouldThrow_WhenSignatureSecretIsNull()
        {
            var options = new SignedMessageOptions();

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_ShouldThrow_WhenSignatureSecretIsEmpty()
        {
            var options = new SignedMessageOptions { SignatureSecret = Array.Empty<byte>() };

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_ShouldSucceed_WhenSignatureSecretIsSet()
        {
            var options = new SignedMessageOptions { SignatureSecret = new byte[] { 1, 2, 3 } };

            options.ValidateOptions();
        }
    }
}
