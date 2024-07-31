using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Cuemon.Extensions;
using Cuemon;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Extensions.SimpleQueueService.Assets;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;
using System.Runtime.InteropServices;
using System.Threading;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Collections.Generic;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Messaging.Cryptography;

namespace Savvyio.Extensions.SimpleQueueService.Commands
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AmazonCommandQueueNewtonsoftJsonSerializerContextTest : HostTest<HostFixture>
    {
        private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        private readonly AmazonCommandQueue _queue;
        private static readonly InMemoryTestStore<IMessage<ICommand>> Comparer = new();
        private readonly IMarshaller _marshaller;

        public AmazonCommandQueueNewtonsoftJsonSerializerContextTest(HostFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            _queue = fixture.ServiceProvider.GetRequiredService<AmazonCommandQueue>();
            _marshaller = fixture.ServiceProvider.GetRequiredService<IMarshaller>();
        }

        [Fact, Priority(0)]
        public async Task SendAsync_CreateMemberCommand_OneTime()
        {
            var sut1 = new CreateMemberCommand("John Doe", 44, "jd@outlook.com");
            var sut2 = "https://fancy.io/members".ToUri();
            var sut3 = sut1.ToMessage(sut2, nameof(CreateMemberCommand));

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = Environment.NewLine));

            TestOutput.WriteLine(_marshaller.Serialize(sut2).ToEncodedString());

            Comparer.Add(sut3);

            await _queue.SendAsync(sut3.Yield());
        }

        [Fact, Priority(1)]
        public async Task ReceiveAsync_CreateMemberCommand_OneTime()
        {
            var sut1 = Comparer.Query(message => message.Source == "https://fancy.io/members").Single();
            
            var ct = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            IMessage<ICommand> sut2 = null;
            while (sut2 == null)
            {
                sut2 = await _queue.ReceiveAsync().SingleOrDefaultAsync(ct).ConfigureAwait(false);
            }

            Assert.Equivalent(sut1.Data, sut2.Data);
            Assert.Equivalent(sut1.Time, sut2.Time);
            Assert.Equivalent(sut1.Source, sut2.Source);
            Assert.Equivalent(sut1.Id, sut2.Id);
            Assert.Equivalent(sut1.Type, sut2.Type);
        }

        [Fact, Priority(2)]
        public async Task SendAsync_CreateMemberCommand_OneTime_Signed()
        {
            var sut1 = new CreateMemberCommand("John Doe", 44, "jd@outlook.com");
            var sut2 = "https://fancy.io/members/signed".ToUri();
            var sut3 = sut1.ToMessage(sut2, nameof(CreateMemberCommand)).Sign(_marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = Environment.NewLine));

            TestOutput.WriteLine(_marshaller.Serialize(sut2).ToEncodedString());

            Comparer.Add(sut3);

            await _queue.SendAsync(sut3.Yield());
        }

        [Fact, Priority(3)]
        public async Task ReceiveAsync_CreateMemberCommand_OneTime_Signed()
        {
            var sut1 = Comparer.Query(message => message.Source == "https://fancy.io/members/signed").Single();
            
            var ct = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            ISignedMessage<ICommand> sut2 = null;
            while (sut2 == null)
            {
                sut2 = await _queue.ReceiveAsync().SingleOrDefaultAsync(ct).ConfigureAwait(false) as ISignedMessage<ICommand>;
                sut2?.CheckSignature(_marshaller, o => o.SignatureSecret = new byte[] { 1, 2, 3 });
            }

            Assert.Equivalent(sut1.Data, sut2.Data);
            Assert.Equivalent(sut1.Time, sut2.Time);
            Assert.Equivalent(sut1.Source, sut2.Source);
            Assert.Equivalent(sut1.Id, sut2.Id);
            Assert.Equivalent(sut1.Type, sut2.Type);
        }

        [Fact, Priority(5)]
        public async Task SendAsync_CreateMemberCommand_HundredTimes()
        {
            var messages = Generate.RangeOf(100, i =>
            {
                var email = $"{Generate.RandomString(5)}@outlook.com";
                var message = new CreateMemberCommand(Generate.RandomString(10), (byte)Generate.RandomNumber(byte.MaxValue), email).ToMessage($"urn:{i}:{email}".ToUri(), nameof(CreateMemberCommand));
                Comparer.Add(message);
                return message;
            }).ToList();

            var profiler = await TimeMeasure.WithActionAsync(_ => _queue.SendAsync(messages)).ConfigureAwait(false);

            TestOutput.WriteLine(profiler.ToString());
        }

        [Fact, Priority(6)]
        public async Task ReceiveAsync_CreateMemberCommand_All()
        {
            var ct = new CancellationTokenSource(TimeSpan.FromSeconds(90)).Token;
            var realizedCommands = new List<IMessage<ICommand>>();
            var sut1 = Comparer.Query(message => message.Source.StartsWith("urn")).ToList();

            while (realizedCommands.Count < sut1.Count)
            {
                realizedCommands.AddRange(await _queue.ReceiveAsync(o => o.CancellationToken = ct).ToListAsync(ct).ConfigureAwait(false));
            }
            
            TestOutput.WriteLine(realizedCommands.Count.ToString());
            TestOutput.WriteLines(realizedCommands.Take(10));

            Assert.Equivalent(sut1.Count, realizedCommands.Count);
            Assert.Equivalent(sut1, realizedCommands);
            Assert.Equivalent(sut1.Select(message => message.Data), realizedCommands.Select(message => message.Data));
            Assert.Equivalent(sut1.Select(message => message.Data.Metadata), realizedCommands.Select(message => message.Data.Metadata));
        }

        public override void ConfigureServices(IServiceCollection services)
        {
	        services.AddMarshaller<NewtonsoftJsonMarshaller>();
            services.AddAmazonCommandQueue(o =>
            {
	            var queue = IsLinux ? "newtonsoft-savvyio-commands" : "newtonsoft-savvyio-commands.fifo";
                o.Endpoint = RegionEndpoint.EUWest1;
                if (Configuration["AWS:LocalStack"] != null)
                {
                    o.Credentials = new BasicAWSCredentials("AKIAIOSFODNN7EXAMPLE", "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY");
                    o.SourceQueue = new Uri($"http://sqs.eu-west-1.localhost.localstack.cloud:4566/000000000000/{queue}");
                    o.ConfigureClient(client =>
                    {
                        client.ServiceURL = Configuration["AWS:LocalStack"];
                        client.AuthenticationRegion = RegionEndpoint.EUWest1.SystemName;
                    });
                }
                else
                {
                    o.Credentials = new BasicAWSCredentials(Configuration["AWS:IAM:AccessKey"], Configuration["AWS:IAM:SecretKey"]);
                    o.SourceQueue = new Uri($"https://sqs.eu-west-1.amazonaws.com/{Configuration["AWS:CallerIdentity"]}/{queue}");
                }
	            o.ReceiveContext.UseApproximateNumberOfMessages = true;
            });
        }
    }
}
