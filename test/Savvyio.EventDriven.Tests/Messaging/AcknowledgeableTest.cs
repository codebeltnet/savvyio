using System.Threading.Tasks;
using Cuemon.Extensions;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Xunit;

namespace Savvyio.Messaging
{
    public class AcknowledgeableTest : Test
    {
        public AcknowledgeableTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task AcknowledgeAsync_ShouldRaiseAcknowledgedEvent_WithMessageProperties()
        {
            var message = new Message<CreateMemberCommand>("mid1", "urn:members:1".ToUri(), nameof(CreateMemberCommand), new CreateMemberCommand("Jane", 21, "j@j.com"));
            AcknowledgedEventArgs acknowledged = null;
            message.Properties["messageId"] = message.Id;
            message.Acknowledged += (_, e) =>
            {
                acknowledged = e;
                return Task.CompletedTask;
            };

            await message.AcknowledgeAsync();

            Assert.NotNull(acknowledged);
            Assert.Same(message.Properties, acknowledged.Properties);
            Assert.Equal(message.Id, acknowledged.Properties["messageId"]);
        }
    }
}
