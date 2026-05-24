using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Xunit;

namespace Savvyio.Messaging
{
    public class MessageAsyncEnumerableTest : Test
    {
        public MessageAsyncEnumerableTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenSourceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new MessageAsyncEnumerable<CreateMemberCommand>((IAsyncEnumerable<IMessage<CreateMemberCommand>>)null));
        }

        [Fact]
        public async Task GetAsyncEnumerator_ShouldInvokeCallbacks_AndCollectAcknowledgedProperties()
        {
            var callbacks = new List<string>();
            List<IDictionary<string, object>> acknowledged = null;
            var utc = DateTime.UtcNow;
            var source = new IMessage<CreateMemberCommand>[]
            {
                new Message<CreateMemberCommand>("mid1", "urn:members:1".ToUri(), nameof(CreateMemberCommand), new CreateMemberCommand("Jane", 21, "jane@example.com"), utc),
                new Message<CreateMemberCommand>("mid2", "urn:members:2".ToUri(), nameof(CreateMemberCommand), new CreateMemberCommand("John", 22, "john@example.com"), utc)
            };
            var sut = new MessageAsyncEnumerable<CreateMemberCommand>(source, o =>
            {
                o.MessageCallback = async message =>
                {
                    callbacks.Add(message.Id);
                    message.Properties["messageId"] = message.Id;
                    await message.AcknowledgeAsync();
                };
                o.AcknowledgedPropertiesCallback = properties =>
                {
                    acknowledged = properties.ToList();
                    return Task.CompletedTask;
                };
            });

            await foreach (var message in sut)
            {
                Assert.NotNull(message);
            }

            Assert.Equal(new[] { "mid1", "mid2" }, callbacks);
            Assert.NotNull(acknowledged);
            Assert.Equal(2, acknowledged.Count);
            Assert.Equal(new[] { "mid1", "mid2" }, acknowledged.Select(p => p["messageId"]).Cast<string>().OrderBy(id => id).ToArray());
        }
    }
}
