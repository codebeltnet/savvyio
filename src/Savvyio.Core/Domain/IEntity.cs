namespace Savvyio.Domain
{
    /// <summary>
    /// Defines an Entity as specified in Domain Driven Design.
    /// </summary>
    /// <typeparam name="TKey">The type of the identifier.</typeparam>
    /// <seealso cref="IIdentity{TKey}" />
    public interface IEntity<out TKey> : IIdentity<TKey>
    {
    }
}
