using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.Events;
using Savvyio.Queries;

namespace Savvyio
{
    /// <summary>
    /// Represents the base class from which all implementations of <see cref="ICommand"/>, <see cref="IDomainEvent"/>, <see cref="IIntegrationEvent"/> and <see cref="IQuery{TResult}"/> should derive.
    /// </summary>
    /// <seealso cref="IMetadata" />
    public abstract class Model : IMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        protected Model()
        {
        }

        /// <summary>
        /// Gets the associated metadata of this model.
        /// </summary>
        /// <value>The associated metadata of this model.</value>
        public IMetadataDictionary Metadata { get; } = new MetadataDictionary();
    }
}
