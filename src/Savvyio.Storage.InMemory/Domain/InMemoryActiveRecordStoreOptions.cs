using System;
using System.Collections.Generic;

namespace Savvyio.Domain
{
    public class InMemoryActiveRecordStoreOptions<TAggregate, TKey> where TAggregate : class, IAggregateRoot<IDomainEvent, TKey>
    {
        public InMemoryActiveRecordStoreOptions()
        {
        }

        
        public Func<IEnumerable<TAggregate>, TKey> IdentityProvider { get; set; }

        public bool HasIdentityProvider => IdentityProvider != null;
    }
}
