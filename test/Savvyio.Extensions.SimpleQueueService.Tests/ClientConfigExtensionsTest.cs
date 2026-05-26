using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.SimpleQueueService
{
    public class ClientConfigExtensionsTest : Test
    {
        public ClientConfigExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void IsValid_Should_Return_True_When_Both_Aws_Client_Configurations_Are_Present()
        {
            ClientConfig[] configurations = [ new AmazonSQSConfig(), new AmazonSimpleNotificationServiceConfig() ];

            Assert.True(configurations.IsValid());
            Assert.IsType<AmazonSQSConfig>(configurations.SimpleQueueService());
            Assert.IsType<AmazonSimpleNotificationServiceConfig>(configurations.SimpleNotificationService());
        }

        [Fact]
        public void IsValid_Should_Return_False_When_Configurations_Are_Missing_Expected_Types()
        {
            ClientConfig[] invalid = [ new AmazonSQSConfig() ];

            Assert.False(invalid.IsValid());
            Assert.NotNull(invalid.SimpleQueueService());
            Assert.Null(invalid.SimpleNotificationService());
        }
    }
}
