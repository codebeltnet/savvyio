using Cuemon.Extensions.DependencyInjection;

namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of persistent data access based on the Data Access Object pattern.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}" />
    /// <seealso cref="IDataAccessObject{T}"/>
    public interface IDataAccessObject<in T, TMarker> : IDataAccessObject<T>, IDependencyInjectionMarker<TMarker> where T : class
    {
    }
}
