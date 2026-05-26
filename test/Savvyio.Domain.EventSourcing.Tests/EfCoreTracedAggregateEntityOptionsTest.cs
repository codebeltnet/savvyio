using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.EFCore.Domain.EventSourcing
{
    public class EfCoreTracedAggregateEntityOptionsTest : Test
    {
        public EfCoreTracedAggregateEntityOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EfCoreTracedAggregateEntityOptions_Ensure_Initialization_Defaults()
        {
            var sut = new EfCoreTracedAggregateEntityOptions();

            Assert.Equal("DomainEvents", sut.TableName);
            Assert.Equal("id", sut.CompositePrimaryKeyIdColumnName);
            Assert.Equal("uniqueidentifier", sut.CompositePrimaryKeyIdColumnType);
            Assert.Equal("version", sut.CompositePrimaryKeyVersionColumnName);
            Assert.Equal("int", sut.CompositePrimaryKeyVersionColumnType);
            Assert.Equal("timestamp", sut.TimestampColumnName);
            Assert.Equal("datetime", sut.TimestampColumnType);
            Assert.Equal("clrtype", sut.TypeColumnName);
            Assert.Equal("varchar(1024)", sut.TypeColumnType);
            Assert.Equal("payload", sut.PayloadColumnName);
            Assert.Equal("varchar(max)", sut.PayloadColumnType);
        }

        [Fact]
        public void EfCoreTracedAggregateEntityOptions_ShouldAcceptCustomValues()
        {
            var sut = new EfCoreTracedAggregateEntityOptions
            {
                TableName = "TraceEvents",
                CompositePrimaryKeyIdColumnName = "aggregate_id",
                CompositePrimaryKeyIdColumnType = "uuid",
                CompositePrimaryKeyVersionColumnName = "aggregate_version",
                CompositePrimaryKeyVersionColumnType = "bigint",
                TimestampColumnName = "recorded_at",
                TimestampColumnType = "datetime2",
                TypeColumnName = "aggregate_type",
                TypeColumnType = "nvarchar(512)",
                PayloadColumnName = "body",
                PayloadColumnType = "varbinary(max)"
            };

            Assert.Equal("TraceEvents", sut.TableName);
            Assert.Equal("aggregate_id", sut.CompositePrimaryKeyIdColumnName);
            Assert.Equal("uuid", sut.CompositePrimaryKeyIdColumnType);
            Assert.Equal("aggregate_version", sut.CompositePrimaryKeyVersionColumnName);
            Assert.Equal("bigint", sut.CompositePrimaryKeyVersionColumnType);
            Assert.Equal("recorded_at", sut.TimestampColumnName);
            Assert.Equal("datetime2", sut.TimestampColumnType);
            Assert.Equal("aggregate_type", sut.TypeColumnName);
            Assert.Equal("nvarchar(512)", sut.TypeColumnType);
            Assert.Equal("body", sut.PayloadColumnName);
            Assert.Equal("varbinary(max)", sut.PayloadColumnType);
        }
    }
}
