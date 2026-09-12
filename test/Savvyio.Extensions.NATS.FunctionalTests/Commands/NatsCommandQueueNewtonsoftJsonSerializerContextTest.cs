using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;
using Cuemon.Extensions.IO;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.NATS.Assets;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Xunit;

namespace Savvyio.Extensions.NATS.Commands
{
    public class NatsCommandQueueNewtonsoftJsonSerializerContextTest : Test
    {
        public NatsCommandQueueNewtonsoftJsonSerializerContextTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ReceiveAndSendAsync_CreateMemberCommand_OneTime()
        {
            var managed = HostTestFactory.Create(services =>
            {
                services.AddMarshaller<NewtonsoftJsonMarshaller>();
                services.AddMessageQueue<ObservableNatsCommandQueue, ICommand>().AddConfiguredOptions<NatsCommandQueueOptions>(o =>
                {
                    o.NatsUrl = NatsTestEnvironment.Url;
                    o.AutoAcknowledge = true;
                    o.Subject = Guid.NewGuid().ToString();
                    o.StreamName = Guid.NewGuid().ToString();
                    o.ConsumerName = Guid.NewGuid().ToString();
                });
            });

            var queue = managed.Host.Services.GetRequiredService<ObservableNatsCommandQueue>();
            var marshaller = managed.Host.Services.GetRequiredService<IMarshaller>();

            var member = new CreateMemberCommand("John Doe", 44, "jd@outlook.com");
            var urn = "https://fancy.io/members".ToUri();
            var message = member.ToMessage(urn, nameof(CreateMemberCommand));
            var receivedMessages = Channel.CreateUnbounded<IMessage<ICommand>>();

            TestOutput.WriteLine(marshaller.Serialize(urn).ToEncodedString());

            Task.Run<Task>(async () =>
            {
                await foreach (var msg in queue.ReceiveAsync().ConfigureAwait(false))
                {
                    await receivedMessages.Writer.WriteAsync(msg).ConfigureAwait(false);
                }
            });

            await queue.WaitUntilConsumerReadyAsync();

            await queue.SendAsync(message.Yield()).ConfigureAwait(false);

            var received = await receivedMessages.Reader.ReadAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(10));

            Assert.Equivalent(message.Data, received.Data);
            Assert.Equivalent(message.Time, received.Time);
            Assert.Equivalent(message.Source, received.Source);
            Assert.Equivalent(message.Id, received.Id);
            Assert.Equivalent(message.Type, received.Type);
        }

        [Fact]
        public async Task ReceiveAndSendAsync_CreateMemberCommand_OneTime_Signed()
        {
            var managed = HostTestFactory.Create(services =>
            {
                services.AddMarshaller<NewtonsoftJsonMarshaller>();
                services.AddMessageQueue<ObservableNatsCommandQueue, ICommand>().AddConfiguredOptions<NatsCommandQueueOptions>(o =>
                {
                    o.NatsUrl = NatsTestEnvironment.Url;
                    o.AutoAcknowledge = true;
                    o.Subject = Guid.NewGuid().ToString();
                    o.StreamName = Guid.NewGuid().ToString();
                    o.ConsumerName = Guid.NewGuid().ToString();
                });
            });

            var queue = managed.Host.Services.GetRequiredService<ObservableNatsCommandQueue>();
            var marshaller = managed.Host.Services.GetRequiredService<IMarshaller>();

            var member = new CreateMemberCommand("John Doe", 44, "jd@outlook.com");
            var urn = "https://fancy.io/members/signed".ToUri();
            var message = member.ToMessage(urn, nameof(CreateMemberCommand)).Sign(marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });
            var receivedMessages = Channel.CreateUnbounded<IMessage<ICommand>>();

            TestOutput.WriteLine(marshaller.Serialize(message).ToEncodedString());

            Task.Run<Task>(async () =>
            {
                await foreach (var msg in queue.ReceiveAsync().ConfigureAwait(false))
                {
                    await receivedMessages.Writer.WriteAsync(msg).ConfigureAwait(false);
                }
            });

            await queue.WaitUntilConsumerReadyAsync();

            await queue.SendAsync(message.Yield()).ConfigureAwait(false);

            var received = (await receivedMessages.Reader.ReadAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(10))) as ISignedMessage<ICommand>;
            received?.CheckSignature(marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            Assert.Equivalent(message.Data, received.Data);
            Assert.Equivalent(message.Time, received.Time);
            Assert.Equivalent(message.Source, received.Source);
            Assert.Equivalent(message.Id, received.Id);
            Assert.Equivalent(message.Type, received.Type);
        }

        [Fact]
        public async Task ReceiveAndSendAsync_CreateMemberCommand_HundredTimes()
        {
            var managed = HostTestFactory.Create(services =>
            {
                services.AddMarshaller<NewtonsoftJsonMarshaller>();
                services.AddMessageQueue<ObservableNatsCommandQueue, ICommand>().AddConfiguredOptions<NatsCommandQueueOptions>(o =>
                {
                    o.NatsUrl = NatsTestEnvironment.Url;
                    o.StreamName = Guid.NewGuid().ToString();
                    o.ConsumerName = Guid.NewGuid().ToString();
                    o.Subject = Guid.NewGuid().ToString();
                });
            });

            var queue = managed.Host.Services.GetRequiredService<ObservableNatsCommandQueue>();
            var marshaller = managed.Host.Services.GetRequiredService<IMarshaller>();

            var messages = Generate.RangeOf(100, i =>
            {
                var email = $"{Generate.RandomString(5)}@outlook.com";
                var message = new CreateMemberCommand(Generate.RandomString(10), (byte)Generate.RandomNumber(byte.MaxValue), email).ToMessage($"urn:{i}:{email}".ToUri(), nameof(CreateMemberCommand));
                return message;
            }).ToList();

            var receivedMessages = Channel.CreateUnbounded<IMessage<ICommand>>();

            var count1 = 0;
            Task.Run<Task>(async () =>
            {
                while (count1 < messages.Count)
                {
                    await foreach (var msg in queue.ReceiveAsync().ConfigureAwait(false))
                    {
                        Interlocked.Increment(ref count1);
                        await receivedMessages.Writer.WriteAsync(msg).ConfigureAwait(false);
                        await msg.AcknowledgeAsync().ConfigureAwait(false);
                    }
                }
            });

            var count2 = 0;
            Task.Run<Task>(async () =>
            {
                while (count2 < messages.Count)
                {
                    await foreach (var msg in queue.ReceiveAsync().ConfigureAwait(false))
                    {
                        Interlocked.Increment(ref count2);
                        await receivedMessages.Writer.WriteAsync(msg).ConfigureAwait(false);
                        await msg.AcknowledgeAsync().ConfigureAwait(false);
                    }
                }
            });

            await queue.WaitUntilConsumerReadyAsync();

            await queue.SendAsync(messages).ConfigureAwait(false);

            var received = await MessageTestHelper.ReadAsync(receivedMessages.Reader, messages.Count);

            Assert.Equal(messages.Count, count1 + count2);

            TestOutput.WriteLine(count1.ToString());
            TestOutput.WriteLine(count2.ToString());

            TestOutput.WriteLine(received.Count.ToString());
            TestOutput.WriteLines(received.Take(10));

            Assert.Equivalent(messages.Count, received.Count);
            Assert.Equivalent(messages, received);
            Assert.Equivalent(messages.Select(message => message.Data), received.Select(message => message.Data));
            Assert.Equivalent(messages.Select(message => message.Data.Metadata), received.Select(message => message.Data.Metadata));
        }
    }
}
