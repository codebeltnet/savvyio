namespace Savvyio
{
    /// <summary>
    /// Defines an identity typically associated with a storage such as a database.
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
