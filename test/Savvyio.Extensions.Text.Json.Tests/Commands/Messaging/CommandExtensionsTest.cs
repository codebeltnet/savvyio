using System;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Savvyio.Commands.Messaging;
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
        public void EncloseToMessage_ShouldSerialize_WithoutSignature()
        {
            var utcNow = DateTime.UtcNow;
            var sut1 = new CreateMemberCommand("Jane Doe", 21, "jd@office.com").SetCorrelationId("3eefdef050c340bfba100bd49c58c181");
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utcNow;
            });
            var json = JsonFormatter.SerializeObject(sut2);
            var jsonString = json.ToEncodedString();

            TestOutput.WriteLine(jsonString);

            Assert.Equal($$"""
                         {
                           "id": "2d4030d32a254ee8a27046e5bafe696a",
                           "source": "https://fancy.api/members",
                           "type": "Savvyio.Assets.Commands.CreateMemberCommand, Savvyio.Assets.Tests",
                           "time": "{{utcNow:O}}",
                           "data": {
                             "name": "Jane Doe",
                             "age": 21,
                             "emailAddress": "jd@office.com",
                             "metadata": {
                               "correlationId": "3eefdef050c340bfba100bd49c58c181"
                             }
                           }
                         }
                         """.ReplaceLineEndings(), jsonString);
        }
    }
}
