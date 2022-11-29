using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Cuemon;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.Extensions.SimpleQueueService.Assets;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Savvyio.Extensions.SimpleQueueService.EventDriven
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AmazonEventBusTest : HostTest<HostFixture>
    {
        private readonly AmazonEventBus _bus;
        private static readonly InMemoryTestStore<IMessage<IIntegrationEvent>> Comparer = new();

        public AmazonEventBusTest(HostFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            _bus = fixture.ServiceProvider.GetRequiredService<AmazonEventBus>();
        }

        [Fact, Priority(0)]
        public async Task PublishAsync_MemberCreated_OneTime()
        {
            var sut1 = new MemberCreated("John Doe", "jd@outlook.com");
            var sut2 = "member-events-one.fifo".ToSnsUri();
            var sut3 = sut1.EncloseToMessage(sut2);

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = Environment.NewLine));

            Comparer.Add(sut3);

            await _bus.PublishAsync(sut3);
        }

        [Fact, Priority(1)]
        public async Task SubscribeAsync_MemberCreated_OneTime()
        {
            var handlerInvocations = 0;
            var sut1 = Comparer.Query(message => message.Source.EndsWith("member-events-one.fifo")).Single();
            
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
        public async Task PublishAsync_MemberCreated_ThousandTimes()
        {
            var messages = Generate.RangeOf(1000, _ =>
            {
                var email = $"{Generate.RandomString(5)}@outlook.com";
                return new MemberCreated(Generate.RandomString(10), email).EncloseToMessage("member-events-many.fifo".ToSnsUri());
            });

            await ParallelFactory.ForEachAsync(messages, (message, token) =>
            {
                Comparer.Add(message);
                return _bus.PublishAsync(message, o => o.CancellationToken = token);
            }).ConfigureAwait(false);
        }

        [Fact, Priority(3)]
        public async Task SubscribeAsync_MemberCreated_All()
        {
            var sut1 = Comparer.Query(message => message.Source.EndsWith("member-events-many.fifo")).ToList();
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
            services.AddSingleton(new AmazonEventBus(new OptionsWrapper<AmazonEventBusOptions>(new AmazonEventBusOptions
            {
                Credentials = new BasicAWSCredentials(Configuration["AWS:IAM:AccessKey"], Configuration["AWS:IAM:SecretKey"]),
                Endpoint = RegionEndpoint.EUWest1,
                SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{Configuration["AWS:CallerIdentity"]}/savvyio-events.fifo")
            })));
            AmazonResourceNameOptions.DefaultAccountId = Configuration["AWS:CallerIdentity"];
        }
    }
}
