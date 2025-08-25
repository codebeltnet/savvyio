using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.EventDriven.Messaging.CloudEvents.Cryptography;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.NATS.Assets;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.NATS.EventDriven
{
    public class NatsEventBusNewtonsoftJsonSerializerContextTest : Test
    {
         public NatsEventBusNewtonsoftJsonSerializerContextTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task SubscribeAndPublishAsync_MemberCreated_OneTime()
        {
            var managed = HostTestFactory.Create(services =>
            {
                services.AddMarshaller<NewtonsoftJsonMarshaller>();
                services.AddMessageBus<NatsEventBus, IIntegrationEvent>(o => o.Lifetime = ServiceLifetime.Singleton).AddConfiguredOptions<NatsEventBusOptions>(o => o.Subject = Generate.RandomString(10));
            });

            var bus = managed.Host.Services.GetRequiredService<NatsEventBus>();
            var marshaller = managed.Host.Services.GetRequiredService<IMarshaller>();

            var member = new MemberCreated("John Doe", "jd@outlook.com");
            var urn = "urn:member-events-one".ToUri();
            var message = member.ToMessage(urn, $"{nameof(MemberCreated)}.updated-event");
            var receivedMessages = Channel.CreateUnbounded<IMessage<IIntegrationEvent>>();

            TestOutput.WriteLine(marshaller.Serialize(urn).ToEncodedString());
            TestOutput.WriteLine(marshaller.Serialize(message).ToEncodedString());
            
            var handlerInvocations = 0;
            Task.Run<Task>(async () =>
            {
                await bus.SubscribeAsync(async (msg, token) =>
                {
                    handlerInvocations++;
                    await receivedMessages.Writer.WriteAsync(msg, token).ConfigureAwait(false);
                }).ConfigureAwait(false);
            });

            await Task.Delay(200); // wait briefly to ensure subscription setup

            await bus.PublishAsync(message).ConfigureAwait(false);
            
            await Task.Delay(200);

            receivedMessages.Writer.Complete(); // mark channel write is complete
            
            var received = await receivedMessages.Reader.ReadAsync();

            Assert.Equal(1, handlerInvocations);
            Assert.Equivalent(message.Data, received.Data);
            Assert.Equivalent(message.Time, received.Time);
            Assert.Equivalent(message.Source, received.Source);
            Assert.Equivalent(message.Id, received.Id);
            Assert.Equivalent(message.Type, received.Type);
        }

        [Fact]
        public async Task SubscribeAndPublishAsync_MemberCreated_OneTime_Signed()
        {
            var managed = HostTestFactory.Create(services =>
            {
                services.AddMarshaller<NewtonsoftJsonMarshaller>();
                services.AddMessageBus<NatsEventBus, IIntegrationEvent>(o => o.Lifetime = ServiceLifetime.Singleton).AddConfiguredOptions<NatsEventBusOptions>(o => o.Subject = Generate.RandomString(10));
            });

            var bus = managed.Host.Services.GetRequiredService<NatsEventBus>();
            var marshaller = managed.Host.Services.GetRequiredService<IMarshaller>();

            var member = new MemberCreated("John Doe", "jd@outlook.com");
            var urn = "urn:member-events-one".ToUri();
            var message = member.ToMessage(urn, $"{nameof(MemberCreated)}.updated-event.signed").Sign(marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });
            var receivedMessages = Channel.CreateUnbounded<IMessage<IIntegrationEvent>>();

            TestOutput.WriteLine(marshaller.Serialize(urn).ToEncodedString());
            TestOutput.WriteLine(marshaller.Serialize(message).ToEncodedString());

            var handlerInvocations = 0;
            Task.Run<Task>(async () =>
            {
                await bus.SubscribeAsync(async (msg, token) =>
                {
                    handlerInvocations++;
                    await receivedMessages.Writer.WriteAsync(msg, token).ConfigureAwait(false);
                }).ConfigureAwait(false);
            });

            await Task.Delay(200); // wait briefly to ensure subscription setup

            await bus.PublishAsync(message).ConfigureAwait(false);
            
            await Task.Delay(200);

            receivedMessages.Writer.Complete(); // mark channel write is complete
            
            var received = await receivedMessages.Reader.ReadAsync() as ISignedMessage<IIntegrationEvent>;
            received?.CheckSignature(marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            Assert.Equal(1, handlerInvocations);
            Assert.Equivalent(message.Data, received.Data);
            Assert.Equivalent(message.Time, received.Time);
            Assert.Equivalent(message.Source, received.Source);
            Assert.Equivalent(message.Id, received.Id);
            Assert.Equivalent(message.Type, received.Type);
            Assert.Equivalent(message.Signature, received.Signature);
        }

        [Fact]
        public async Task SubscribeAndPublishAsync_MemberCreated_OneTime_CloudEvent()
        {
            var managed = HostTestFactory.Create(services =>
            {
                services.AddMarshaller<NewtonsoftJsonMarshaller>();
                services.AddMessageBus<NatsEventBus, IIntegrationEvent>(o => o.Lifetime = ServiceLifetime.Singleton).AddConfiguredOptions<NatsEventBusOptions>(o => o.Subject = Generate.RandomString(10));
            });

            var bus = managed.Host.Services.GetRequiredService<NatsEventBus>();
            var marshaller = managed.Host.Services.GetRequiredService<IMarshaller>();

            var member = new MemberCreated("John Doe", "jd@outlook.com");
            var urn = "urn:member-events-one".ToUri();
            var message = member.ToMessage(urn, $"{nameof(MemberCreated)}.updated-event.cloud-event").ToCloudEvent();
            var receivedMessages = Channel.CreateUnbounded<IMessage<IIntegrationEvent>>();

            TestOutput.WriteLine(marshaller.Serialize(urn).ToEncodedString());
            TestOutput.WriteLine(marshaller.Serialize(message).ToEncodedString());

            var handlerInvocations = 0;
            Task.Run<Task>(async () =>
            {
                await bus.SubscribeAsync(async (msg, token) =>
                {
                    handlerInvocations++;
                    await receivedMessages.Writer.WriteAsync(msg, token).ConfigureAwait(false);
                }).ConfigureAwait(false);
            });

            await Task.Delay(200); // wait briefly to ensure subscription setup
            
            await bus.PublishAsync(message);

            await Task.Delay(200);

            receivedMessages.Writer.Complete(); // mark channel write is complete
            
            var received = await receivedMessages.Reader.ReadAsync() as ICloudEvent<IIntegrationEvent>;

            Assert.Equal(1, handlerInvocations);
            Assert.Equivalent(message.Data, received.Data);
            Assert.Equivalent(message.Time, received.Time);
            Assert.Equivalent(message.Source, received.Source);
            Assert.Equivalent(message.Id, received.Id);
            Assert.Equivalent(message.Type, received.Type);
            Assert.Equivalent(message.Specversion, received.Specversion);
        }

        [Fact]
        public async Task SubscribeAndPublishAsync_MemberCreated_OneTime_CloudEvent_Signed()
        {
            var managed = HostTestFactory.Create(services =>
            {
                services.AddMarshaller<NewtonsoftJsonMarshaller>();
                services.AddMessageBus<NatsEventBus, IIntegrationEvent>(o => o.Lifetime = ServiceLifetime.Singleton).AddConfiguredOptions<NatsEventBusOptions>(o => o.Subject = Generate.RandomString(10));
            });

            var bus = managed.Host.Services.GetRequiredService<NatsEventBus>();
            var marshaller = managed.Host.Services.GetRequiredService<IMarshaller>();

            var member = new MemberCreated("John Doe", "jd@outlook.com");
            var urn = "urn:member-events-one".ToUri();
            var message = member.ToMessage(urn, $"{nameof(MemberCreated)}.updated-event.signed-cloud-event").ToCloudEvent().SignCloudEvent(marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });
            var receivedMessages = Channel.CreateUnbounded<IMessage<IIntegrationEvent>>();

            TestOutput.WriteLine(marshaller.Serialize(urn).ToEncodedString());
            TestOutput.WriteLine(marshaller.Serialize(message).ToEncodedString());

            var handlerInvocations = 0;
            Task.Run<Task>(async () =>
            {
                await bus.SubscribeAsync(async (msg, token) =>
                {
                    handlerInvocations++;
                    await receivedMessages.Writer.WriteAsync(msg, token).ConfigureAwait(false);
                }).ConfigureAwait(false);
            });

            await Task.Delay(200); // wait briefly to ensure subscription setup

            await bus.PublishAsync(message);

            await Task.Delay(200);

            receivedMessages.Writer.Complete(); // mark channel write is complete
            
            var received = await receivedMessages.Reader.ReadAsync() as ISignedCloudEvent<IIntegrationEvent>;

            Assert.Equal(1, handlerInvocations);
            Assert.Equivalent(message.Data, received.Data);
            Assert.Equivalent(message.Time, received.Time);
            Assert.Equivalent(message.Source, received.Source);
            Assert.Equivalent(message.Id, received.Id);
            Assert.Equivalent(message.Type, received.Type);
            Assert.Equivalent(message.Specversion, received.Specversion);
            Assert.Equivalent(message.Signature, received.Signature);
        }

        [Fact]
        public async Task SubscribeAndPublishAsync_MemberCreated_HundredTimes()
        {
            var managed = HostTestFactory.Create(services =>
            {
                services.AddMarshaller<NewtonsoftJsonMarshaller>();
                services.AddMessageBus<NatsEventBus, IIntegrationEvent>(o => o.Lifetime = ServiceLifetime.Singleton).AddConfiguredOptions<NatsEventBusOptions>(o => o.Subject = Generate.RandomString(10));
            });

            var bus = managed.Host.Services.GetRequiredService<NatsEventBus>();
            var marshaller = managed.Host.Services.GetRequiredService<IMarshaller>();

            var messages = Generate.RangeOf(100, _ =>
            {
                var email = $"{Generate.RandomString(5)}@outlook.com";
                return new MemberCreated(Generate.RandomString(10), email).ToMessage("urn:member-events-many".ToUri(), $"{nameof(MemberCreated)}");
            }).ToList();
            var receivedMessages1 = Channel.CreateUnbounded<IMessage<IIntegrationEvent>>();
            var receivedMessages2 = Channel.CreateUnbounded<IMessage<IIntegrationEvent>>();

            var handlerInvocations1 = 0;
            Task.Run<Task>(async () =>
            {
                await bus.SubscribeAsync(async (msg, token) =>
                {
                    handlerInvocations1++;
                    await receivedMessages1.Writer.WriteAsync(msg, token).ConfigureAwait(false);
                }).ConfigureAwait(false);
            });

            var handlerInvocations2 = 0;
            Task.Run<Task>(async () =>
            {
                await bus.SubscribeAsync(async (msg, token) =>
                {
                    handlerInvocations2++;
                    await receivedMessages2.Writer.WriteAsync(msg, token).ConfigureAwait(false);
                }).ConfigureAwait(false);
            });

            await Task.Delay(200); // wait briefly to ensure subscription setup

            await ParallelFactory.ForEachAsync(messages, (message, token) =>
            {
                return bus.PublishAsync(message, o => o.CancellationToken = token);
            }).ConfigureAwait(false);

            await Task.Delay(750);

            receivedMessages1.Writer.Complete(); // mark channel write is complete
            receivedMessages2.Writer.Complete(); // mark channel write is complete

            var received1 = await receivedMessages1.Reader.ReadAllAsync().ToListAsync();
            var received2 = await receivedMessages2.Reader.ReadAllAsync().ToListAsync();

            TestOutput.WriteLine(received1.Count.ToString());
            TestOutput.WriteLines(received1.Take(10));

            Assert.Equal(100, handlerInvocations1);
            Assert.Equal(100, handlerInvocations2);
            Assert.Equivalent(messages.Count, received1.Count);
            Assert.Equivalent(messages, received1);
            Assert.Equivalent(messages, received2);
            Assert.Equivalent(messages.Select(message => message.Data), received1.Select(message => message.Data));
            Assert.Equivalent(messages.Select(message => message.Data.Metadata), received1.Select(message => message.Data.Metadata));
            Assert.Equivalent(messages.Select(message => message.Data), received2.Select(message => message.Data));
            Assert.Equivalent(messages.Select(message => message.Data.Metadata), received2.Select(message => message.Data.Metadata));
        }
    }
}
