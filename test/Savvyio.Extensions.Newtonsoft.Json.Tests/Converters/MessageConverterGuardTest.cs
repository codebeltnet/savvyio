#nullable enable
using System.Globalization;
using System.IO;
using Codebelt.Extensions.Xunit;
using Newtonsoft.Json;
using Xunit;

namespace Savvyio.Extensions.Newtonsoft.Json.Converters;

public class MessageConverterGuardTest : Test
{
    public MessageConverterGuardTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void WriteJson_ShouldLeaveWriterUntouchedWhenValueIsNull()
    {
        using var text = new StringWriter(CultureInfo.InvariantCulture);
        using var writer = new JsonTextWriter(text);

        new MessageConverter().WriteJson(writer, null!, new JsonSerializer());

        Assert.Equal(string.Empty, text.ToString());
        Assert.Equal(WriteState.Start, writer.WriteState);
    }
}
