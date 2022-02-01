using System;
using Cuemon.Extensions;
using Microsoft.EntityFrameworkCore;
using Savvyio.Domain.EventSourcing;

namespace Savvyio.Extensions.EntityFrameworkCore.Domain.EventSourcing
{
    /// <summary>
    /// Extension methods for the <see cref="ModelBuilder"/> class.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Adds a default implementation of an EF Core compatible Event Sourcing model.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity that implements the <see cref="ITracedAggregateRoot{TKey}"/> interface.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <param name="mb">The <see cref="ModelBuilder"/> to extend.</param>
        /// <param name="setup">The <see cref="EfCoreTracedAggregateEntityOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="mb"/> so that additional configuration calls can be chained.</returns>
        public static ModelBuilder AddEventSourcing<TEntity, TKey>(this ModelBuilder mb, Action<EfCoreTracedAggregateEntityOptions> setup = null) where TEntity : class, ITracedAggregateRoot<TKey>
        {
            var options = setup.Configure();
            return mb.Entity<EfCoreTracedAggregateEntity<TEntity, TKey>>(entity =>
            {
                entity.ToTable(options.TableName);
                entity.HasKey(es => new { es.Id, es.Version });
                entity.Property(es => es.Id)
                    .HasColumnName(options.CompositePrimaryKeyIdColumnName)
                    .HasColumnType(options.CompositePrimaryKeyIdColumnType);
                entity.Property(es => es.Version)
                    .HasColumnName(options.CompositePrimaryKeyVersionColumnName)
                    .HasColumnType(options.CompositePrimaryKeyVersionColumnType);
                entity.Property(es => es.Timestamp)
                    .HasColumnName(options.TimestampColumnName)
                    .HasColumnType(options.TimestampColumnType);
                entity.Property(es => es.Type)
                    .HasColumnName(options.TypeColumnName)
                    .HasColumnType(options.TypeColumnType);
                entity.Property(es => es.Payload)
                    .HasColumnName(options.PayloadColumnName)
                    .HasColumnType(options.PayloadColumnType);
            });
        }
    }
}
