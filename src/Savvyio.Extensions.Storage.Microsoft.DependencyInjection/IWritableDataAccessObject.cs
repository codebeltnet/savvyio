namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of writable data access objects (CrUd).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IWritableDataAccessObject{T}"/>
    public interface IWritableDataAccessObject<in T, TMarker> : IDataAccessObject<T, TMarker>, IWritableDataAccessObject<T> where T : class
    {
    }
}
