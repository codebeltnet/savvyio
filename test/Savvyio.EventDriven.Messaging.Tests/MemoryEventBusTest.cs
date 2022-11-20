using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.Xunit;
using Cuemon.Threading;
using Savvyio.EventDriven.Messaging.Assets;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Savvyio.EventDriven.Messaging
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class MemoryEventBusTest : Test
    {
        private static readonly MemoryEventBus Bus = new();
        private static readonly InMemoryTestStore<IMessage<IIntegrationEvent>> Comparer = new();

        public MemoryEventBusTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact, Priority(0)]
        public async Task PublishAsync_MemberCreated_OneTime()
        {
            var sut1 = new MemberCreated("John Doe", "jd@outlook.com");
            var sut2 = "urn:member-events:1".ToUri();
            var sut3 = sut1.EncloseToMessage(sut2);

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = Environment.NewLine));

            Comparer.Add(sut3);

            await Bus.PublishAsync(sut3);
        }

        [Fact, Priority(1)]
        public async Task SubscribeAsync_MemberCreated_OneTime()
        {
            var sut1 = Comparer.Query(message => message.Source == "urn:member-events:1").Single();
            var sut2 = (await Bus.SubscribeAsync()).Single();

            Assert.Equal(sut1.Data, sut2.Data);
            Assert.Equal(sut1.Time, sut2.Time);
            Assert.Equal(sut1.Source, sut2.Source);
            Assert.Equal(sut1.Id, sut2.Id);
            Assert.Equal(sut1.Type, sut2.Type);
        }

        [Fact, Priority(2)]
        public async Task PublishAsync_MemberCreated_ThousandTimes()
        {
            var messages = Generate.RangeOf(1000, i =>
            {
                var email = $"{Generate.RandomString(5)}@outlook.com";
                return new MemberCreated(Generate.RandomString(10), email).EncloseToMessage($"urn:member-events-many:{i}".ToUri());
            });

            await ParallelFactory.ForEachAsync(messages, (message, token) =>
            {
                Comparer.Add(message);
                return Bus.PublishAsync(message, o => o.CancellationToken = token);
            });
        }

        [Fact, Priority(3)]
        public async Task SubscribeAsync_MemberCreated_All()
        {
            var sut1 = Comparer.Query(message => message.Source.StartsWith("urn:member-events-many")).ToList();
            var sut2 = (await Bus.SubscribeAsync(o =>
            {
                o.MaxNumberOfMessages = int.MaxValue;
            })).ToList();
            
            TestOutput.WriteLines(sut2.Take(10));

            Assert.Equal(sut1.Count, sut2.Count);
            Assert.Equal(sut1, sut2);
            Assert.Equal(sut1.Select(message => message.Data), sut2.Select(message => message.Data));
            Assert.Equal(sut1.Select(message => message.Data.Metadata), sut2.Select(message => message.Data.Metadata));
        }
    }
}
