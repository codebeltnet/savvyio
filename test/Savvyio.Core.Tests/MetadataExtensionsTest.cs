using System;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Reflection;
using Xunit;

namespace Savvyio
{
    public class MetadataExtensionsTest : Test
    {
        public MetadataExtensionsTest(ITestOutputHelper output) : base(output)
        {
            
        }

        private class TestMetadata : IMetadata
        {
            public IMetadataDictionary Metadata { get; } = new MetadataDictionary();
        }

        [Fact]
        public void GetCausationId_ShouldReturnCausationId()
        {
            var request = new TestMetadata();
            request.Metadata[MetadataDictionary.CausationId] = "causation-id";

            var result = request.GetCausationId();

            Assert.Equal("causation-id", result);
        }

        [Fact]
        public void GetCorrelationId_ShouldReturnCorrelationId()
        {
            var request = new TestMetadata();
            request.Metadata[MetadataDictionary.CorrelationId] = "correlation-id";

            var result = request.GetCorrelationId();

            Assert.Equal("correlation-id", result);
        }

        [Fact]
        public void GetRequestId_ShouldReturnRequestId()
        {
            var request = new TestMetadata();
            request.SetRequestId("request-id");

            var result = request.GetRequestId();

            Assert.Equal("request-id", result);
        }

        [Fact]
        public void GetMemberType_ShouldReturnMemberType()
        {
            var request = new TestMetadata();
            request.Metadata[MetadataDictionary.MemberType] = "member-type";

            var result = request.GetMemberType();

            Assert.Equal("member-type", result);
        }

        [Fact]
        public void SetCausationId_ShouldSetCausationId()
        {
            var request = new TestMetadata();

            request.SetCausationId("new-causation-id");

            Assert.Equal("new-causation-id", request.Metadata[MetadataDictionary.CausationId]);
        }

        [Fact]
        public void SetCorrelationId_ShouldSetCorrelationId()
        {
            var request = new TestMetadata();

            request.SetCorrelationId("new-correlation-id");

            Assert.Equal("new-correlation-id", request.Metadata[MetadataDictionary.CorrelationId]);
        }

        [Fact]
        public void SetRequestId_ShouldSetRequestId()
        {
            var request = new TestMetadata();

            request.SetRequestId("new-request-id");

            Assert.Equal("new-request-id", request.Metadata[MetadataDictionary.RequestId]);
        }

        [Fact]
        public void SetEventId_ShouldSetEventId()
        {
            var request = new TestMetadata();

            request.SetEventId("new-event-id");

            Assert.Equal("new-event-id", request.Metadata[MetadataDictionary.EventId]);
        }

        [Fact]
        public void SetTimestamp_ShouldSetTimestamp()
        {
            var request = new TestMetadata();
            var timestamp = DateTime.UtcNow;

            request.SetTimestamp(timestamp);

            Assert.Equal(timestamp, request.Metadata[MetadataDictionary.Timestamp]);
        }

        [Fact]
        public void SetTimestamp_ShouldSetCurrentUtcTimestamp_WhenNoTimestampProvided()
        {
            var request = new TestMetadata();

            request.SetTimestamp();

            Assert.True(request.Metadata.ContainsKey(MetadataDictionary.Timestamp));
            Assert.IsType<DateTime>(request.Metadata[MetadataDictionary.Timestamp]);
        }

        [Fact]
        public void SetMemberType_ShouldSetMemberType()
        {
            var request = new TestMetadata();
            var type = typeof(string);

            request.SetMemberType(type);

            Assert.Equal(type.ToFullNameIncludingAssemblyName(), request.Metadata[MetadataDictionary.MemberType]);
        }

        [Fact]
        public void SaveMetadata_ShouldAddOrUpdateMetadata()
        {
            var request = new TestMetadata();

            request.SaveMetadata("key", "value");

            Assert.Equal("value", request.Metadata["key"]);
        }

        [Fact]
        public void MergeMetadata_ShouldCopyMetadataFromSourceToDestination()
        {
            var source = new TestMetadata();
            source.Metadata["key1"] = "value1";
            var destination = new TestMetadata();

            destination.MergeMetadata(source);

            Assert.Equal("value1", destination.Metadata["key1"]);
        }

        [Fact]
        public void MergeMetadata_ShouldNotOverwriteExistingMetadata()
        {
            var source = new TestMetadata();
            source.Metadata["key1"] = "value1";
            var destination = new TestMetadata();
            destination.Metadata["key1"] = "existing-value";

            destination.MergeMetadata(source);

            Assert.Equal("existing-value", destination.Metadata["key1"]);
        }
    }
}
