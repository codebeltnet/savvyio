using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Cuemon.Threading;
using Savvyio.Assets.Commands;
using Savvyio.Commands.Assets;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Savvyio.Commands.Messaging
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class InMemoryCommandQueueTest : Test
    {
        private static readonly InMemoryCommandQueue Queue = new();
        private static readonly InMemoryTestStore<IMessage<ICommand>> Comparer = new();

        public InMemoryCommandQueueTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact, Priority(0)]
        public async Task SendAsync_CreateMemberCommand_OneTime()
        {
            var sut1 = new CreateMemberCommand("John Doe", 44, "jd@outlook.com");
            var sut2 = "https://fancy.io/members".ToUri();
            var sut3 = sut1.ToMessage(sut2);

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = Environment.NewLine));

            TestOutput.WriteLine(NewtonsoftJsonFormatter.SerializeObject(sut2).ToEncodedString());

            Comparer.Add(sut3);

            await Queue.SendAsync(sut3);
        }

        [Fact, Priority(1)]
        public async Task ReceiveAsync_CreateMemberCommand_OneTime()
        {
            var sut1 = Comparer.Query(message => message.Source == "https://fancy.io/members").Single();
            var sut2 = await Queue.ReceiveAsync().SingleOrDefaultAsync();

            Assert.Equal(sut1.Data, sut2.Data);
            Assert.Equal(sut1.Time, sut2.Time);
            Assert.Equal(sut1.Source, sut2.Source);
            Assert.Equal(sut1.Id, sut2.Id);
            Assert.Equal(sut1.Type, sut2.Type);
        }

        [Fact, Priority(2)]
        public async Task SendAsync_CreateMemberCommand_ThousandTimes()
        {
            var messages = Generate.RangeOf(1000, i =>
            {
                var email = $"{Generate.RandomString(5)}@outlook.com";
                return new CreateMemberCommand(Generate.RandomString(10), (byte)Generate.RandomNumber(byte.MaxValue), email).ToMessage($"urn:{i}:{email}".ToUri());
            });

            await ParallelFactory.ForEachAsync(messages, (message, token) =>
            {
                Comparer.Add(message);
                return Queue.SendAsync(message, o => o.CancellationToken = token);
            });
        }

        [Fact, Priority(3)]
        public async Task ReceiveAsync_CreateMemberCommand_All()
        {
            var sut1 = Comparer.Query(message => message.Source.StartsWith("urn")).ToList();
            var sut2 = (await Queue.ReceiveAsync(o =>
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
