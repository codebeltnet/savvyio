using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions;
using Cuemon;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Savvyio.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Extensions.SimpleQueueService.Assets;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;
using System.Runtime.InteropServices;

namespace Savvyio.Extensions.SimpleQueueService.Commands
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AmazonCommandQueueTest : HostTest<HostFixture>
    {
        private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        private readonly AmazonCommandQueue _queue;
        private static readonly InMemoryTestStore<IMessage<ICommand>> Comparer = new();

        public AmazonCommandQueueTest(HostFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            _queue = fixture.ServiceProvider.GetRequiredService<AmazonCommandQueue>();
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

            await _queue.SendAsync(sut3);
        }

        [Fact, Priority(1)]
        public async Task ReceiveAsync_CreateMemberCommand_OneTime()
        {
            var sut1 = Comparer.Query(message => message.Source == "https://fancy.io/members").Single();
            var sut2 = await _queue.ReceiveAsync().SingleOrDefaultAsync();

            Assert.Equivalent(sut1.Data, sut2.Data);
            Assert.Equivalent(sut1.Time, sut2.Time);
            Assert.Equivalent(sut1.Source, sut2.Source);
            Assert.Equivalent(sut1.Id, sut2.Id);
            Assert.Equivalent(sut1.Type, sut2.Type);
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
                return _queue.SendAsync(message, o => o.CancellationToken = token);
            }).ConfigureAwait(false);
        }

        [Fact, Priority(3)]
        public async Task ReceiveAsync_CreateMemberCommand_All()
        {
            var sut1 = Comparer.Query(message => message.Source.StartsWith("urn")).ToList();
            var sut2 = (await _queue.ReceiveAsync(o =>
            {
                o.MaxNumberOfMessages = int.MaxValue;
            })).ToList();
            
            TestOutput.WriteLine(sut2.Count.ToString());
            TestOutput.WriteLines(sut2.Take(10));

            Assert.Equivalent(sut1.Count, sut2.Count);
            Assert.Equivalent(sut1, sut2);
            Assert.Equivalent(sut1.Select(message => message.Data), sut2.Select(message => message.Data));
            Assert.Equivalent(sut1.Select(message => message.Data.Metadata), sut2.Select(message => message.Data.Metadata));
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var queue = IsLinux ? "savvyio-commands" : "savvyio-commands.fifo";
            services.AddSingleton(new AmazonCommandQueue(new OptionsWrapper<AmazonCommandQueueOptions>(new AmazonCommandQueueOptions
            {
                Credentials = new BasicAWSCredentials(Configuration["AWS:IAM:AccessKey"], Configuration["AWS:IAM:SecretKey"]),
                Endpoint = RegionEndpoint.EUWest1,
                SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{Configuration["AWS:CallerIdentity"]}/{queue}")
            })));
        }
    }
}
