using System;
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Messaging.Cryptography
{
    public class SignedMessageOptionsTest : Test
    {
        public SignedMessageOptionsTest(ITestOutputHelper helper) : base(helper)
        {
        }

        [Fact]
        public void ValidateOptions_WhenSignatureSecretIsNull_ShouldThrowInvalidOperationException()
        {
            var options = new SignedMessageOptions();
            options.SignatureSecret = null;

            var ex = Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            TestOutput.WriteLine(ex.Message);

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'SignatureSecret == null || SignatureSecret.Length == 0')", ex.Message);
        }

        [Fact]
        public void ValidateOptions_WhenSignatureSecretIsEmpty_ShouldThrowInvalidOperationException()
        {
            var options = new SignedMessageOptions();
            options.SignatureSecret = new byte[0];

            var ex = Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            TestOutput.WriteLine(ex.Message);

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'SignatureSecret == null || SignatureSecret.Length == 0')", ex.Message);
        }

        [Fact]
        public void ValidateOptions_WhenAllPropertiesAreValid_ShouldNotThrowException()
        {
            var options = new SignedMessageOptions();
            options.SignatureSecret = new byte[] { 1, 2, 3 };
            options.ValidateOptions();
        }
    }
}
