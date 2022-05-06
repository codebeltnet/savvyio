using Cuemon.Threading;

namespace Savvyio.Data
{
    /// <summary>
    /// Defines a generic way of abstracting persistent data access objects (CRUD).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of the options associated with this DTO.</typeparam>
    /// <seealso cref="IWritableDataAccessObject{T,TOptions}"/>
    /// <seealso cref="IReadableDataAccessObject{T,TOptions}"/>
    /// <seealso cref="IDeletableDataAccessObject{T,TOptions}"/>
    public interface IPersistentDataAccessObject<T, TOptions> : IWritableDataAccessObject<T, TOptions>, IReadableDataAccessObject<T, TOptions>, IDeletableDataAccessObject<T, TOptions> 
        where T : class 
        where TOptions : AsyncOptions, new()
    {
    }
}
