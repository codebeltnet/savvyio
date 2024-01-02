using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Commands.Messaging.Cryptography;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Newtonsoft.Json.Commands.Messaging.Cryptography
{
    public class CommandExtensionsTest : Test
    {
        public CommandExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EncloseToSignedMessage_ShouldSerializeAndDeserialize_WithSignature()
        {
            var utc = DateTime.Parse("2023-11-16T23:24:17.8414532Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var sut1 = new CreateMemberCommand("Jane Doe", 21, "jd@office.com").SetCorrelationId("3eefdef050c340bfba100bd49c58c181");
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), nameof(CreateMemberCommand), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utc;
            });

            var sut3 = sut2.Sign(new NewtonsoftJsonMarshaller(), o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });

            var json = new NewtonsoftJsonMarshaller().Serialize(sut3);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var sut4 = new NewtonsoftJsonMarshaller().Deserialize<IMessage<CreateMemberCommand>>(json);

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
                               "signature": "194374d817b2b3f9f8ea1ce6557bbb4a96200de2a759270df96d2a20cb28cdae"
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
                               "signature": "8d15d927f35b57f8e89392952e43e5cca32d240902c01bc00baaa819f71c5a5a"
                             }
                             """, jsonString);
            }

        }
    }
}
