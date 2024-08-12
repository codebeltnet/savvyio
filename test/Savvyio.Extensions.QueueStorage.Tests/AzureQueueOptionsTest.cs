using System;
using Azure.Identity;
using Cuemon;
using Cuemon.Extensions.Xunit;
using Savvyio.Extensions.QueueStorage.Commands;
using Savvyio.Extensions.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.QueueStorage
{
    public class AzureQueueOptionsTest : Test
    {
        public AzureQueueOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            var options = new AzureQueueOptions();

            Assert.NotNull(options.Credential);
            Assert.IsType<DefaultAzureCredential>(options.Credential);
            Assert.NotNull(options.Settings);
            Assert.NotNull(options.ReceiveContext);
            Assert.NotNull(options.SendContext);
            Assert.Null(options.StorageAccountName);
            Assert.Null(options.QueueName);
            Assert.Null(options.SasCredential);
            Assert.Null(options.StorageKeyCredential);
            Assert.Null(options.ConnectionString);
        }

        [Fact]
        public void ValidateOptions_ShouldThrowException_WhenOptionsAreInvalid()
        {
            var options = new AzureQueueOptions();

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            options.ConnectionString = "DefaultEndpointsProtocol=https;AccountName=testaccount;AccountKey=testkey;EndpointSuffix=core.windows.net";
            options.QueueName = "testqueue";
            options.ValidateOptions(); // Should not throw

            options.ConnectionString = null;
            options.Credential = null;
            options.SasCredential = null;
            options.StorageKeyCredential = null;

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_ThrowsInvalidOperationException_WhenConnectionStringIsSetAndQueueNameIsNull()
        {
            var sut1 = new AzureQueueOptions
            {
                ConnectionString = "DefaultEndpointsProtocol=https;AccountName=testaccount;AccountKey=testkey;EndpointSuffix=core.windows.net",
                QueueName = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            TestOutput.WriteLine(sut2.Message);

            Assert.Equal($"{nameof(AzureQueueOptions.QueueName)} cannot be null, empty, or consists only of white-space when a {nameof(AzureQueueOptions.ConnectionString)} has been specified. (Expression '!string.IsNullOrWhiteSpace({nameof(AzureQueueOptions.ConnectionString)}) && string.IsNullOrWhiteSpace({nameof(AzureQueueOptions.QueueName)})')", sut2.Message);
            Assert.Equal($"{nameof(AzureQueueOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void ValidateOptions_ThrowsInvalidOperationException_WhenAllCredentialPropertiesAreNull()
        {
            var sut1 = new AzureQueueOptions
            {
                Credential = null,
                SasCredential = null,
                StorageKeyCredential = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            TestOutput.WriteLine(sut2.Message);

            Assert.Equal($"A credential type has to be specified for Azure authentication. (Expression '{nameof(AzureQueueOptions.Credential)} == null && {nameof(AzureQueueOptions.SasCredential)} == null && {nameof(AzureQueueOptions.StorageKeyCredential)} == null')", sut2.Message);
            Assert.Equal($"{nameof(AzureQueueOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void ValidateOptions_ThrowsInvalidOperationException_WhenStorageNameAndQueueNameIsNotSpecified()
        {
            var sut1 = new AzureQueueOptions();
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            TestOutput.WriteLine(sut2.Message);

            Assert.Equal($"{nameof(AzureQueueOptions.StorageAccountName)} and {nameof(AzureQueueOptions.QueueName)} cannot be null, empty, or consists only of white-space characters. (Expression 'string.IsNullOrWhiteSpace({nameof(AzureQueueOptions.StorageAccountName)}) && string.IsNullOrWhiteSpace({nameof(AzureQueueOptions.QueueName)})')", sut2.Message);
            Assert.Equal($"{nameof(AzureQueueOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void PostConfigureClient_ShouldInvokeCallback()
        {
            var callbackInvoked = false;
            var client = new AzureCommandQueue(JsonMarshaller.Default, new AzureQueueOptions()
            {
                ConnectionString = "UseDevelopmentStorage=true",
                QueueName = "testqueue"
            }.PostConfigureClient(c =>
            {
                callbackInvoked = true;
                Assert.Equal(c.Name, "testqueue");
            }));

            Assert.True(callbackInvoked);
        }
    }
}
