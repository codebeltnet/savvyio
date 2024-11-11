using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Text.Json;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.EventDriven.Messaging.CloudEvents.Cryptography;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;
using Savvyio.Reflection;

namespace Savvyio.Extensions.Text.Json.Converters
{
    /// <summary>
    /// Converts an <see cref="IMessage{T}"/> (or derived) to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class MessageConverter : JsonConverterFactory
    {
        internal static readonly Lazy<IList<TypeInfo>> CloudEventTypes = new(() => AssemblyContext.CurrentDomainAssemblies.SelectMany(a => a.DefinedTypes.Where(ti => ti.HasInterfaces(typeof(ICloudEvent<>)) &&
                                                                                                                                                                      ti is { IsAbstract: false, IsInterface: false })).ToList());

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConverter"/> class.
        /// </summary>
        public MessageConverter()
        {
        }

        /// <summary>
        /// When overridden in a derived class, determines whether the converter instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The type of the object to check whether it can be converted by this converter instance.</param>
        /// <returns><see langword="true" /> if the instance can convert the specified object type; otherwise, <see langword="false" />.</returns>
		public override bool CanConvert(Type typeToConvert)
        {
            return (typeToConvert.IsGenericType &&
                    typeToConvert.GetGenericTypeDefinition().HasInterfaces(typeof(IMessage<>)));
        }

        /// <summary>
        /// Creates a converter for a specified type.
        /// </summary>
        /// <param name="typeToConvert">The type handled by the converter.</param>
        /// <param name="options">The serialization options to use.</param>
        /// <returns>A converter that is compatible with <paramref name="typeToConvert" />.</returns>
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
                var idKey = options.PropertyNamingPolicy!.ConvertName(nameof(IMessage<T>.Id));
                var sourceKey = options.PropertyNamingPolicy.ConvertName(nameof(IMessage<T>.Source));
                var timeKey = options.PropertyNamingPolicy.ConvertName(nameof(IMessage<T>.Time));
                var typeKey = options.PropertyNamingPolicy.ConvertName(nameof(IMessage<T>.Type));
                var dataKey = options.PropertyNamingPolicy.ConvertName(nameof(IMessage<T>.Data));
                var metadataKey = options.PropertyNamingPolicy.ConvertName(nameof(IMetadata.Metadata));
                var signatureKey = options.PropertyNamingPolicy.ConvertName(nameof(ISignedMessage<T>.Signature));

                var id = document.RootElement.GetProperty(idKey).GetString();
                var source = document.RootElement.GetProperty(sourceKey).GetString().ToUri();
                var type = document.RootElement.GetProperty(typeKey).GetString();
                var memberType = typeToConvert.GenericTypeArguments[0];
                var time = document.RootElement.GetProperty(timeKey).GetDateTimeOffset().UtcDateTime;
                var data = (T)document.RootElement.GetProperty(dataKey).Deserialize(memberType!, options);
                if (data is IMetadata) // for unknown reasons, Microsoft does not use the custom converter for IMetadataDictionary here; have to fiddle extra around as seen below .. for the record; this just works with Newtonsoft!
                {
                    var md = document.RootElement.GetProperty(dataKey).GetProperty(metadataKey).Deserialize<IMetadataDictionary>(options);
                    var property = memberType.GetAllProperties().SingleOrDefault(pi => pi.Name == nameof(IMetadata.Metadata));
                    if (property != null)
                    {
                        if (property.CanWrite)
                        {
                            property.SetValue(data, md);
                        }
                        else
                        {
                            memberType.GetAllFields().SingleOrDefault(fi => fi.Name.Contains(nameof(IMetadata.Metadata)))?.SetValue(data, md);
                        }
                    }
                }

                var message = new Message<T>(id, source, type, data, time);

                if (typeToConvert.HasInterfaces(typeof(ICloudEvent<>)))
                {
                    var specVersionKey = options.PropertyNamingPolicy.ConvertName(nameof(ICloudEvent<IIntegrationEvent>.Specversion));

                    var requestType = typeToConvert.GetGenericArguments()[0];
                    var cloudEventType = MessageConverter.CloudEventTypes.Value.Single(ti => ti.FullName!.StartsWith("Savvyio.EventDriven.Messaging.CloudEvents.CloudEvent"));
                    var specVersion = document.RootElement.GetProperty(specVersionKey).GetString();
                    var cloudEvent = Activator.CreateInstance(cloudEventType.MakeGenericType(requestType), [message, specVersion]) as IMessage<T>;

                    if (typeToConvert.HasInterfaces(typeof(ISignedCloudEvent<>)))
                    {
                        var signedCloudEventType = MessageConverter.CloudEventTypes.Value.Single(ti => ti.FullName!.StartsWith("Savvyio.EventDriven.Messaging.CloudEvents.Cryptography.SignedCloudEvent"));
                        var signature = document.RootElement.GetProperty(signatureKey).GetString();

                        return Activator.CreateInstance(signedCloudEventType.MakeGenericType(requestType), [cloudEvent, signature]) as IMessage<T>;
                    }

                    var reservedKeys = new[] { idKey, sourceKey, timeKey, typeKey, dataKey, metadataKey, signatureKey, specVersionKey };

                    if (cloudEvent is IDictionary<string, object> dictionary)
                    {
                        foreach (var property in document.RootElement.EnumerateObject().Where(jp => !reservedKeys.Contains(jp.Name)))
                        {
                            dictionary.Add(property.Name, property.Value.Deserialize(property.Value.GetType(), options));
                        }
                    }

                    return cloudEvent;
                }

                if (typeToConvert.HasInterfaces(typeof(ISignedMessage<>)))
                {
                    var signature = document.RootElement.GetProperty(signatureKey).GetString();
                    return new SignedMessage<T>(message, signature);
                }

                return message;
            }
        }

        public override void Write(Utf8JsonWriter writer, IMessage<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(options.PropertyNamingPolicy.DefaultOrConvertName(nameof(value.Id)), value.Id);
            writer.WriteString(options.PropertyNamingPolicy.DefaultOrConvertName(nameof(value.Source)), value.Source);
            writer.WriteString(options.PropertyNamingPolicy.DefaultOrConvertName(nameof(value.Type)), value.Type);
            writer.WritePropertyName(options.PropertyNamingPolicy.DefaultOrConvertName(nameof(value.Time)));
            writer.WriteObject(value.Time, options);
            writer.WritePropertyName(options.PropertyNamingPolicy.DefaultOrConvertName(nameof(value.Data)));
            writer.WriteObject(value.Data, options);

            if (value.GetType().HasInterfaces(typeof(ICloudEvent<>)))
            {
                dynamic ce = value;
                writer.WriteString(options.PropertyNamingPolicy.DefaultOrConvertName(nameof(ICloudEvent<IIntegrationEvent>.Specversion)), ce.Specversion);
            }

            if (value is ISignedMessage<T> sm)
            {
                writer.WriteString(options.PropertyNamingPolicy.DefaultOrConvertName(nameof(sm.Signature)), sm.Signature);
            }

            if (value is IDictionary<string, object> dictionary)
            {
                foreach (var kvp in dictionary)
                {
                    writer.WritePropertyName(options.PropertyNamingPolicy.DefaultOrConvertName(kvp.Key));
                    writer.WriteObject(kvp.Value, options);
                }
            }

            writer.WriteEndObject();
        }
    }
}
