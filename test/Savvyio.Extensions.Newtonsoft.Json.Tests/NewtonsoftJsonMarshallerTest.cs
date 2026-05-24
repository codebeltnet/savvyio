using System;
using System.IO;
using Codebelt.Extensions.Xunit;
using Newtonsoft.Json;
using Savvyio.Assets.Commands;
using Xunit;

namespace Savvyio.Extensions.Newtonsoft.Json
{
    public class NewtonsoftJsonMarshallerTest : Test
    {
        public NewtonsoftJsonMarshallerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void NewtonsoftJsonMarshaller_Default_ShouldUseCompactFormatting()
        {
            using var stream = NewtonsoftJsonMarshaller.Default.Serialize(new CreateMemberCommand("Jane Doe", 21, "jd@office.com"));
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            Assert.DoesNotContain(Environment.NewLine, json);
        }

        [Fact]
        public void NewtonsoftJsonMarshaller_Create_ShouldHonorConfiguredFormatting()
        {
            var sut = NewtonsoftJsonMarshaller.Create(o => o.Settings.Formatting = Formatting.Indented);

            using var stream = sut.Serialize(new CreateMemberCommand("Jane Doe", 21, "jd@office.com"));
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            Assert.Contains(Environment.NewLine, json);
        }

        [Fact]
        public void NewtonsoftJsonMarshaller_ShouldRoundtripUsingTypeBasedMethods()
        {
            var marshaller = new NewtonsoftJsonMarshaller();
            using var stream = marshaller.Serialize(new CreateMemberCommand("Jane Doe", 21, "jd@office.com"), typeof(CreateMemberCommand));

            var sut = marshaller.Deserialize(stream, typeof(CreateMemberCommand));

            Assert.IsType<CreateMemberCommand>(sut);
            Assert.Equal("Jane Doe", ((CreateMemberCommand)sut).Name);
        }
    }
}
