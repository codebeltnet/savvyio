using System;
using System.Text.Json;
using Cuemon;

namespace Savvyio.Extensions.Text.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JsonSerializerOptions"/> class.
    /// </summary>
    public static class JsonSerializerOptionsExtensions
    {
        /// <summary>
        /// Copies the options from a <see cref="JsonSerializerOptions"/> instance to a new instance.
        /// </summary>
        /// <param name="options">The <see cref="JsonSerializerOptions"/> to extend.</param>
        /// <param name="setup">The <see cref="JsonSerializerOptions" /> which may be configured.</param>
        /// <returns>A new cloned instance of <paramref name="options"/> with optional altering as specified by the <paramref name="setup"/> delegate.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        public static JsonSerializerOptions Clone(this JsonSerializerOptions options, Action<JsonSerializerOptions> setup = null)
        {
            Validator.ThrowIfNull(options);
            options = new JsonSerializerOptions(options);
            setup?.Invoke(options);
            return options;
        }
    }
}
