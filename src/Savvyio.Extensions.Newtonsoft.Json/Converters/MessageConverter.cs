using System;
using System.Reflection;
using Cuemon.Extensions;
using Cuemon.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;

namespace Savvyio.Extensions.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts an <see cref="IMessage{T}"/> (or derived) to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class MessageConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteObject(value, serializer);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var requestType = objectType.GetGenericArguments()[0];
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(MessageConverter<>).MakeGenericType(requestType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;
            return converter.ReadJson(reader, objectType, existingValue, serializer);
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return (objectType.IsGenericType &&
                    (objectType.GetGenericTypeDefinition() == typeof(IMessage<>) 
                     || objectType.GetGenericTypeDefinition() == typeof(ISignedMessage<>)));
        }
    }

    internal class MessageConverter<T> : JsonConverter<IMessage<T>> where T : IRequest
    {
        public MessageConverter()
        {
        }

        public override void WriteJson(JsonWriter writer, IMessage<T> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IMessage<T> ReadJson(JsonReader reader, Type objectType, IMessage<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var document = JObject.Load(reader);
            var id = document.Root["id"].Value<string>();
            var source = document.Root["source"].Value<string>().ToUri();
            var type = document.Root["type"].Value<string>();
            var memberType = Type.GetType(document.Root["data"]["metadata"]["memberType"].Value<string>());
            var time = document.Root["time"].Value<DateTime>();
            var data = (T)serializer.Deserialize(document.Root["data"].CreateReader(), memberType);
            
            var message = new Message<T>(id, source, type, data, time);
            if (objectType == typeof(ISignedMessage<T>))
            {
                var signature = document.Root["signature"].Value<string>();
                return new SignedMessage<T>(message, signature);

            }
            return message;
        }
    }
}
