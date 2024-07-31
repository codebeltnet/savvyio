using System;
using Cuemon.Extensions.IO;
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
        /// <param name="entity">The entity to convert.</param>
        /// <param name="tracedDomainEventType">The originating type of <see cref="ITracedDomainEvent"/>.</param>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting the specified <paramref name="tracedDomainEventType"/> into a deserialized version of <see cref="ITracedDomainEvent"/>.</param>
        /// <returns>A new instance of an <see cref="ITracedDomainEvent"/> implementation.</returns>
        public static ITracedDomainEvent ToTracedDomainEvent<TEntity, TKey>(this EfCoreTracedAggregateEntity<TEntity, TKey> entity, Type tracedDomainEventType, IMarshaller marshaller) where TEntity : class, ITracedAggregateRoot<TKey>
        {
            return (ITracedDomainEvent)marshaller.Deserialize(entity.Payload.ToStream(), tracedDomainEventType);
        }
    }
}
