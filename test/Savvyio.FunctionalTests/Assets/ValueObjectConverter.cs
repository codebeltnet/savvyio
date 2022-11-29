using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon;
using Cuemon.Collections.Generic;
using Cuemon.Extensions;
using Cuemon.Extensions.Newtonsoft.Json;
using Cuemon.Extensions.Reflection;
using Cuemon.Reflection;
using Cuemon.Text;
using Newtonsoft.Json;
using Savvyio.Domain;

namespace Savvyio.Assets
{
    public class ValueObjectConverter : JsonConverter<ValueObject>
    {
        private readonly ActivatorOptions _options;

        public ValueObjectConverter(Action<ActivatorOptions> setup = null)
        {
            _options = Patterns.Configure(setup);
        }

        public override void WriteJson(JsonWriter writer, ValueObject value, JsonSerializer serializer)
        {
            var properties = value.GetType().GetRuntimePropertiesExceptOf<ValueObject>().Where(pi => pi.CanRead).ToList();
            writer.WriteStartObject();

            foreach (var property in properties)
            {
                writer.WritePropertyName(property.Name, serializer);
                if (property.PropertyType.HasTypes(typeof(SingleValueObject<>)))
                {
                    var svo = property.GetValue(value);
                    writer.WriteValue(svo.GetType().GetProperty("Value").GetValue(svo));
                }
                else
                {
                    writer.WriteObject(property.GetValue(value), serializer);
                }
            }

            writer.WriteEndObject();
        }

        public override ValueObject ReadJson(JsonReader reader, Type objectType, ValueObject existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var properties = objectType.GetRuntimePropertiesExceptOf<ValueObject>().Where(pi => pi.CanRead).ToList();
            if (properties.Count == 1 && objectType.HasTypes(typeof(SingleValueObject<>)))
            {
                var property = properties.Single();
                if (property.DeclaringType?.IsAssignableFrom(objectType) ?? false)
                {
                    return Activator.CreateInstance(objectType, _options.Flags, _options.Binder, new[] { Decorator.Enclose(reader.Value).ChangeType(property.PropertyType) }, _options.FormatProvider) as ValueObject;
                }
            }

            var propertyData = new List<DataPair>();
            if (reader.TokenType == JsonToken.StartObject)
            {
                var depth = reader.Depth;
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.PropertyName:
                            var propertyName = (string)reader.Value;
                            var matchingProperty = properties.FirstOrDefault(pi => pi.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
                            if (matchingProperty != null)
                            {
                                reader.Read();
                                if (matchingProperty.PropertyType.HasTypes(typeof(SingleValueObject<>)))
                                {
                                    propertyData.Add(new DataPair(matchingProperty.Name, serializer.Deserialize(reader, matchingProperty.PropertyType), matchingProperty.PropertyType));
                                }
                                else
                                {
                                    propertyData.Add(new DataPair(matchingProperty.Name, ParserFactory.FromObject().Parse(reader.Value?.ToString(), matchingProperty.PropertyType, o => o.FormatProvider = _options.FormatProvider) ?? serializer.Deserialize(reader, matchingProperty.PropertyType), matchingProperty.PropertyType));
                                }
                            }
                            break;
                    }

                    if (reader.Depth == depth && reader.TokenType == JsonToken.EndObject) { break; }
                }
            }

            var ctors = objectType.GetConstructors(_options.Flags).ToList();
            if (ctors.Any())
            {
                var matchingCtor = ctors.SingleOrDefault(info =>
                {
                    var paramters = info.GetParameters().ToList();
                    return paramters.Count == propertyData.Count && paramters.Select(pi => pi.ParameterType).SequenceEqual(propertyData.Select(pair => pair.Type));
                });
                
                if (matchingCtor != null)
                {
                    return matchingCtor.Invoke(propertyData.Select(pair => pair.Value).ToArray()) as ValueObject;
                }
                else
                {
                    var defaultCtor = ctors.SingleOrDefault(ci => ci.GetParameters().Length == 0);
                    if (defaultCtor != null)
                    {
                        var vo = defaultCtor.Invoke(Array.Empty<object>()) as ValueObject;
                        foreach (var property in properties)
                        {
                            if (property.CanWrite)
                            {
                                property.SetValue(vo, propertyData.SingleOrDefault(pair => pair.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase))?.Value);
                            }
                        }
                        return vo;
                    }
                }
            }

            throw ExceptionInsights.Embed(new InvalidOperationException($"Unable to deserialize {objectType.FullName}; consider adding a custom converter for this type."), MethodBase.GetCurrentMethod(), Arguments.ToArray(reader, objectType, existingValue, hasExistingValue, serializer));
        }
    }
}
