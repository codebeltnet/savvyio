using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Cuemon.Extensions;
using Cuemon;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Extensions.SimpleQueueService.Assets;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Savvyio.Extensions.Newtonsoft.Json;

namespace Savvyio.Extensions.SimpleQueueService.Commands
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AmazonCommandQueueNewtonsoftJsonSerializerContextTest : HostTest<HostFixture>
    {
        private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        private readonly AmazonCommandQueue _queue;
        private static readonly InMemoryTestStore<IMessage<ICommand>> Comparer = new();
        private readonly ISerializerContext _serializer;

        public AmazonCommandQueueNewtonsoftJsonSerializerContextTest(HostFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            _queue = fixture.ServiceProvider.GetRequiredService<AmazonCommandQueue>();
            _serializer = fixture.ServiceProvider.GetRequiredService<ISerializerContext>();
        }

        [Fact, Priority(0)]
        public async Task SendAsync_CreateMemberCommand_OneTime()
        {
            var sut1 = new CreateMemberCommand("John Doe", 44, "jd@outlook.com");
            var sut2 = "https://fancy.io/members".ToUri();
            var sut3 = sut1.ToMessage(sut2);

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = Environment.NewLine));

            TestOutput.WriteLine(_serializer.Serialize(sut2).ToEncodedString());

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

            await ParallelFactory.ForEachAsync(messages, async (message, token) =>
            {
                Comparer.Add(message);
                await _queue.SendAsync(message, o => o.CancellationToken = token).ConfigureAwait(false);
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
            services.AddTransient<ISerializerContext, NewtonsoftJsonSerializerContext>(_ =>
            {
                var newtonsoftjsonSerializer = new NewtonsoftJsonSerializerContext(o =>
                {
                    o.Settings.ContractResolver = new CamelCasePropertyNamesContractResolver
                    {
                        IgnoreSerializableInterface = true,
                        NamingStrategy = new CamelCaseNamingStrategy
                        {
                            ProcessDictionaryKeys = false
                        }
                    };
                    o.Settings.Converters
                        .AddMessageConverter()
                        .AddMetadataDictionaryConverter();
                });
                return newtonsoftjsonSerializer;
            });

            services.Configure<AmazonCommandQueueOptions>(o =>
            {
                var queue = IsLinux ? "newtonsoft-savvyio-commands" : "newtonsoft-savvyio-commands.fifo";
                o.Credentials = new BasicAWSCredentials(Configuration["AWS:IAM:AccessKey"], Configuration["AWS:IAM:SecretKey"]);
                o.Endpoint = RegionEndpoint.EUWest1;
                o.SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{Configuration["AWS:CallerIdentity"]}/{queue}");
            });

            services.AddScoped<AmazonCommandQueue>();
        }
    }
}
