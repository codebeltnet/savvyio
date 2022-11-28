using System;
using Amazon;
using Amazon.Runtime;
using Cuemon;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.SimpleQueueService
{
    public class AmazonMessageOptionsTest : Test
    {
        public AmazonMessageOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldHaveDefaultValues()
        {
            var sut = new AmazonMessageOptions();

            Assert.Null(sut.Endpoint);
            Assert.Null(sut.SourceQueue);
            Assert.Null(sut.Credentials);
        }

        [Fact]
        public void ValidateOptions_ThrowsInvalidOperationException_WhenEndpointIsNull()
        {
            var sut1 = new AmazonMessageOptions
            {
                SourceQueue = new Uri("urn:null"),
                Credentials = new AnonymousAWSCredentials()
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            Assert.Equal($"Operation is not valid due to the current state of the object. (Expression '{nameof(AmazonMessageOptions.Endpoint)} == null')", sut2.Message);
            Assert.Equal($"{nameof(AmazonMessageOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void ValidateOptions_ThrowsInvalidOperationException_WhenSourceQueueIsNull()
        {
            var sut1 = new AmazonMessageOptions
            {
                Endpoint = RegionEndpoint.EUWest1,
                Credentials = new AnonymousAWSCredentials()
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            Assert.Equal($"Operation is not valid due to the current state of the object. (Expression '{nameof(AmazonMessageOptions.SourceQueue)} == null')", sut2.Message);
            Assert.Equal($"{nameof(AmazonMessageOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void ValidateOptions_ThrowsInvalidOperationException_WhenCredentialsIsNull()
        {
            var sut1 = new AmazonMessageOptions
            {
                Endpoint = RegionEndpoint.EUWest1,
                SourceQueue = new Uri("urn:null"),
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            Assert.Equal($"Operation is not valid due to the current state of the object. (Expression '{nameof(AmazonMessageOptions.Credentials)} == null')", sut2.Message);
            Assert.Equal($"{nameof(AmazonMessageOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }
    }
}
