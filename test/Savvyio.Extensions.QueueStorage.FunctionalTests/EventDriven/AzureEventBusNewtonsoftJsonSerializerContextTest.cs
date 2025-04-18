using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.EventDriven.Messaging.CloudEvents.Cryptography;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.QueueStorage;
using Savvyio.Extensions.DependencyInjection.QueueStorage.EventDriven;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Extensions.QueueStorage.Assets;
using Savvyio.Extensions.QueueStorage.Commands;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Savvyio.Extensions.QueueStorage.EventDriven
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AzureEventBusNewtonsoftJsonSerializerContextTest : HostTest<ManagedHostFixture>
    {
        private static readonly string Platform = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux" : "win";
        private static readonly string BuildType = typeof(AzureCommandQueue).Assembly.IsDebugBuild() ? "debug" : "release";
        private static readonly InMemoryTestStore<IMessage<IIntegrationEvent>> Comparer = new();
        private readonly AzureEventBus _bus;
        private readonly IMarshaller _marshaller;

        public AzureEventBusNewtonsoftJsonSerializerContextTest(ManagedHostFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            _bus = ResolveEventBus(fixture);
            _marshaller = fixture.Host.Services.GetRequiredService<IMarshaller>();
        }

        private static AzureEventBus ResolveEventBus(ManagedHostFixture fixture)
        {
            return Platform switch
            {
                "win" => BuildType switch
                {
                    "debug" => fixture.Host.Services.GetRequiredService<AzureEventBus<WindowsDebug>>(),
                    "release" => fixture.Host.Services.GetRequiredService<AzureEventBus<WindowsRelease>>(),
                    _ => throw new NotSupportedException()
                },
                "linux" => BuildType switch
                {
                    "debug" => fixture.Host.Services.GetRequiredService<AzureEventBus<LinuxDebug>>(),
                    "release" => fixture.Host.Services.GetRequiredService<AzureEventBus<LinuxRelease>>(),
                    _ => throw new NotSupportedException()
                },
                _ => throw new NotSupportedException()
            };
        }

        [Fact, Priority(0)]
        public async Task PublishAsync_MemberCreated_OneTime()
        {
            var sut1 = new MemberCreated("John Doe", "jd@outlook.com");
            var sut2 = "urn:member-events-one".ToUri();
            var sut3 = sut1.ToMessage(sut2, $"{nameof(MemberCreated)}.{BuildType}.updated-event");

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = System.Environment.NewLine));
            TestOutput.WriteLine(Generate.ObjectPortrayal(sut3, o => o.Delimiter = System.Environment.NewLine));

            TestOutput.WriteLine(_marshaller.Serialize(sut3.ToCloudEvent()).ToEncodedString(o => o.LeaveOpen = true));

            Comparer.Add(sut3);

            await _bus.PublishAsync(sut3);
        }

        [Fact, Priority(1)]
        public async Task SubscribeAsync_MemberCreated_OneTime()
        {
            var handlerInvocations = 0;
            var sut1 = Comparer.Query(message => message.Type.Contains($"{BuildType}.updated-event")).Single();
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            while (!cancellationToken.IsCancellationRequested)
            {
                await _bus.SubscribeAsync((sut2, _) =>
                {
                    if (!sut2.Type.Contains($"{BuildType}.updated-event")) { return Task.CompletedTask; }
                    handlerInvocations++;
                    Assert.Equivalent(sut1.Data, sut2.Data);
                    Assert.Equivalent(sut1.Time, sut2.Time);
                    Assert.Equivalent(sut1.Source, sut2.Source);
                    Assert.Equivalent(sut1.Id, sut2.Id);
                    Assert.Equivalent(sut1.Type, sut2.Type);
                    sut2.AcknowledgeAsync();
                    cancellationToken.Cancel();
                    return Task.CompletedTask;
                }, o => o.CancellationToken = cancellationToken.Token).ConfigureAwait(false);
            }

            Assert.Equal(1, handlerInvocations);
        }

        [Fact, Priority(2)]
        public async Task PublishAsync_MemberCreated_OneTime_Signed()
        {
            var sut1 = new MemberCreated("John Doe", "jd@outlook.com");
            var sut2 = "urn:member-events-one".ToUri();
            var sut3 = sut1.ToMessage(sut2, $"{nameof(MemberCreated)}.{Platform}.{BuildType}.updated-event.signed").Sign(_marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = System.Environment.NewLine));

            TestOutput.WriteLine(_marshaller.Serialize(sut3).ToEncodedString(o => o.LeaveOpen = true));

            Comparer.Add(sut3);

            await _bus.PublishAsync(sut3);
        }

        [Fact, Priority(3)]
        public async Task SubscribeAsync_MemberCreated_OneTime_Signed()
        {
            var handlerInvocations = 0;
            var sut1 = Comparer.Query(message => message.Type.Contains($"{Platform}.{BuildType}.updated-event.signed")).Single();
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            while (!cancellationToken.IsCancellationRequested)
            {
                await _bus.SubscribeAsync((sut2, _) =>
                {
                    if (!sut2.Type.Contains($"{Platform}.{BuildType}.updated-event.signed")) { return Task.CompletedTask; }
                    ((ISignedMessage<IIntegrationEvent>)sut2).CheckSignature(_marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });
                    handlerInvocations++;
                    Assert.Equivalent(sut1.Data, sut2.Data);
                    Assert.Equivalent(sut1.Time, sut2.Time);
                    Assert.Equivalent(sut1.Source, sut2.Source);
                    Assert.Equivalent(sut1.Id, sut2.Id);
                    Assert.Equivalent(sut1.Type, sut2.Type);
                    sut2.AcknowledgeAsync();
                    cancellationToken.Cancel();
                    return Task.CompletedTask;
                }).ConfigureAwait(false);
            }
            Assert.Equal(1, handlerInvocations);
        }

        [Fact, Priority(4)]
        public async Task PublishAsync_MemberCreated_OneTime_CloudEvent()
        {
            var sut1 = new MemberCreated("John Doe", "jd@outlook.com");
            var sut2 = "urn:member-events-one".ToUri();
            var sut3 = sut1.ToMessage(sut2, $"{nameof(MemberCreated)}.{Platform}.{BuildType}.updated-event.cloud-event").ToCloudEvent();

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = System.Environment.NewLine));

            TestOutput.WriteLine(_marshaller.Serialize(sut3).ToEncodedString(o => o.LeaveOpen = true));

            Comparer.Add(sut3);

            await _bus.PublishAsync(sut3);
        }

        [Fact, Priority(5)]
        public async Task SubscribeAsync_MemberCreated_OneTime_CloudEvent()
        {
            var handlerInvocations = 0;
            var sut1 = Comparer.Query(message => message.Type.Contains($"{Platform}.{BuildType}.updated-event.cloud-event")).Single() as ICloudEvent<IIntegrationEvent>;
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            while (!cancellationToken.IsCancellationRequested)
            {
                await _bus.SubscribeAsync((sut2, _) =>
                {
                    if (!sut2.Type.Contains($"{Platform}.{BuildType}.updated-event.cloud-event")) { return Task.CompletedTask; }
                    handlerInvocations++;
                    Assert.Equivalent(sut1.Data, sut2.Data);
                    Assert.Equivalent(sut1.Time, sut2.Time);
                    Assert.Equivalent(sut1.Source, sut2.Source);
                    Assert.Equivalent(sut1.Id, sut2.Id);
                    Assert.Equivalent(sut1.Type, sut2.Type);
                    Assert.Equivalent(sut1.Specversion, ((ICloudEvent<IIntegrationEvent>)sut2).Specversion);
                    sut2.AcknowledgeAsync();
                    cancellationToken.Cancel();
                    return Task.CompletedTask;
                }).ConfigureAwait(false);

            }
            Assert.Equal(1, handlerInvocations);
        }

        [Fact, Priority(6)]
        public async Task PublishAsync_MemberCreated_OneTime_CloudEvent_Signed()
        {
            var sut1 = new MemberCreated("John Doe", "jd@outlook.com");
            var sut2 = "urn:member-events-one".ToUri();
            var sut3 = sut1.ToMessage(sut2, $"{nameof(MemberCreated)}.{Platform}.{BuildType}.updated-event.signed-cloud-event").ToCloudEvent().SignCloudEvent(_marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = System.Environment.NewLine));

            TestOutput.WriteLine(_marshaller.Serialize(sut3).ToEncodedString(o => o.LeaveOpen = true));

            Comparer.Add(sut3);

            await _bus.PublishAsync(sut3);
        }

        [Fact, Priority(7)]
        public async Task SubscribeAsync_MemberCreated_OneTime_CloudEvent_Signed()
        {
            var handlerInvocations = 0;
            var sut1 = Comparer.Query(message => message.Type.Contains($"{Platform}.{BuildType}.updated-event.signed-cloud-event")).Single();
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            while (!cancellationToken.IsCancellationRequested)
            {
                await _bus.SubscribeAsync((sut2, _) =>
                {
                    if (!sut2.Type.Contains($"{Platform}.{BuildType}.updated-event.signed-cloud-event")) { return Task.CompletedTask; }
                    ((ISignedCloudEvent<IIntegrationEvent>)sut2).CheckCloudEventSignature(_marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });
                    handlerInvocations++;
                    Assert.Equivalent(sut1.Data, sut2.Data);
                    Assert.Equivalent(sut1.Time, sut2.Time);
                    Assert.Equivalent(sut1.Source, sut2.Source);
                    Assert.Equivalent(sut1.Id, sut2.Id);
                    Assert.Equivalent(sut1.Type, sut2.Type);
                    sut2.AcknowledgeAsync();
                    cancellationToken.Cancel();
                    return Task.CompletedTask;
                }).ConfigureAwait(false);
            }
            Assert.Equal(1, handlerInvocations);
        }

        [Fact, Priority(8)]
        public async Task PublishAsync_MemberCreated_HundredTimes() // reduced as SNS is not part of free tier (FIFO)
        {
            var messages = Generate.RangeOf(100, _ =>
            {
                var email = $"{Generate.RandomString(5)}@outlook.com";
                return new MemberCreated(Generate.RandomString(10), email).ToMessage("urn:member-events-many".ToUri(), $"{nameof(MemberCreated)}.{Platform}.{BuildType}");
            });

            await ParallelFactory.ForEachAsync(messages, (message, token) =>
            {
                Comparer.Add(message);
                return _bus.PublishAsync(message, o => o.CancellationToken = token);
            }).ConfigureAwait(false);
        }

        [Fact, Priority(9)]
        public async Task SubscribeAsync_MemberCreated_All()
        {
            var sut1 = Comparer.Query(message => message.Source.Contains("member-events-many") && message.Type.Contains($"{Platform}.{BuildType}")).ToList();
            var sut2 = new List<IMessage<IIntegrationEvent>>();
            var sut3 = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            await _bus.SubscribeAsync((message, _) =>
            {
                if (!message.Type.Contains($"{Platform}.{BuildType}")) { return Task.CompletedTask; }
                sut2.Add(message);
                message.AcknowledgeAsync();
                return Task.CompletedTask;
            }, o => o.CancellationToken = sut3.Token).ConfigureAwait(false);

            TestOutput.WriteLine(sut2.Count.ToString());
            TestOutput.WriteLines(sut2.Take(10));

            Assert.Equivalent(sut1.Count, sut2.Count);
            Assert.Equivalent(sut1, sut2);
            Assert.Equivalent(sut1.Select(message => message.Data), sut2.Select(message => message.Data));
            Assert.Equivalent(sut1.Select(message => message.Data.Metadata), sut2.Select(message => message.Data.Metadata));
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var queueName = $"newtonsoft-savvyio-events-{Platform}-{BuildType}";
            var queueConnection = Configuration["Azure:Storage:QueueConnectionString"];

            services.AddMarshaller<NewtonsoftJsonMarshaller>();

            services.AddAzureEventBus<WindowsDebug>(o =>
            {
                o.QueueName = queueName;
                o.ConnectionString = queueConnection;
                o.PostConfigureClient(client =>
                {
                    client.CreateIfNotExists();
                    client.ClearMessages();
                });
            }, o =>
            {
                o.TopicEndpoint = "https://newtonsoft-savvyio-events-win-debug.swedencentral-1.eventgrid.azure.net/api/events".ToUri();
                o.KeyCredential = new AzureKeyCredential(Configuration["Azure:Newtonsoft:EventGrid:Windows:Debug:Key"]!);
            });

            services.AddAzureEventBus<WindowsRelease>(o =>
            {
                o.QueueName = queueName;
                o.ConnectionString = queueConnection;
                o.PostConfigureClient(client =>
                {
                    client.CreateIfNotExists();
                    client.ClearMessages();
                });
            }, o =>
            {
                o.TopicEndpoint = "https://newtonsoft-savvyio-events-win-release.swedencentral-1.eventgrid.azure.net/api/events".ToUri();
                o.KeyCredential = new AzureKeyCredential(Configuration["Azure:Newtonsoft:EventGrid:Windows:Release:Key"]!);
            });

            services.AddAzureEventBus<LinuxDebug>(o =>
            {
                o.QueueName = queueName;
                o.ConnectionString = queueConnection;
                o.PostConfigureClient(client =>
                {
                    client.CreateIfNotExists();
                    client.ClearMessages();
                });
            }, o =>
            {
                o.TopicEndpoint = "https://newtonsoft-savvyio-events-linux-debug.swedencentral-1.eventgrid.azure.net/api/events".ToUri();
                o.KeyCredential = new AzureKeyCredential(Configuration["Azure:Newtonsoft:EventGrid:Linux:Debug:Key"]!);
            });

            services.AddAzureEventBus<LinuxRelease>(o =>
            {
                o.QueueName = queueName;
                o.ConnectionString = queueConnection;
                o.PostConfigureClient(client =>
                {
                    client.CreateIfNotExists();
                    client.ClearMessages();
                });
            }, o =>
            {
                o.TopicEndpoint = "https://newtonsoft-savvyio-events-linux-release.swedencentral-1.eventgrid.azure.net/api/events".ToUri();
                o.KeyCredential = new AzureKeyCredential(Configuration["Azure:Newtonsoft:EventGrid:Linux:Release:Key"]!);
            });
        }
    }
}
