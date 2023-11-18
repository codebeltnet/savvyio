using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Savvyio.Commands.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Commands.Messaging.Cryptography
{
    public class CommandExtensionsTest : Test
    {
        public CommandExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EncloseToSignedMessage_ShouldSerialize_WithSignature()
        {
            var utc = DateTime.Parse("2023-11-16T23:24:17.8414532Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var sut1 = new CreateMemberCommand("Jane Doe", 21, "jd@office.com").SetCorrelationId("3eefdef050c340bfba100bd49c58c181");
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utc;
            }).Sign(o =>
            {
                o.SerializerFactory = message => JsonFormatter.SerializeObject(message);
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });
            var json = JsonFormatter.SerializeObject(sut2);
            var jsonString = json.ToEncodedString();
            TestOutput.WriteLine(jsonString);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Assert.Equal("""
                             {
                               "id": "2d4030d32a254ee8a27046e5bafe696a",
                               "source": "https://fancy.api/members",
                               "type": "Savvyio.Commands.Assets.CreateMemberCommand, Savvyio.Commands.Tests",
                               "time": "2023-11-16T23:24:17.8414532Z",
                               "data": {
                                 "name": "Jane Doe",
                                 "age": 21,
                                 "emailAddress": "jd@office.com",
                                 "metadata": {
                                   "correlationId": "3eefdef050c340bfba100bd49c58c181"
                                 }
                               },
                               "signature": "aca8389dea1157c81825b63b16191da0f5a0097263ab607a98ebea248a1ca4a9"
                             }
                             """.ReplaceLineEndings(), jsonString);
            }
            else
            {
                Assert.Equal("""
                             {
                               "id": "2d4030d32a254ee8a27046e5bafe696a",
                               "source": "https://fancy.api/members",
                               "type": "Savvyio.Commands.Assets.CreateMemberCommand, Savvyio.Commands.Tests",
                               "time": "2023-11-16T23:24:17.8414532Z",
                               "data": {
                                 "name": "Jane Doe",
                                 "age": 21,
                                 "emailAddress": "jd@office.com",
                                 "metadata": {
                                   "correlationId": "3eefdef050c340bfba100bd49c58c181"
                                 }
                               },
                               "signature": "c25fd37ae917ddcd3eddab395ddc8bc0ebe1954be185b4291d09af0abaded935"
                             }
                             """, jsonString);
            }

        }
    }
}
