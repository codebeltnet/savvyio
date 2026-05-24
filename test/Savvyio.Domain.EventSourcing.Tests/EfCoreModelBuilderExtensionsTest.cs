using System;
using Codebelt.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Savvyio.Assets.Domain.EventSourcing;
using Savvyio.Extensions.EFCore;
using Xunit;

namespace Savvyio.Extensions.EFCore.Domain.EventSourcing
{
    public class EfCoreModelBuilderExtensionsTest : Test
    {
        public EfCoreModelBuilderExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ModelBuilderExtensions_ShouldApplyConfiguredEventSourcingSchema()
        {
            var source = new EfCoreDataSource(new EfCoreDataSourceOptions
            {
                ContextConfigurator = b => b.UseInMemoryDatabase("event-sourcing-" + Guid.NewGuid()),
                ModelConstructor = mb => mb.AddEventSourcing<TracedAccount, Guid>(o =>
                {
                    o.TableName = "TraceEvents";
                    o.CompositePrimaryKeyIdColumnName = "aggregate_id";
                    o.CompositePrimaryKeyVersionColumnName = "aggregate_version";
                    o.TimestampColumnName = "recorded_at";
                    o.TypeColumnName = "aggregate_type";
                    o.PayloadColumnName = "body";
                })
            });

            var schema = source.DbContext.Model.ToDebugString(MetadataDebugStringOptions.LongDefault);

            Assert.Contains("Relational:TableName: TraceEvents", schema);
            Assert.Contains("Relational:ColumnName: aggregate_id", schema);
            Assert.Contains("Relational:ColumnName: aggregate_version", schema);
            Assert.Contains("Relational:ColumnName: recorded_at", schema);
            Assert.Contains("Relational:ColumnName: aggregate_type", schema);
            Assert.Contains("Relational:ColumnName: body", schema);
        }
    }
}
