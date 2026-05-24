using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.NATS.EventDriven
{
    public class NatsEventBusOptionsTest : Test
    {
        public NatsEventBusOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_Should_Inherit_Defaults_From_Base_Type()
        {
            var options = new NatsEventBusOptions();

            Assert.IsAssignableFrom<NatsMessageOptions>(options);
            Assert.Equal(new System.Uri("nats://127.0.0.1:4222"), options.NatsUrl);
            Assert.Null(options.Subject);
        }

        [Fact]
        public void ValidateOptions_Should_Require_Subject()
        {
            var options = new NatsEventBusOptions();

            Assert.Throws<System.InvalidOperationException>(() => options.ValidateOptions());
        }
    }
}
