namespace Savvyio
{
    /// <summary>
    /// Defines a generic identity typically associated with a storage such as a database.
    /// </summary>
    /// <typeparam name="TKey">The type of the identifier.</typeparam>
    public interface IIdentity<out TKey>
    {
        /// <summary>
        /// Gets the value of the identifier.
        /// </summary>
        /// <value>The value of the identifier.</value>
        TKey Id { get; }
    }
}
