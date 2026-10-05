#nullable enable
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Codebelt.Extensions.Xunit;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.Text.Json.Converters;

public class MessageConverterGuardTest : Test
{
    public MessageConverterGuardTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Read_ShouldSupportRequestsWithoutMetadata()
    {
        var result = Roundtrip(new PlainRequest { Name = "Jane Doe" });

        Assert.Equal("Jane Doe", result.Data.Name);
    }

    [Fact]
    public void Read_ShouldPreserveExplicitMetadataUsingJsonPropertyName()
    {
        var request = new ExplicitMetadataRequest { Name = "Jane Doe" };
        request.Values.Add("custom", "value");

        var result = Roundtrip(request);

        Assert.Equal("Jane Doe", result.Data.Name);
        Assert.Equal("value", result.Data.Values["custom"].ToString());
    }

    [Fact]
    public void Read_ShouldAllowComputedMetadataWithoutWritablePropertyOrBackingField()
    {
        var result = Roundtrip(new ComputedMetadataRequest { Name = "Jane Doe" });

        Assert.Equal("Jane Doe", result.Data.Name);
        Assert.Equal("computed", result.Data.Metadata["source"]);
    }

    [Fact]
    public void AddExtensionAttributes_ShouldIgnoreMessagesWithoutDictionarySupport()
    {
        var options = CreateOptions();
        var converter = new MessageConverter().CreateConverter(typeof(Message<PlainRequest>), options);
        var message = CreateMessage(new PlainRequest { Name = "Jane Doe" });
        // The public path constructs CloudEvent<T>, which always implements IDictionary.
        // Exercise the internal helper's defensive no-op contract with a plain message.
        var helper = converter.GetType().GetMethod("AddExtensionAttributes", BindingFlags.Static | BindingFlags.NonPublic)!;

        var error = Record.Exception(() => helper.Invoke(null, [message, default(JsonElement), Array.Empty<string>(), options]));

        Assert.Null(error);
        Assert.Equal("Jane Doe", message.Data.Name);
    }

    private static Message<T> Roundtrip<T>(T request) where T : IRequest
    {
        var options = CreateOptions();
        var message = CreateMessage(request);
        var json = JsonSerializer.Serialize(message, options);
        var result = JsonSerializer.Deserialize<Message<T>>(json, options)!;
        Assert.Equal(message.Id, result.Id);
        Assert.Equal(message.Source, result.Source);
        Assert.Equal(message.Time, result.Time);
        return result;
    }

    private static Message<T> CreateMessage<T>(T request) where T : IRequest => new(
        "message-id", new Uri("https://example.test/requests"), typeof(T).Name, request,
        new DateTime(2023, 11, 16, 23, 24, 17, DateTimeKind.Utc));

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        options.Converters.Add(new MessageConverter());
        options.Converters.Add(new MetadataDictionaryConverter());
        return options;
    }

    private sealed class PlainRequest : IRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    private sealed class ExplicitMetadataRequest : IRequest, IMetadata
    {
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("metadata")]
        public IMetadataDictionary Values { get; set; } = new MetadataDictionary();

        IMetadataDictionary IMetadata.Metadata => Values;
    }

    private sealed class ComputedMetadataRequest : IRequest, IMetadata
    {
        public string Name { get; set; } = string.Empty;

        public IMetadataDictionary Metadata => new MetadataDictionary { { "source", "computed" } };
    }
}
