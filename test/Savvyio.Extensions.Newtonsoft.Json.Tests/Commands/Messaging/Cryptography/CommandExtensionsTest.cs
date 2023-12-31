using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
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
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utc;
            });

            var sut3 = sut2.Sign(new NewtonsoftJsonSerializerContext(), o =>
            {
                o.SignatureSecret = new byte[] { 1, 2, 3 };
            });

            var json = NewtonsoftJsonFormatter.SerializeObject(sut3);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var sut4 = NewtonsoftJsonFormatter.DeserializeObject<IMessage<CreateMemberCommand>>(json, o =>
            {
                o.Settings.Converters
                    .AddMessageConverter()
                    .AddMetadataDictionaryConverter();
            });

            Assert.Equivalent(sut2, sut4, true);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Assert.Equal("""
                             {
                               "id": "2d4030d32a254ee8a27046e5bafe696a",
                               "source": "https://fancy.api/members",
                               "type": "Savvyio.Assets.Commands.CreateMemberCommand, Savvyio.Assets.Tests",
                               "time": "2023-11-16T23:24:17.8414532Z",
                               "data": {
                                 "name": "Jane Doe",
                                 "age": 21,
                                 "emailAddress": "jd@office.com",
                                 "metadata": {
                                   "correlationId": "3eefdef050c340bfba100bd49c58c181"
                                 }
                               },
                               "signature": "869a217f20a7688c24cd11c7e2ffbdfd07be460bb512e4f0963b0defd07b104f"
                             }
                             """.ReplaceLineEndings(), jsonString);
            }
            else
            {
                Assert.Equal("""
                             {
                               "id": "2d4030d32a254ee8a27046e5bafe696a",
                               "source": "https://fancy.api/members",
                               "type": "Savvyio.Assets.Commands.CreateMemberCommand, Savvyio.Assets.Tests",
                               "time": "2023-11-16T23:24:17.8414532Z",
                               "data": {
                                 "name": "Jane Doe",
                                 "age": 21,
                                 "emailAddress": "jd@office.com",
                                 "metadata": {
                                   "correlationId": "3eefdef050c340bfba100bd49c58c181"
                                 }
                               },
                               "signature": "41b0f82f5ed643c720e3ea177c194b62e37afc43177a2db35acb19a7aded0e7b"
                             }
                             """, jsonString);
            }

        }
    }
}
