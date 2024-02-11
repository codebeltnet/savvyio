using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions.Collections.Generic;
using Savvyio.Domain;
using Savvyio.Domain.EventSourcing;

namespace Savvyio.Extensions.EFCore.Domain.EventSourcing
{
    /// <summary>
    /// Provides a generic way for EF Core to surrogate and support an implementation of <see cref="ITracedAggregateRoot{TKey}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that implements the <see cref="ITracedAggregateRoot{TKey}"/> interface.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="ITracedAggregateRoot{TKey}" />
    public class EfCoreTracedAggregateEntity<TEntity, TKey> : ITracedAggregateRoot<TKey> where TEntity : class, IEntity<TKey>, ITracedAggregateRoot<TKey>
    {
        private readonly IMetadataDictionary _metadata;
        private readonly IReadOnlyList<ITracedDomainEvent> _events;
        private readonly TKey _id;
        private readonly DateTime _timestamp;
        private readonly long _version;
        private readonly string _type;
        private readonly byte[] _payload;

        EfCoreTracedAggregateEntity() // efcore
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreTracedAggregateEntity{TEntity,TKey}"/> class.
        /// </summary>
        /// <param name="aggregate">The traced aggregate to convert into this EF Core compatible instance.</param>
        /// <param name="domainEvent">The traced domain event to convert into this EF Core compatible instance.</param>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting <see cref="ITracedDomainEvent"/> into a serialized format.</param>
        public EfCoreTracedAggregateEntity(TEntity aggregate, ITracedDomainEvent domainEvent, IMarshaller marshaller)
        {
            _id = aggregate.Id;
            _version = domainEvent.GetAggregateVersion();
            _type = domainEvent.GetMemberType();
            _timestamp = domainEvent.GetTimestamp();
            _payload = domainEvent.ToByteArray(marshaller);
            _metadata = aggregate.Metadata;
            _events = domainEvent.Yield().ToList();
        }

        /// <summary>
        /// Gets the value of the identifier associated with an aggregate.
        /// </summary>
        /// <value>The value of the identifier associated with an aggregate.</value>
        public TKey Id => _id;

        /// <summary>
        /// Gets the timestamp of the traced domain event.
        /// </summary>
        /// <value>The timestamp of the traced domain event.</value>
        public DateTime Timestamp => _timestamp;

        /// <summary>
        /// Gets the version of the aggregate from the traced domain event.
        /// </summary>
        /// <value>The version of the aggregate from the traced domain event.</value>
        public long Version => _version;

        /// <summary>
        /// Gets the CLR type of the traced domain event.
        /// </summary>
        /// <value>The CLR type of the traced domain event.</value>
        public string Type => _type;

        /// <summary>
        /// Gets the payload of the traced domain event.
        /// </summary>
        /// <value>The payload of the traced domain event.</value>
        public byte[] Payload => _payload;

        IMetadataDictionary IMetadata.Metadata => _metadata;

        IReadOnlyList<ITracedDomainEvent> IAggregateRoot<ITracedDomainEvent>.Events => _events;

        void IAggregateRoot<ITracedDomainEvent>.RemoveAllEvents()
        {
            // empty as we do not want to clear events due to dehydrate/rehydrate
        }
    }
}
