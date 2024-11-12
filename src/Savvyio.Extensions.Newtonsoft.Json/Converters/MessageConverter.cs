using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Extensions;
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.EventDriven.Messaging.CloudEvents.Cryptography;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;
using Savvyio.Reflection;

namespace Savvyio.Extensions.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts an <see cref="IMessage{T}"/> (or derived) to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class MessageConverter : JsonConverter
    {
        internal static readonly Lazy<IList<TypeInfo>> CloudEventTypes = new(() => AssemblyContext.CurrentDomainAssemblies.SelectMany(a => a.DefinedTypes.Where(ti => ti.HasInterfaces(typeof(ICloudEvent<>)) &&
                                                                                                                                                                      ti is { IsAbstract: false, IsInterface: false })).ToList());

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) { return; }
            var objectType = value.GetType();
            var requestType = objectType.GetGenericArguments()[0];
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(MessageConverter<>).MakeGenericType(requestType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;
            converter.WriteJson(writer, value, serializer);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
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

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return (objectType.IsGenericType &&
                    objectType.GetGenericTypeDefinition().HasInterfaces(typeof(IMessage<>)));
        }
    }

    internal class MessageConverter<T> : JsonConverter<IMessage<T>> where T : IRequest
    {
        public MessageConverter()
        {
        }

        public override void WriteJson(JsonWriter writer, IMessage<T> value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(value.Id), serializer);
            writer.WriteValue(value.Id);
            writer.WritePropertyName(nameof(value.Source), serializer);
            writer.WriteValue(value.Source);
            writer.WritePropertyName(nameof(value.Type), serializer);
            writer.WriteValue(value.Type);
            writer.WritePropertyName(nameof(value.Time), serializer);
            writer.WriteObject(value.Time, serializer);
            writer.WritePropertyName(nameof(value.Data), serializer);
            writer.WriteObject(value.Data, serializer);

            if (value.GetType().HasInterfaces(typeof(ICloudEvent<>)))
            {
                dynamic ce = value;
                writer.WritePropertyName(nameof(ICloudEvent<IIntegrationEvent>.Specversion), serializer);
                writer.WriteValue(ce.Specversion);
            }

            if (value is ISignedMessage<T> sm)
            {
                writer.WritePropertyName(nameof(sm.Signature), serializer);
                writer.WriteValue(sm.Signature);
            }

            if (value is IDictionary<string, object> dictionary)
            {
                foreach (var kvp in dictionary)
                {
                    writer.WritePropertyName(kvp.Key);
                    writer.WriteObject(kvp.Value, serializer);
                }
            }

            writer.WriteEndObject();
        }

        public override IMessage<T> ReadJson(JsonReader reader, Type objectType, IMessage<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var idKey = serializer.ResolvePropertyKeyByConvention(nameof(IMessage<T>.Id));
            var sourceKey = serializer.ResolvePropertyKeyByConvention(nameof(IMessage<T>.Source));
            var timeKey = serializer.ResolvePropertyKeyByConvention(nameof(IMessage<T>.Time));
            var typeKey = serializer.ResolvePropertyKeyByConvention(nameof(IMessage<T>.Type));
            var dataKey = serializer.ResolvePropertyKeyByConvention(nameof(IMessage<T>.Data));
            var metadataKey = serializer.ResolvePropertyKeyByConvention(nameof(IMetadata.Metadata));
            var memberTypeKey = serializer.ResolveDictionaryKeyByConvention(nameof(MetadataDictionary.MemberType));
            var signatureKey = serializer.ResolvePropertyKeyByConvention(nameof(ISignedMessage<T>.Signature));

            var document = JObject.Load(reader);
            var id = document.Root[idKey]!.Value<string>();
            var source = document.Root[sourceKey]!.Value<string>().ToUri();
            var type = document.Root[typeKey]!.Value<string>();
            var memberType = Type.GetType(document.Root[dataKey]![metadataKey]![memberTypeKey]!.Value<string>());
            var time = document.Root[timeKey]!.Value<DateTime>().ToUniversalTime();
            var data = (T)serializer.Deserialize(document.Root[dataKey].CreateReader(), memberType);

            var message = new Message<T>(id, source, type, data, time);

            if (objectType.HasInterfaces(typeof(ICloudEvent<>)))
            {
                var specVersionKey = serializer.ResolvePropertyKeyByConvention(nameof(ICloudEvent<IIntegrationEvent>.Specversion));

                var requestType = objectType.GetGenericArguments()[0];
                var cloudEventType = MessageConverter.CloudEventTypes.Value.Single(ti => ti.FullName!.StartsWith("Savvyio.EventDriven.Messaging.CloudEvents.CloudEvent"));
                var specVersion = document.Root[specVersionKey]!.Value<string>();
                var cloudEvent = Activator.CreateInstance(cloudEventType.MakeGenericType(requestType), [message, specVersion]) as IMessage<T>;

                if (objectType.HasInterfaces(typeof(ISignedCloudEvent<>)))
                {
                    var signedCloudEventType = MessageConverter.CloudEventTypes.Value.Single(ti => ti.FullName!.StartsWith("Savvyio.EventDriven.Messaging.CloudEvents.Cryptography.SignedCloudEvent"));
                    var signature = document.Root[signatureKey]!.Value<string>();

                    return Activator.CreateInstance(signedCloudEventType.MakeGenericType(requestType), [cloudEvent, signature]) as IMessage<T>;
                }

                var reservedKeys = new[] { idKey, sourceKey, timeKey, typeKey, dataKey, metadataKey, signatureKey, specVersionKey };

                if (cloudEvent is IDictionary<string, object> dictionary)
                {
                    foreach (var property in document.Properties().Where(jp => !reservedKeys.Contains(jp.Name)))
                    {
                        dictionary.Add(property.Name, property.Value.ToObject(null, serializer));
                    }
                }

                return cloudEvent;
            }

            if (objectType.HasInterfaces(typeof(ISignedMessage<>)))
            {
                var signature = document.Root[signatureKey]!.Value<string>();
                return new SignedMessage<T>(message, signature);
            }

            return message;
        }
    }
}
