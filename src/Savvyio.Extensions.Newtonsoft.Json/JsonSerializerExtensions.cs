using Cuemon.Extensions.Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Savvyio.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Provides extension methods for the <see cref="JsonSerializer"/> class.
    /// </summary>
    public static class JsonSerializerExtensions
    {
        /// <summary>
        /// Resolves the property key by convention using the specified <see cref="JsonSerializer"/>.
        /// </summary>
        /// <param name="serializer">The <see cref="JsonSerializer"/> to use for resolving the property key.</param>
        /// <param name="name">The name of the property to resolve.</param>
        /// <returns>The resolved property key.</returns>
        public static string ResolvePropertyKeyByConvention(this JsonSerializer serializer, string name)
        {
            var ns = serializer.ContractResolver.ResolveNamingStrategyOrDefault();
            return ns.GetPropertyName(name, false);
        }

        /// <summary>
        /// Resolves the dictionary key by convention using the specified <see cref="JsonSerializer"/>.
        /// </summary>
        /// <param name="serializer">The <see cref="JsonSerializer"/> to use for resolving the dictionary key.</param>
        /// <param name="name">The name of the dictionary key to resolve.</param>
        /// <returns>The resolved dictionary key.</returns>
        public static string ResolveDictionaryKeyByConvention(this JsonSerializer serializer, string name)
        {
            var ns = serializer.ContractResolver.ResolveNamingStrategyOrDefault();
            return ns.GetDictionaryKey(name);
        }
    }
}
