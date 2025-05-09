﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Diagnostics;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Reflection;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Savvyio.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.QueueStorage;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Extensions.QueueStorage.Assets;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Savvyio.Extensions.QueueStorage.Commands
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AzureCommandQueueNewtonsoftJsonSerializerContextTest : HostTest<ManagedHostFixture>
    {
        private static readonly string Platform = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux" : "windows";
        private static readonly string BuildType = typeof(AzureCommandQueue).Assembly.IsDebugBuild() ? "debug" : "release";
        private static readonly InMemoryTestStore<IMessage<ICommand>> Comparer = new();
        private readonly AzureCommandQueue _queue;
        private readonly IMarshaller _marshaller;

        public AzureCommandQueueNewtonsoftJsonSerializerContextTest(ManagedHostFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            _queue = fixture.Host.Services.GetRequiredService<AzureCommandQueue>();
            _marshaller = fixture.Host.Services.GetRequiredService<IMarshaller>();
        }

        [Fact, Priority(0)]
        public async Task SendAsync_CreateMemberCommand_OneTime()
        {
            var sut1 = new CreateMemberCommand("John Doe", 44, "jd@outlook.com");
            var sut2 = "https://fancy.io/members".ToUri();
            var sut3 = sut1.ToMessage(sut2, nameof(CreateMemberCommand));

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = System.Environment.NewLine));

            TestOutput.WriteLine(_marshaller.Serialize(sut3).ToEncodedString());

            Comparer.Add(sut3);

            await _queue.SendAsync(sut3.Yield()).ConfigureAwait(false);
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
                if (sut2 != null) { await sut2.AcknowledgeAsync().ConfigureAwait(false); }
            }

            TestOutput.WriteLine(_marshaller.Serialize(sut2).ToEncodedString());

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

            TestOutput.WriteLine(Generate.ObjectPortrayal(sut2, o => o.Delimiter = System.Environment.NewLine));

            TestOutput.WriteLine(_marshaller.Serialize(sut2).ToEncodedString());

            Comparer.Add(sut3);

            await _queue.SendAsync(sut3.Yield()).ConfigureAwait(false);
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
                if (sut2 != null) { await sut2.AcknowledgeAsync().ConfigureAwait(false); }
            }

            Assert.Equivalent(sut1.Data, sut2.Data);
            Assert.Equivalent(sut1.Time, sut2.Time);
            Assert.Equivalent(sut1.Source, sut2.Source);
            Assert.Equivalent(sut1.Id, sut2.Id);
            Assert.Equivalent(sut1.Type, sut2.Type);
        }

        [Fact, Priority(4)]
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

        [Fact, Priority(5)]
        public async Task ReceiveAsync_CreateMemberCommand_All()
        {
            var ct = new CancellationTokenSource(TimeSpan.FromSeconds(90)).Token;
            var realizedCommands = new List<IMessage<ICommand>>();
            var sut1 = Comparer.Query(message => message.Source.StartsWith("urn")).ToList();

            while (realizedCommands.Count < sut1.Count)
            {
                realizedCommands.AddRange(await _queue.ReceiveAsync(o => o.CancellationToken = ct).Select(message =>
                {
                    message.AcknowledgeAsync();
                    return message;
                }).ToListAsync(ct).ConfigureAwait(false));
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
            services.AddMarshaller<NewtonsoftJsonMarshaller>()
                .AddConfiguredOptions<NewtonsoftJsonFormatterOptions>(o => o.Settings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new KebabCaseNamingStrategy() { ProcessDictionaryKeys = false } });
            services.AddAzureCommandQueue(o =>
            {
                o.QueueName = $"newtonsoft-savvyio-commands-{Platform}-{BuildType}";
                o.ConnectionString = Configuration["Azure:Storage:QueueConnectionString"];
                o.PostConfigureClient(client =>
                {
                    client.CreateIfNotExists();
                    client.ClearMessages();
                });
            });
        }
    }
}
