using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
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
                services.AddMessageQueue<NatsCommandQueue, ICommand>().AddConfiguredOptions<NatsCommandQueueOptions>(o =>
                {
                    o.AutoAcknowledge = true;
                    o.Subject = "queue2";
                    o.StreamName = "stream";
                    o.ConsumerName = "client";
                });
            });

            var queue = managed.Host.Services.GetRequiredService<NatsCommandQueue>();
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

            await Task.Delay(200); // wait briefly to ensure subscription setup

            await queue.SendAsync(message.Yield()).ConfigureAwait(false);

            await Task.Delay(200);

            receivedMessages.Writer.Complete(); // mark channel write is complete

            var received = await receivedMessages.Reader.ReadAsync();

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
                services.AddMessageQueue<NatsCommandQueue, ICommand>().AddConfiguredOptions<NatsCommandQueueOptions>(o =>
                {
                    o.AutoAcknowledge = true;
                    o.Subject = Generate.RandomString(10);
                    o.StreamName = Generate.RandomString(10);
                    o.ConsumerName = Generate.RandomString(10);
                });
            });

            var queue = managed.Host.Services.GetRequiredService<NatsCommandQueue>();
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

            await Task.Delay(200); // wait briefly to ensure subscription setup

            await queue.SendAsync(message.Yield()).ConfigureAwait(false);

            await Task.Delay(200);

            receivedMessages.Writer.Complete(); // mark channel write is complete

            var received = (await receivedMessages.Reader.ReadAsync()) as ISignedMessage<ICommand>;
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
                services.AddMessageQueue<NatsCommandQueue, ICommand>().AddConfiguredOptions<NatsCommandQueueOptions>(o =>
                {
                    o.StreamName = "stream";
                    o.ConsumerName = "client";
                    o.Subject = Generate.RandomString(10);
                });
            });

            var queue = managed.Host.Services.GetRequiredService<NatsCommandQueue>();
            var marshaller = managed.Host.Services.GetRequiredService<IMarshaller>();

            var messages = Generate.RangeOf(100, i =>
            {
                var email = $"{Generate.RandomString(5)}@outlook.com";
                var message = new CreateMemberCommand(Generate.RandomString(10), (byte)Generate.RandomNumber(byte.MaxValue), email).ToMessage($"urn:{i}:{email}".ToUri(), nameof(CreateMemberCommand));
                return message;
            }).ToList();

            var receivedMessages = Channel.CreateUnbounded<IMessage<ICommand>>();
            
            var count = 0;
            Task.Run<Task>(async () =>
            {
                while (count < messages.Count) 
                {
                    await foreach (var msg in queue.ReceiveAsync().ConfigureAwait(false))
                    {
                        count++;
                        await receivedMessages.Writer.WriteAsync(msg).ConfigureAwait(false);
                        await msg.AcknowledgeAsync().ConfigureAwait(false);
                    }
                }
            });

            await Task.Delay(200); // wait briefly to ensure subscription setup

            await queue.SendAsync(messages).ConfigureAwait(false);

            await Task.Delay(750);

            receivedMessages.Writer.Complete(); // mark channel write is complete

            var received = await receivedMessages.Reader.ReadAllAsync().ToListAsync();

            TestOutput.WriteLine(received.Count.ToString());
            TestOutput.WriteLines(received.Take(10));

            Assert.Equivalent(messages.Count, received.Count);
            Assert.Equivalent(messages, received);
            Assert.Equivalent(messages.Select(message => message.Data), received.Select(message => message.Data));
            Assert.Equivalent(messages.Select(message => message.Data.Metadata), received.Select(message => message.Data.Metadata));
        }
    }
}
