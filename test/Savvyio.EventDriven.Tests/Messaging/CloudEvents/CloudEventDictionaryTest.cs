using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.EventDriven;
using Xunit;

namespace Savvyio.EventDriven.Messaging.CloudEvents
{
    public class CloudEventDictionaryTest : Test
    {
        public CloudEventDictionaryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void IDictionaryMembers_ShouldManageExtensionAttributes()
        {
            var sut = CreateCloudEvent();
            IDictionary<string, object> dictionary = sut;
            ICollection<KeyValuePair<string, object>> collection = dictionary;

            sut.Add("TenantId", 42);
            sut["TraceId"] = "abc123";

            Assert.True(sut.ContainsKey("tenantid"));
            Assert.True(sut.ContainsKey("traceid"));
            Assert.Equal(42, sut["tenantid"]);
            Assert.True(sut.TryGetValue("traceid", out var traceId));
            Assert.Equal("abc123", traceId);
            Assert.Contains("tenantid", dictionary.Keys);
            Assert.Contains("traceid", dictionary.Keys);
            Assert.Contains(42, dictionary.Values);
            Assert.Contains("abc123", dictionary.Values);
            Assert.True(collection.Contains(new KeyValuePair<string, object>("tenantid", 42)));
            var copy = new KeyValuePair<string, object>[collection.Count];
            collection.CopyTo(copy, 0);
            Assert.Contains(copy, kvp => kvp.Key == "tenantid" && Equals(kvp.Value, 42));
            Assert.False(collection.IsReadOnly);
            Assert.True(sut.Remove("traceid"));
            Assert.False(sut.ContainsKey("traceid"));
        }

        [Fact]
        public void EnumeratorsAndClear_ShouldExposeAndResetExtensionAttributes()
        {
            var sut = CreateCloudEvent();
            IDictionary<string, object> dictionary = sut;
            ICollection<KeyValuePair<string, object>> collection = dictionary;

            collection.Add(new KeyValuePair<string, object>("CorrelationId", "corr-1"));
            collection.Add(new KeyValuePair<string, object>("TenantId", 7));

            var genericItems = dictionary.ToList();
            var nongenericItems = ((IEnumerable)sut).Cast<KeyValuePair<string, object>>().ToList();

            Assert.Equal(2, genericItems.Count);
            Assert.Equal(genericItems, nongenericItems);
            Assert.True(collection.Remove(new KeyValuePair<string, object>("tenantid", 7)));
            Assert.Single(dictionary);

            sut.Clear();

            Assert.Empty(dictionary);
        }

        private static CloudEvent<MemberCreated> CreateCloudEvent()
        {
            var message = new MemberCreated("Jane Doe", "jd@office.com").ToMessage("https://savvyio.net/members".ToUri(), nameof(MemberCreated), o =>
            {
                o.MessageId = "message-id";
                o.Time = new System.DateTime(2024, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            });

            return (CloudEvent<MemberCreated>)message.ToCloudEvent();
        }
    }
}
