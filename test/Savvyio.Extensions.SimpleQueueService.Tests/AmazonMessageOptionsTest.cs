using System;
using System.Linq;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
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
            Assert.Empty(sut.ClientConfigurations);
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

        [Fact]
        public void ValidateOptions_ShouldPass()
        {
            var sut1 = new AmazonMessageOptions
            {
                Credentials = new AnonymousAWSCredentials(),
                Endpoint = RegionEndpoint.EUWest1,
                SourceQueue = new Uri("urn:null")
            };

            sut1.ValidateOptions();

            Assert.Equivalent(new AnonymousAWSCredentials(), sut1.Credentials, true);
            Assert.Equal(RegionEndpoint.EUWest1, sut1.Endpoint);
            Assert.Equal(new Uri("urn:null"), sut1.SourceQueue);
            Assert.Equal(0, sut1.ClientConfigurations.Length);
        }

        [Fact]
        public void ValidateOptions_ShouldPassWithConfiguredClient()
        {
            var sut1 = new AmazonMessageOptions
            {
                Credentials = new AnonymousAWSCredentials(),
                Endpoint = RegionEndpoint.EUWest1,
                SourceQueue = new Uri("urn:null")
            }.ConfigureClient(client =>
            {
                client.ServiceURL = "http://localhost:4566";
                client.AuthenticationRegion = RegionEndpoint.EUWest1.SystemName;
            });

            sut1.ValidateOptions();

            Assert.Equivalent(new AnonymousAWSCredentials(), sut1.Credentials, true);
            Assert.Equal(RegionEndpoint.EUWest1, sut1.Endpoint);
            Assert.Equal(new Uri("urn:null"), sut1.SourceQueue);
            Assert.True(sut1.ClientConfigurations.All(config => config is AmazonSQSConfig || config is AmazonSimpleNotificationServiceConfig));
            Assert.Equal(2, sut1.ClientConfigurations.Length);
            Assert.Equal("http://localhost:4566/", sut1.ClientConfigurations.SimpleQueueService().ServiceURL);
            Assert.Equal("eu-west-1", sut1.ClientConfigurations.SimpleQueueService().AuthenticationRegion);
            Assert.Equal(sut1.ClientConfigurations.SimpleQueueService().ServiceURL, sut1.ClientConfigurations.SimpleNotificationService().ServiceURL);
            Assert.Equal(sut1.ClientConfigurations.SimpleQueueService().AuthenticationRegion, sut1.ClientConfigurations.SimpleNotificationService().AuthenticationRegion);
        }
    }
}
