namespace Savvyio
{
    /// <summary>
    /// Defines a generic way to associate metadata with any type of object.
    /// </summary>
    public interface IMetadata
    {
        /// <summary>
        /// Gets the associated metadata of this instance.
        /// </summary>
        /// <value>The associated metadata of this instance.</value>
        public IMetadataDictionary Metadata { get; }
    }
}
