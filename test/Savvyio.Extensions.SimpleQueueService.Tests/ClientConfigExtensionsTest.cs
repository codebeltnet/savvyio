using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.SimpleQueueService
{
    public class ClientConfigExtensionsTest : Test
    {
        public ClientConfigExtensionsTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void IsValid_ShouldEvaluateConfigurations()
        {
            ClientConfig[] invalid = null;
            Assert.False(invalid.IsValid());

            var valid = new ClientConfig[] { new AmazonSQSConfig(), new AmazonSimpleNotificationServiceConfig() };
            Assert.True(valid.IsValid());

            var wrongLength = new ClientConfig[] { new AmazonSQSConfig() };
            Assert.False(wrongLength.IsValid());
        }

        [Fact]
        public void ShouldResolveSpecificConfigurations()
        {
            var sqs = new AmazonSQSConfig();
            var sns = new AmazonSimpleNotificationServiceConfig();
            ClientConfig[] configs = { sqs, sns };

            Assert.Same(sqs, configs.SimpleQueueService());
            Assert.Same(sns, configs.SimpleNotificationService());
        }
    }
}
