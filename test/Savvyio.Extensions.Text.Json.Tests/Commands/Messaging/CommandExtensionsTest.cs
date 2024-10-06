using System;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Text.Json.Commands.Messaging
{
    public class CommandExtensionsTest : Test
    {
        public CommandExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToMessage_ShouldSerializeAndDeserialize_CreateMemberCommand_UsingInterface()
        {
            var utcNow = DateTime.UtcNow;
            var sut1 = new CreateMemberCommand("Jane Doe", 21, "jd@office.com").SetCorrelationId("3eefdef050c340bfba100bd49c58c181");
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), nameof(CreateMemberCommand), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utcNow;
            });

            var json = new JsonMarshaller().Serialize(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);
            
            var sut4 = new JsonMarshaller().Deserialize<IMessage<CreateMemberCommand>>(json);

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
