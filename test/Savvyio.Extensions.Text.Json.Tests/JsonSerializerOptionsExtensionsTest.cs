using System;
using System.Text.Json;
using Codebelt.Extensions.Xunit;
using Savvyio.Extensions.Text.Json.Converters;
using Xunit;

namespace Savvyio.Extensions.Text.Json
{
    public class JsonSerializerOptionsExtensionsTest : Test
    {
        public JsonSerializerOptionsExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Clone_ShouldThrowArgumentNullException_WhenOptionsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((JsonSerializerOptions)null).Clone());
        }

        [Fact]
        public void Clone_ShouldReturnNewInstance_WithoutSetup()
        {
            var original = new JsonSerializerOptions { WriteIndented = true };
            var cloned = original.Clone();

            Assert.NotSame(original, cloned);
            Assert.True(cloned.WriteIndented);
        }

        [Fact]
        public void Clone_ShouldApplySetup_WhenProvided()
        {
            var original = new JsonSerializerOptions { WriteIndented = true };
            var cloned = original.Clone(o => o.WriteIndented = false);

            Assert.NotSame(original, cloned);
            Assert.True(original.WriteIndented);
            Assert.False(cloned.WriteIndented);
        }
    }
}
