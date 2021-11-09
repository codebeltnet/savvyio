using System.Collections.Generic;

namespace Savvyio.Domain
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        protected Entity()
        {
        }

        protected Entity(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; protected set; }

        public virtual bool IsTransient => EqualityComparer<TKey>.Default.Equals(Id, default);
    }
}
