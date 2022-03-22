using System;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;
using Savvyio.Domain.EventSourcing;

namespace Savvyio.Extensions.EFCore.Domain.EventSourcing
{
    /// <summary>
    /// Extension methods for the <see cref="EfCoreTracedAggregateEntity{TEntity,TKey}"/> class.
    /// </summary>
    public static class EfCoreTracedAggregateEntityExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="entity"/> into an implementation of <see cref="ITracedDomainEvent"/> interface equivalent to <paramref name="tracedDomainEventType"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity that implements the <see cref="ITracedAggregateRoot{TKey}"/> interface.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tracedDomainEventType">The target.</param>
        /// <returns>ITracedDomainEvent.</returns>
        public static ITracedDomainEvent ToTracedDomainEvent<TEntity, TKey>(this EfCoreTracedAggregateEntity<TEntity, TKey> entity, Type tracedDomainEventType) where TEntity : class, ITracedAggregateRoot<TKey>
        {
            var formatter = new JsonFormatter(o =>
            {
                o.Settings.Converters.Add(DynamicJsonConverter.Create<IMetadataDictionary>(null, MetadataReader));
            });
            return (ITracedDomainEvent)formatter.Deserialize(entity.Payload.ToStream(), tracedDomainEventType);
        }

        private static IMetadataDictionary MetadataReader(JsonReader reader, Type type, IMetadataDictionary dictionary, JsonSerializer serializer)
        {
            var md = new MetadataDictionary();
            var result = JData.ReadAll(reader);
            foreach (var entry in result)
            {
               md.Add(entry.PropertyName, entry.Value);
            }
            return md;
        }
    }
}
