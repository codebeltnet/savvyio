using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Cuemon;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.EventDriven.Messaging.CloudEvents.Cryptography;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService;
using Savvyio.Extensions.SimpleQueueService.Assets;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Savvyio.Extensions.SimpleQueueService.EventDriven
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AmazonEventBusJsonSerializerContextTest : HostTest<HostFixture>
    {
        private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        private readonly AmazonEventBus _bus;
        private static readonly InMemoryTestStore<IMessage<IIntegrationEvent>> Comparer = new();
        private readonly IMarshaller _marshaller;

        public AmazonEventBusJsonSerializerContextTest(HostFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            _bus = fixture.ServiceProvider.GetRequiredService<AmazonEventBus>();
            _marshaller = fixture.ServiceProvider.GetRequiredService<IMarshaller>();
        }

        [Fact, Priority(0)]
        public async Task PublishAsync_MemberCreated_OneTime()
        {
            var sut1 = new MemberCreated("John Doe", "jd@outlook.com");
            var sut2 = (IsLinux ? "member-events-one" : "member-events-one.fifo").ToSnsUri();
            var sut3 = sut1.ToMessage(sut2, "journal-svc.journals.updated-event");

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = Environment.NewLine));

            Comparer.Add(sut3);

            await _bus.PublishAsync(sut3);
        }

        [Fact, Priority(1)]
        public async Task SubscribeAsync_MemberCreated_OneTime()
        {
            var handlerInvocations = 0;
            var sut1 = Comparer.Query(message => message.Source.Contains("member-events-one")).Single();
            
            await _bus.SubscribeAsync((sut2, _) =>
            {
                handlerInvocations++;
                Assert.Equivalent(sut1.Data, sut2.Data);
                Assert.Equivalent(sut1.Time, sut2.Time);
                Assert.Equivalent(sut1.Source, sut2.Source);
                Assert.Equivalent(sut1.Id, sut2.Id);
                Assert.Equivalent(sut1.Type, sut2.Type);
                return Task.CompletedTask;
            }).ConfigureAwait(false);
            Assert.Equal(1, handlerInvocations);
        }

        [Fact, Priority(2)]
        public async Task PublishAsync_MemberCreated_OneTime_Signed()
        {
            var sut1 = new MemberCreated("John Doe", "jd@outlook.com");
            var sut2 = (IsLinux ? "member-events-one" : "member-events-one.fifo").ToSnsUri();
            var sut3 = sut1.ToMessage(sut2, "journal-svc.journals.updated-event.signed").Sign(_marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = Environment.NewLine));

            Comparer.Add(sut3);

            await _bus.PublishAsync(sut3);
        }

        [Fact, Priority(3)]
        public async Task SubscribeAsync_MemberCreated_OneTime_Signed()
        {
            var handlerInvocations = 0;
            var sut1 = Comparer.Query(message => message.Type.Contains("updated-event.signed")).Single();
            
            await _bus.SubscribeAsync((sut2, _) =>
            {
                ((ISignedMessage<IIntegrationEvent>)sut2).CheckSignature(_marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });
                handlerInvocations++;
                Assert.Equivalent(sut1.Data, sut2.Data);
                Assert.Equivalent(sut1.Time, sut2.Time);
                Assert.Equivalent(sut1.Source, sut2.Source);
                Assert.Equivalent(sut1.Id, sut2.Id);
                Assert.Equivalent(sut1.Type, sut2.Type);
                return Task.CompletedTask;
            }).ConfigureAwait(false);
            Assert.Equal(1, handlerInvocations);
        }

        [Fact, Priority(4)]
        public async Task PublishAsync_MemberCreated_OneTime_CloudEvent()
        {
            var sut1 = new MemberCreated("John Doe", "jd@outlook.com");
            var sut2 = (IsLinux ? "member-events-one" : "member-events-one.fifo").ToSnsUri();
            var sut3 = sut1.ToMessage(sut2, "journal-svc.journals.updated-event.cloud-event").ToCloudEvent();

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = Environment.NewLine));

            Comparer.Add(sut3);

            await _bus.PublishAsync(sut3);
        }

        [Fact, Priority(5)]
        public async Task SubscribeAsync_MemberCreated_OneTime_CloudEvent()
        {
            var handlerInvocations = 0;
            var sut1 = Comparer.Query(message => message.Type.Contains("updated-event.cloud-event")).Single() as ICloudEvent<IIntegrationEvent>;
            
            await _bus.SubscribeAsync((sut2, _) =>
            {
                handlerInvocations++;
                Assert.Equivalent(sut1.Data, sut2.Data);
                Assert.Equivalent(sut1.Time, sut2.Time);
                Assert.Equivalent(sut1.Source, sut2.Source);
                Assert.Equivalent(sut1.Id, sut2.Id);
                Assert.Equivalent(sut1.Type, sut2.Type);
                Assert.Equivalent(sut1.SpecVersion, ((ICloudEvent<IIntegrationEvent>)sut2).SpecVersion);
                return Task.CompletedTask;
            }).ConfigureAwait(false);
            Assert.Equal(1, handlerInvocations);
        }

        [Fact, Priority(6)]
        public async Task PublishAsync_MemberCreated_OneTime_CloudEvent_Signed()
        {
            var sut1 = new MemberCreated("John Doe", "jd@outlook.com");
            var sut2 = (IsLinux ? "member-events-one" : "member-events-one.fifo").ToSnsUri();
            var sut3 = sut1.ToMessage(sut2, "journal-svc.journals.updated-event.signed-cloud-event").ToCloudEvent().SignCloudEvent(_marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = Environment.NewLine));

            Comparer.Add(sut3);

            await _bus.PublishAsync(sut3);
        }

        [Fact, Priority(7)]
        public async Task SubscribeAsync_MemberCreated_OneTime_CloudEvent_Signed()
        {
            var handlerInvocations = 0;
            var sut1 = Comparer.Query(message => message.Type.Contains("updated-event.signed-cloud-event")).Single();
            
            await _bus.SubscribeAsync((sut2, _) =>
            {
                ((ISignedCloudEvent<IIntegrationEvent>)sut2).CheckCloudEventSignature(_marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });
                handlerInvocations++;
                Assert.Equivalent(sut1.Data, sut2.Data);
                Assert.Equivalent(sut1.Time, sut2.Time);
                Assert.Equivalent(sut1.Source, sut2.Source);
                Assert.Equivalent(sut1.Id, sut2.Id);
                Assert.Equivalent(sut1.Type, sut2.Type);
                return Task.CompletedTask;
            }).ConfigureAwait(false);
            Assert.Equal(1, handlerInvocations);
        }

        [Fact, Priority(8)]
        public async Task PublishAsync_MemberCreated_HundredTimes() // reduced as SNS is not part of free tier (FIFO)
        {
            var messages = Generate.RangeOf(100, _ =>
            {
                var email = $"{Generate.RandomString(5)}@outlook.com";
                return new MemberCreated(Generate.RandomString(10), email).ToMessage((IsLinux ? "member-events-many" : "member-events-many.fifo").ToSnsUri(), nameof(MemberCreated));
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
            var sut1 = Comparer.Query(message => message.Source.Contains("member-events-many")).ToList();
            var sut2 = new List<IMessage<IIntegrationEvent>>();
            var sut3 = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            await _bus.SubscribeAsync((message, _) =>
            {
                sut2.Add(message);
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
            AmazonResourceNameOptions.DefaultAccountId = Configuration["AWS:CallerIdentity"];
            
            services.AddMarshaller<JsonMarshaller>();
            services.AddAmazonEventBus(o =>
            {
	            var queue = IsLinux ? "savvyio-events" : "savvyio-events.fifo";
	            o.Credentials = new BasicAWSCredentials(Configuration["AWS:IAM:AccessKey"], Configuration["AWS:IAM:SecretKey"]);
	            o.Endpoint = RegionEndpoint.EUWest1;
	            o.SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{Configuration["AWS:CallerIdentity"]}/{queue}");
            });
        }
    }
}
