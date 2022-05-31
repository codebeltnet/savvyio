using Cuemon.Extensions.DependencyInjection;
using Savvyio.Data;

namespace Savvyio.Extensions.DependencyInjection.Data
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of data persistence based on the Data Access Object pattern aka DAO.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDataStore{T,TMarker}"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}" />
    public interface IDataStore<in T, TMarker> : IDataStore<T>, IDependencyInjectionMarker<TMarker> where T : class
    {
    }
}
