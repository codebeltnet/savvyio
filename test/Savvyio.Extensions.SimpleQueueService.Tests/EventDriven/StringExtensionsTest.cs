using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.SimpleQueueService.EventDriven
{
    public class StringExtensionsTest : Test
    {
        public StringExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToSnsUri_Should_Use_Default_Amazon_Resource_Name_Options()
        {
            var result = "order-created".ToSnsUri();

            Assert.Equal($"arn:{AmazonResourceNameOptions.DefaultPartition}:sns:{AmazonResourceNameOptions.DefaultRegion}:{AmazonResourceNameOptions.DefaultAccountId}:order-created", result.OriginalString);
        }

        [Fact]
        public void ToSnsUri_Should_Apply_Custom_Amazon_Resource_Name_Options()
        {
            var result = "order-created".ToSnsUri(o =>
            {
                o.Partition = "aws-cn";
                o.Region = "cn-north-1";
                o.AccountId = "123456789012";
            });

            Assert.Equal("arn:aws-cn:sns:cn-north-1:123456789012:order-created", result.OriginalString);
        }

        [Fact]
        public void ToSnsUri_Should_Throw_When_Configurator_Produces_Invalid_Options()
        {
            Assert.Throws<ArgumentException>(() => "order-created".ToSnsUri(o => o.AccountId = "bad"));
        }
    }
}
