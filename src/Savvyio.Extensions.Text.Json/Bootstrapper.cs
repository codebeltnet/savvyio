using Cuemon.Extensions.Text.Json.Formatters;
using System.Threading;

namespace Savvyio.Extensions.Text.Json
{
    internal static class Bootstrapper
    {
        private static readonly Lock PadLock = new();
        private static bool _initialized;

        internal static void Initialize()
        {
            if (!_initialized)
            {
                lock (PadLock)
                {
                    if (!_initialized)
                    {
                        _initialized = true;
                        JsonFormatterOptions.DefaultConverters += list =>
                        {
                            list.AddRequestConverter();
                            list.AddMetadataDictionaryConverter();
                            list.AddMessageConverter();
                            list.AddSingleValueObjectConverter();
                            list.AddDateTimeConverter();
                            list.AddDateTimeOffsetConverter();
                        };
                    }
                }
            }
        }
    }
}
