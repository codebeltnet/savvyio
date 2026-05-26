using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Savvyio.Assets.Domain.Events;
using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.Newtonsoft.Json;
using Xunit;

namespace Savvyio.Extensions.EFCore.Domain.EventSourcing
{
    public class TracedDomainEventExtensionsTest : Test
    {
        public TracedDomainEventExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TracedDomainEventExtensions_ShouldConvertDomainEventToByteArray()
        {
            var domainEvent = new TracedAccountEmailAddressChanged("test@unit.test");
            var expected = NewtonsoftJsonMarshaller.Default.Serialize(domainEvent, typeof(ITracedDomainEvent)).ToByteArray();

            var sut = domainEvent.ToByteArray(NewtonsoftJsonMarshaller.Default);

            Assert.NotEmpty(sut);
            Assert.Equal(expected, sut);
        }
    }
}
