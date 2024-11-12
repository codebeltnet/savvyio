using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Codebelt.Extensions.Xunit;
using Cuemon.Threading;
using Savvyio.Assets.EventDriven;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Savvyio.EventDriven.Messaging
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class InMemoryEventBusTest : Test
    {
        private static readonly InMemoryEventBus Bus = new();
        private static readonly InMemoryTestStore<IMessage<IIntegrationEvent>> Comparer = new();

        public InMemoryEventBusTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact, Priority(0)]
        public async Task PublishAsync_MemberCreated_OneTime()
        {
            var sut1 = new MemberCreated("John Doe", "jd@outlook.com");
            var sut2 = "urn:member-events:1".ToUri();
            var sut3 = sut1.ToMessage(sut2, nameof(MemberCreated));

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = Environment.NewLine));

            Comparer.Add(sut3);

            await Bus.PublishAsync(sut3);
        }

        [Fact, Priority(1)]
        public async Task SubscribeAsync_MemberCreated_OneTime()
        {
            var sut1 = Comparer.Query(message => message.Source == "urn:member-events:1").Single();
            Bus.SubscribeAsync((sut2, _) =>
            {
                Assert.Equal(sut1.Data, sut2.Data);
                Assert.Equal(sut1.Time, sut2.Time);
                Assert.Equal(sut1.Source, sut2.Source);
                Assert.Equal(sut1.Id, sut2.Id);
                Assert.Equal(sut1.Type, sut2.Type);
                return Task.CompletedTask;
            });
        }

        [Fact, Priority(2)]
        public async Task PublishAsync_MemberCreated_ThousandTimes()
        {
            var messages = Generate.RangeOf(1000, i =>
            {
                var email = $"{Generate.RandomString(5)}@outlook.com";
                return new MemberCreated(Generate.RandomString(10), email).ToMessage($"urn:member-events-many:{i}".ToUri(), nameof(MemberCreated));
            });

            await ParallelFactory.ForEachAsync(messages, (message, token) =>
            {
                Comparer.Add(message);
                return Bus.PublishAsync(message, o => o.CancellationToken = token);
            }).ConfigureAwait(false);
        }

        [Fact, Priority(3)]
        public async Task SubscribeAsync_MemberCreated_All()
        {
            var sut1 = Comparer.Query(message => message.Source.StartsWith("urn:member-events-many")).ToList();
            var sut2 = new List<IMessage<IIntegrationEvent>>();
            var sut3 = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            await Bus.SubscribeAsync((message, _) =>
            {
                sut2.Add(message);
                return Task.CompletedTask;
            }, o => o.CancellationToken = sut3.Token).ConfigureAwait(false);

            TestOutput.WriteLine(sut2.Count.ToString());
            TestOutput.WriteLines(sut2.Take(10));

            Assert.Equal(sut1.Count, sut2.Count);
            Assert.Equal(sut1, sut2);
            Assert.Equal(sut1.Select(message => message.Data), sut2.Select(message => message.Data));
            Assert.Equal(sut1.Select(message => message.Data.Metadata), sut2.Select(message => message.Data.Metadata));
        }
    }
}
