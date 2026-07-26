using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cuemon;
using Cuemon.Collections.Generic;
using Cuemon.Extensions;
using Codebelt.Extensions.Newtonsoft.Json;
using Cuemon.Extensions.Reflection;
using Cuemon.Reflection;
using Cuemon.Text;
using Newtonsoft.Json;
using Savvyio.Domain;

namespace Savvyio.Extensions.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="ValueObject"/> to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class ValueObjectConverter : JsonConverter<ValueObject>
    {
        private readonly ActivatorOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObjectConverter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="ActivatorOptions" /> which may be configured.</param>
        public ValueObjectConverter(Action<ActivatorOptions> setup = null)
        {
            _options = Patterns.Configure(setup);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
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

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read. If there is no existing value then <c>null</c> will be used.</param>
        /// <param name="hasExistingValue">The existing value has a value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override ValueObject ReadJson(JsonReader reader, Type objectType, ValueObject existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var properties = objectType.GetRuntimePropertiesExceptOf<ValueObject>().Where(pi => pi.CanRead).ToList();
            if (properties.Count == 1 && objectType.HasTypes(typeof(SingleValueObject<>)))
            {
                var property = properties.Single();
                if (property.DeclaringType?.IsAssignableFrom(objectType) ?? false)
                {
                    return Activator.CreateInstance(objectType, _options.Flags, _options.Binder, new[] { Decorator.Enclose(reader.Value).ChangeType(property.PropertyType) }, _options.FormatProvider as CultureInfo) as ValueObject;
                }
            }

            var propertyData = ReadPropertyData(reader, properties, serializer);

            var result = CreateValueObject(objectType, properties, propertyData);
            if (result != null) { return result; }

            throw ExceptionInsights.Embed(new InvalidOperationException($"Unable to deserialize {objectType.FullName}; consider adding a custom converter for this type."), MethodBase.GetCurrentMethod(), Arguments.ToArray(reader, objectType, existingValue, hasExistingValue, serializer));
        }

        private List<DataPair> ReadPropertyData(JsonReader reader, List<PropertyInfo> properties, JsonSerializer serializer)
        {
            var propertyData = new List<DataPair>();
            if (reader.TokenType != JsonToken.StartObject) { return propertyData; }

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
                            propertyData.Add(ReadDataPair(reader, matchingProperty, serializer));
                        }
                        break;
                }

                if (reader.Depth == depth && reader.TokenType == JsonToken.EndObject) { break; }
            }

            return propertyData;
        }

        private DataPair ReadDataPair(JsonReader reader, PropertyInfo matchingProperty, JsonSerializer serializer)
        {
            if (matchingProperty.PropertyType.HasTypes(typeof(SingleValueObject<>)))
            {
                return new DataPair(matchingProperty.Name, serializer.Deserialize(reader, matchingProperty.PropertyType), matchingProperty.PropertyType);
            }

            return new DataPair(matchingProperty.Name, ParserFactory.FromObject().Parse(reader.Value?.ToString(), matchingProperty.PropertyType, o => o.FormatProvider = _options.FormatProvider) ?? serializer.Deserialize(reader, matchingProperty.PropertyType), matchingProperty.PropertyType);
        }

        private ValueObject CreateValueObject(Type objectType, List<PropertyInfo> properties, List<DataPair> propertyData)
        {
            var ctors = objectType.GetConstructors(_options.Flags).ToList();
            if (ctors.Count == 0) { return null; }

            var matchingCtor = ctors.SingleOrDefault(info => MatchesConstructor(info, propertyData));
            if (matchingCtor != null)
            {
                return matchingCtor.Invoke(propertyData.Select(pair => pair.Value).ToArray()) as ValueObject;
            }

            var defaultCtor = ctors.SingleOrDefault(ci => ci.GetParameters().Length == 0);
            if (defaultCtor == null) { return null; }

            return PopulateDefaultConstructed(defaultCtor, properties, propertyData);
        }

        private static bool MatchesConstructor(ConstructorInfo info, List<DataPair> propertyData)
        {
            var paramters = info.GetParameters().ToList();
            return paramters.Count == propertyData.Count && paramters.Select(pi => pi.ParameterType).SequenceEqual(propertyData.Select(pair => pair.Type));
        }

        private static ValueObject PopulateDefaultConstructed(ConstructorInfo defaultCtor, List<PropertyInfo> properties, List<DataPair> propertyData)
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
