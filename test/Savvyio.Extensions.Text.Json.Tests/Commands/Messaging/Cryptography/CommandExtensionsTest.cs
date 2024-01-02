using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Commands.Messaging.Cryptography;
using Savvyio.Messaging.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Text.Json.Commands.Messaging.Cryptography
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
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), nameof(CreateMemberCommand), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utc;
            }).Sign(new JsonMarshaller(), o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });

            var json = new JsonMarshaller().Serialize(sut2);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var sut4 = new JsonMarshaller().Deserialize<ISignedMessage<CreateMemberCommand>>(json);

            Assert.Equivalent(sut2, sut4, true);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Assert.Equal("""
                             {
                               "id": "2d4030d32a254ee8a27046e5bafe696a",
                               "source": "https://fancy.api/members",
                               "type": "CreateMemberCommand",
                               "time": "2023-11-16T23:24:17.8414532Z",
                               "data": {
                                 "name": "Jane Doe",
                                 "age": 21,
                                 "emailAddress": "jd@office.com",
                                 "metadata": {
                                   "memberType": "Savvyio.Assets.Commands.CreateMemberCommand, Savvyio.Assets.Tests",
                                   "correlationId": "3eefdef050c340bfba100bd49c58c181"
                                 }
                               },
                               "signature": "ade785c5110355c7ee39147c13dd4e1b8a162d905150aa53aa4b47eeeb663aee"
                             }
                             """.ReplaceLineEndings(), jsonString);
            }
            else
            {
                Assert.Equal("""
                             {
                               "id": "2d4030d32a254ee8a27046e5bafe696a",
                               "source": "https://fancy.api/members",
                               "type": "CreateMemberCommand",
                               "time": "2023-11-16T23:24:17.8414532Z",
                               "data": {
                                 "name": "Jane Doe",
                                 "age": 21,
                                 "emailAddress": "jd@office.com",
                                 "metadata": {
                                   "memberType": "Savvyio.Assets.Commands.CreateMemberCommand, Savvyio.Assets.Tests",
                                   "correlationId": "3eefdef050c340bfba100bd49c58c181"
                                 }
                               },
                               "signature": "c5811da11b12d000fa72f60b626d88c7544e95c121df840f9d1bb16ded7d2eea"
                             }
                             """, jsonString);
            }

        }
    }
}
