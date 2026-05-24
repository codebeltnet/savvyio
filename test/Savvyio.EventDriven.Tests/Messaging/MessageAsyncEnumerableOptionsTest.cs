using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Xunit;

namespace Savvyio.Messaging
{
    public class MessageAsyncEnumerableOptionsTest : Test
    {
        public MessageAsyncEnumerableOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldInitializeAcknowledgedProperties()
        {
            var options = new MessageAsyncEnumerableOptions<CreateMemberCommand>();

            Assert.IsType<ConcurrentBag<IDictionary<string, object>>>(options.AcknowledgedProperties);
        }

        [Fact]
        public void ValidateOptions_ShouldThrow_WhenAcknowledgedPropertiesIsNull()
        {
            var options = new MessageAsyncEnumerableOptions<CreateMemberCommand>
            {
                AcknowledgedProperties = null,
                MessageCallback = _ => Task.CompletedTask
            };

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_ShouldThrow_WhenMessageCallbackIsNull()
        {
            var options = new MessageAsyncEnumerableOptions<CreateMemberCommand>
            {
                AcknowledgedProperties = new ConcurrentBag<IDictionary<string, object>>()
            };

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_ShouldSucceed_WhenRequiredPropertiesAreSet()
        {
            var options = new MessageAsyncEnumerableOptions<CreateMemberCommand>
            {
                AcknowledgedProperties = new ConcurrentBag<IDictionary<string, object>>(),
                MessageCallback = _ => Task.CompletedTask
            };

            options.ValidateOptions();
        }
    }
}
