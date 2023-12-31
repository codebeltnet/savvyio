using System;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Newtonsoft.Json.Commands.Messaging
{
    public class CommandExtensionsTest : Test
    {
        public CommandExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EncloseToMessage_ShouldSerialize_WithoutSignature()
        {
            var utcNow = DateTime.UtcNow;
            var sut1 = new CreateMemberCommand("Jane Doe", 21, "jd@office.com").SetCorrelationId("3eefdef050c340bfba100bd49c58c181");
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), nameof(CreateMemberCommand), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utcNow;
            });

            var json = NewtonsoftJsonFormatter.SerializeObject(sut2, o => o.Settings.DateFormatString = "O");
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var sut4 = NewtonsoftJsonFormatter.DeserializeObject<IMessage<CreateMemberCommand>>(json, o =>
            {
                o.Settings.Converters
                    .AddMessageConverter()
                    .AddMetadataDictionaryConverter();
            });

            Assert.Equivalent(sut2, sut4, true);

            Assert.Equal($$"""
                         {
                           "id": "2d4030d32a254ee8a27046e5bafe696a",
                           "source": "https://fancy.api/members",
                           "type": "CreateMemberCommand",
                           "time": "{{utcNow:O}}",
                           "data": {
                             "name": "Jane Doe",
                             "age": 21,
                             "emailAddress": "jd@office.com",
                             "metadata": {
                               "memberType": "Savvyio.Assets.Commands.CreateMemberCommand, Savvyio.Assets.Tests",
                               "correlationId": "3eefdef050c340bfba100bd49c58c181"
                             }
                           }
                         }
                         """.ReplaceLineEndings(), jsonString);
        }
    }
}
