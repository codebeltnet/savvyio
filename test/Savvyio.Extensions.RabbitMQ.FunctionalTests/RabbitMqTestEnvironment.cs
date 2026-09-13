using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.RabbitMQ.Commands;
using Savvyio.Extensions.RabbitMQ.EventDriven;

namespace Savvyio.Extensions.RabbitMQ
{
    internal static class RabbitMqTestEnvironment
    {
        internal static Uri Url { get; } = new(Environment.GetEnvironmentVariable("SAVVYIO_RABBITMQ_URL") ?? "amqp://localhost:5672");

        private static Uri ManagementUrl { get; } = CreateManagementUrl();

        internal static Task WaitForCommandSubscriptionAsync(IServiceProvider services)
        {
            var queueName = services.GetRequiredService<RabbitMqCommandQueueOptions>().QueueName;
            return WaitUntilAsync("api/queues", root => root.EnumerateArray().Any(queue =>
                queue.TryGetProperty("name", out var name) && name.GetString() == queueName &&
                queue.TryGetProperty("consumers", out var consumers) && consumers.GetInt32() > 0), queueName);
        }

        internal static Task WaitForEventSubscriptionAsync(IServiceProvider services)
        {
            var exchangeName = services.GetRequiredService<RabbitMqEventBusOptions>().ExchangeName;
            return WaitUntilAsync("api/bindings", root => root.EnumerateArray().Any(binding =>
                binding.TryGetProperty("source", out var source) && source.GetString() == exchangeName), exchangeName);
        }

        private static Uri CreateManagementUrl()
        {
            var configured = Environment.GetEnvironmentVariable("SAVVYIO_RABBITMQ_MANAGEMENT_URL");
            if (!string.IsNullOrWhiteSpace(configured)) { return new Uri(configured.TrimEnd('/') + "/"); }

            return new UriBuilder(Url)
            {
                Scheme = Uri.UriSchemeHttp,
                Port = 15672,
                Path = "/",
                UserName = "",
                Password = ""
            }.Uri;
        }

        private static async Task WaitUntilAsync(string relativePath, Func<JsonElement, bool> predicate, string resourceName)
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var client = new HttpClient { BaseAddress = ManagementUrl };
            var credentials = string.IsNullOrWhiteSpace(Url.UserInfo) ? "guest:guest" : Uri.UnescapeDataString(Url.UserInfo);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials)));

            try
            {
                while (!timeout.IsCancellationRequested)
                {
                    try
                    {
                        using var response = await client.GetAsync(relativePath, timeout.Token).ConfigureAwait(false);
                        if (response.IsSuccessStatusCode)
                        {
                            await using var content = await response.Content.ReadAsStreamAsync(timeout.Token).ConfigureAwait(false);
                            using var document = await JsonDocument.ParseAsync(content, cancellationToken: timeout.Token).ConfigureAwait(false);
                            if (predicate(document.RootElement)) { return; }
                        }
                    }
                    catch (HttpRequestException) when (!timeout.IsCancellationRequested)
                    {
                    }

                    await Task.Delay(50, timeout.Token).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException) when (timeout.IsCancellationRequested)
            {
            }

            throw new TimeoutException($"RabbitMQ subscription for '{resourceName}' was not ready within 10 seconds.");
        }
    }
}
