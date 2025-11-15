using Xunit;
using Codebelt.Extensions.Xunit;

namespace Savvyio.Extensions.NATS.EventDriven
{
    public class NatsEventBusOptionsTest : Test
    {
        public NatsEventBusOptionsTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void ShouldInheritFromNatsMessageOptions()
        {
            var options = new NatsEventBusOptions();
            Assert.IsAssignableFrom<NatsMessageOptions>(options);
        }
    }
}
