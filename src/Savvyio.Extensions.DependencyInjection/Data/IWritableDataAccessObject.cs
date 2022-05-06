using Cuemon.Threading;
using Savvyio.Data;

namespace Savvyio.Extensions.DependencyInjection.Data
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of writable data access objects (CrUd).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of the options associated with this DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDataAccessObject{T,TOptions,TMarker}"/>
    /// <seealso cref="IWritableDataAccessObject{T,TOptions}"/>
    public interface IWritableDataAccessObject<in T, TOptions, TMarker> : IDataAccessObject<T, TOptions, TMarker>, IWritableDataAccessObject<T, TOptions> 
        where T : class 
        where TOptions : AsyncOptions, new()
    {
    }
}
