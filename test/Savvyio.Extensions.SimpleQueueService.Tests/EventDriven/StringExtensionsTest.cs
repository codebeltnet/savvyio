using System;
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.SimpleQueueService.EventDriven
{
    public class StringExtensionsTest : Test
    {
        public StringExtensionsTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void ToSnsUri_ShouldBuildArnWithDefaults()
        {
            var uri = "sample-topic".ToSnsUri();

            Assert.Equal(new Uri("arn:aws:sns:eu-west-1:000000000000:sample-topic"), uri);
        }

        [Fact]
        public void ToSnsUri_ShouldRespectCustomOptions()
        {
            var uri = "topic".ToSnsUri(o =>
            {
                o.Partition = "aws";
                o.Region = "us-east-1";
                o.AccountId = "123456789012";
            });

            Assert.Equal(new Uri("arn:aws:sns:us-east-1:123456789012:topic"), uri);
        }
    }
}
