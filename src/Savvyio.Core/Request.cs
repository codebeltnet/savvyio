namespace Savvyio
{
    /// <summary>
    /// Represents the base class from which all implementations of <see cref="IRequest"/> should derive.
    /// </summary>
    /// <seealso cref="IMetadata" />
    /// <seealso cref="IRequest" />
    public abstract class Request : IRequest, IMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Request"/> class.
        /// </summary>
        protected Request()
        {
        }

        /// <summary>
        /// Gets the associated metadata of this model.
        /// </summary>
        /// <value>The associated metadata of this model.</value>
        public IMetadataDictionary Metadata { get; } = new MetadataDictionary();
    }
}
