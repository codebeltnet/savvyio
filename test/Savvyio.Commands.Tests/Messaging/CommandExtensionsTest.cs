using System;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Savvyio.Commands.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Commands.Messaging
{
    public class CommandExtensionsTest : Test
    {
        public CommandExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EncloseToMessage_CreateMemberCommand_ShouldBeWrappedInMessage()
        {
            var sut1 = new CreateMemberCommand("Jane Doe", 21, "jd@office.com");
            var sut2 = "https://fancy.api/members".ToUri();
            var sut3 = sut1.ToMessage(sut2);

            Assert.Equal(sut1.EmailAddress, sut3.Data.EmailAddress);
            Assert.Equal(sut1.Age, sut3.Data.Age);
            Assert.Equal(sut1.Name, sut3.Data.Name);
            Assert.NotNull(sut3.Id);
            Assert.NotNull(sut3.Time);
            Assert.Equal(sut1.GetType().ToFullNameIncludingAssemblyName(), sut3.Type);
            Assert.Equal(sut2.OriginalString, sut3.Source);
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
                           "type": "Savvyio.Commands.Assets.CreateMemberCommand, Savvyio.Commands.Tests",
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
