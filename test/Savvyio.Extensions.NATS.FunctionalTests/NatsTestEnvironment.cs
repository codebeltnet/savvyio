using System;

namespace Savvyio.Extensions.NATS
{
    internal static class NatsTestEnvironment
    {
        internal static Uri Url { get; } = new(Environment.GetEnvironmentVariable("SAVVYIO_NATS_URL") ?? "nats://127.0.0.1:4222");
    }
}
