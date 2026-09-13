using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Savvyio.Extensions.RabbitMQ
{
    internal static class MessageTestHelper
    {
        internal static async Task<List<T>> ReadAsync<T>(ChannelReader<T> reader, int count)
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var messages = new List<T>(count);
            while (messages.Count < count)
            {
                messages.Add(await reader.ReadAsync(timeout.Token));
            }
            return messages;
        }
    }
}
