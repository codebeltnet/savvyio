using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Queues;
using Cuemon;
using Cuemon.Extensions.Xunit;
using Savvyio.Extensions.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.QueueStorage.Commands
{
    public class AzureCommandQueueOptionsTest : Test
    {
        public AzureCommandQueueOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            var options = new AzureCommandQueueOptions();

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
            var options = new AzureCommandQueueOptions();

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
            var sut1 = new AzureCommandQueueOptions
            {
                ConnectionString = "DefaultEndpointsProtocol=https;AccountName=testaccount;AccountKey=testkey;EndpointSuffix=core.windows.net",
                QueueName = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            TestOutput.WriteLine(sut2.Message);

            Assert.Equal($"{nameof(AzureCommandQueueOptions.QueueName)} cannot be null, empty, or consists only of white-space when a {nameof(AzureCommandQueueOptions.ConnectionString)} has been specified. (Expression '!string.IsNullOrWhiteSpace({nameof(AzureCommandQueueOptions.ConnectionString)}) && string.IsNullOrWhiteSpace({nameof(AzureCommandQueueOptions.QueueName)})')", sut2.Message);
            Assert.Equal($"{nameof(AzureCommandQueueOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void ValidateOptions_ThrowsInvalidOperationException_WhenAllCredentialPropertiesAreNull()
        {
            var sut1 = new AzureCommandQueueOptions
            {
                Credential = null,
                SasCredential = null,
                StorageKeyCredential = null
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            TestOutput.WriteLine(sut2.Message);

            Assert.Equal($"At least one credential type has to be specified for Azure authentication. (Expression '{nameof(AzureCommandQueueOptions.Credential)} == null && {nameof(AzureCommandQueueOptions.SasCredential)} == null && {nameof(AzureCommandQueueOptions.StorageKeyCredential)} == null')", sut2.Message);
            Assert.Equal($"{nameof(AzureCommandQueueOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void ValidateOptions_ThrowsInvalidOperationException_WhenStorageNameAndQueueNameIsNotSpecified()
        {
            var sut1 = new AzureCommandQueueOptions();
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1, nameof(sut1)));

            TestOutput.WriteLine(sut2.Message);

            Assert.Equal($"{nameof(AzureCommandQueueOptions.StorageAccountName)} and {nameof(AzureCommandQueueOptions.QueueName)} cannot be null, empty, or consists only of white-space characters. (Expression 'string.IsNullOrWhiteSpace({nameof(AzureCommandQueueOptions.StorageAccountName)}) && string.IsNullOrWhiteSpace({nameof(AzureCommandQueueOptions.QueueName)})')", sut2.Message);
            Assert.Equal($"{nameof(AzureCommandQueueOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void PostConfigureClient_ShouldInvokeCallback()
        {
            var callbackInvoked = false;
            var client = new AzureCommandQueue(JsonMarshaller.Default, new AzureCommandQueueOptions()
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
