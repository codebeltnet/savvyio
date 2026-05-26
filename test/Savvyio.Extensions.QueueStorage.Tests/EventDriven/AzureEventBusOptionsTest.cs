using System;
using Azure;
using Azure.Identity;
using Codebelt.Extensions.Xunit;
using Cuemon;
using Xunit;

namespace Savvyio.Extensions.QueueStorage.EventDriven
{
    public class AzureEventBusOptionsTest : Test
    {
        public AzureEventBusOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_Should_Set_Defaults()
        {
            var sut = new AzureEventBusOptions();

            Assert.NotNull(sut.Credential);
            Assert.IsType<DefaultAzureCredential>(sut.Credential);
            Assert.Null(sut.TopicEndpoint);
            Assert.NotNull(sut.Settings);
            Assert.Null(sut.KeyCredential);
            Assert.Null(sut.SasCredential);
        }

        [Fact]
        public void Credential_Setters_Should_Be_Mutually_Exclusive()
        {
            var sut = new AzureEventBusOptions
            {
                KeyCredential = new AzureKeyCredential("key")
            };

            Assert.NotNull(sut.KeyCredential);
            Assert.Null(sut.Credential);
            Assert.Null(sut.SasCredential);

            sut.SasCredential = new AzureSasCredential("sig");
            Assert.NotNull(sut.SasCredential);
            Assert.Null(sut.Credential);
            Assert.Null(sut.KeyCredential);

            sut.Credential = new DefaultAzureCredential();
            Assert.NotNull(sut.Credential);
            Assert.Null(sut.KeyCredential);
            Assert.Null(sut.SasCredential);
        }

        [Fact]
        public void ValidateOptions_Should_Throw_When_Topic_Endpoint_Is_Missing()
        {
            var sut1 = new AzureEventBusOptions();
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal($"A {nameof(AzureEventBusOptions.TopicEndpoint)} is required. (Expression '{nameof(AzureEventBusOptions.TopicEndpoint)} == null')", sut2.Message);
            Assert.Equal($"{nameof(AzureEventBusOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void ValidateOptions_Should_Throw_When_All_Credentials_Are_Missing()
        {
            var sut1 = new AzureEventBusOptions
            {
                TopicEndpoint = new Uri("https://test.topic")
            };
            sut1.Credential = null;
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal("A credential type has to be specified for Azure authentication. (Expression 'Credential == null && SasCredential == null && KeyCredential == null')", sut2.Message);
            Assert.Equal($"{nameof(AzureEventBusOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void ValidateOptions_Should_Not_Throw_When_Valid()
        {
            var sut = new AzureEventBusOptions
            {
                TopicEndpoint = new Uri("https://test.topic")
            };

            var exception = Record.Exception(() => sut.ValidateOptions());

            Assert.Null(exception);
        }
    }
}
