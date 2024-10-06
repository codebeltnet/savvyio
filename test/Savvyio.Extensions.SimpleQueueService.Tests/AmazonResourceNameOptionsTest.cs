using System;
using Cuemon;
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.SimpleQueueService
{
    public class AmazonResourceNameOptionsTest : Test
    {
        public AmazonResourceNameOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldHaveDefaultValues()
        {
            var sut = new AmazonResourceNameOptions();

            Assert.Equal(AmazonResourceNameOptions.DefaultAccountId, sut.AccountId);
            Assert.Equal(AmazonResourceNameOptions.DefaultPartition, sut.Partition);
            Assert.Equal(AmazonResourceNameOptions.DefaultRegion, sut.Region);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("0123456789")]
        [InlineData("A0123456789Z")]
        [InlineData("1012345678910")]
        public void ValidateOptions_ThrowsInvalidOperationException_WhenAccountIdIsNullOrEmptyOrConsistOfWhiteSpaceOnlyOrNotFixedLengthOrNotANumber(string accountId)
        {
            var sut1 = new AmazonResourceNameOptions
            {
                AccountId = accountId
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal($"Operation is not valid due to the current state of the object. (Expression '{nameof(AmazonResourceNameOptions.AccountId)}.IsNullOrWhiteSpace() || {nameof(AmazonResourceNameOptions.AccountId)}.Length != 12 || !{nameof(AmazonResourceNameOptions.AccountId)}.IsNumeric(NumberStyles.Integer)')", sut2.Message);
            Assert.Equal($"{nameof(AmazonResourceNameOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ValidateOptions_ThrowsInvalidOperationException_WhenPartitionIsNullOrEmptyOrConsistOfWhiteSpaceOnly(string partition)
        {
            var sut1 = new AmazonResourceNameOptions
            {
                Partition = partition
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal($"Operation is not valid due to the current state of the object. (Expression '{nameof(AmazonResourceNameOptions.Partition)}.IsNullOrWhiteSpace()')", sut2.Message);
            Assert.Equal($"{nameof(AmazonResourceNameOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ValidateOptions_ThrowsInvalidOperationException_WhenRegionIsNullOrEmptyOrConsistOfWhiteSpaceOnly(string region)
        {
            var sut1 = new AmazonResourceNameOptions
            {
                Region = region
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal($"Operation is not valid due to the current state of the object. (Expression '{nameof(AmazonResourceNameOptions.Region)}.IsNullOrWhiteSpace()')", sut2.Message);
            Assert.Equal($"{nameof(AmazonResourceNameOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }
    }
}
