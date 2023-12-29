using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;

namespace Savvyio.Extensions.Text.Json.Converters
{
    /// <summary>
    /// Converts an <see cref="IMessage{T}"/> (or derived) to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class MessageConverter : JsonConverterFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConverter"/> class.
        /// </summary>
        public MessageConverter()
        {
        }

        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return (typeToConvert.IsGenericType && 
                    (typeToConvert.GetGenericTypeDefinition() == typeof(IMessage<>) 
                     || typeToConvert.GetGenericTypeDefinition() == typeof(ISignedMessage<>)));
        }

        /// <inheritdoc />
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var requestType = typeToConvert.GetGenericArguments()[0];
            return (JsonConverter)Activator.CreateInstance(
                typeof(MessageConverter<>).MakeGenericType(requestType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;
        }
    }

    internal class MessageConverter<T> : JsonConverter<IMessage<T>> where T : IRequest
    {
        public MessageConverter()
        {
        }

        public override IMessage<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var document = JsonDocument.ParseValue(ref reader))
            {
                var id = document.RootElement.GetProperty("id").GetString();
                var source = document.RootElement.GetProperty("source").GetString().ToUri();
                var type = typeToConvert.GenericTypeArguments[0];
                var time = document.RootElement.GetProperty("time").GetDateTime();
                var data = (T)document.RootElement.GetProperty("data").Deserialize(type!, options);
                if (data is IMetadata) // for unknown reasons, Microsoft does not use the custom converter for IMetadataDictionary here; have to fiddle extra around as seen below .. for the record; this just works with Newtonsoft!
                { 
                    var md = document.RootElement.GetProperty("data").GetProperty("metadata").Deserialize<IMetadataDictionary>(options);
                    var property = type.GetAllProperties().SingleOrDefault(pi => pi.Name == nameof(IMetadata.Metadata));
                    if (property != null)
                    {
                        if (property.CanWrite)
                        {
                            property.SetValue(data, md);
                        }
                        else
                        {
                            type.GetAllFields().SingleOrDefault(fi => fi.Name.Contains(nameof(IMetadata.Metadata)))?.SetValue(data, md);
                        }
                    }
                }

                var message = new Message<T>(id, source, data, type, time);
                if (typeToConvert == typeof(ISignedMessage<T>))
                {
                    var signature = document.RootElement.GetProperty("signature").GetString();
                    return new SignedMessage<T>(message, signature);
                }
                return message;
            }
        }

        public override void Write(Utf8JsonWriter writer, IMessage<T> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
