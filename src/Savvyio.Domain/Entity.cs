using System.Collections.Generic;

namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a way to cover the pattern of an Enetiy as specified in Domain Driven Design. This is an abstract class.
    /// </summary>
    /// <typeparam name="TKey">The type of the identifier.</typeparam>
    /// <seealso cref="IEntity{TKey}" />
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{TKey}"/> class.
        /// </summary>
        protected Entity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{TKey}"/> class.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        protected Entity(TKey id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets the value of the identifier.
        /// </summary>
        /// <value>The value of the identifier.</value>
        public TKey Id { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this instance is transient.
        /// </summary>
        /// <value><c>true</c> if this instance is transient; otherwise, <c>false</c>.</value>
        public virtual bool IsTransient => EqualityComparer<TKey>.Default.Equals(Id, default);
    }
}
