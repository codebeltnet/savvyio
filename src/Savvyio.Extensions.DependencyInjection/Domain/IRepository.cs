using Cuemon.Extensions.DependencyInjection;
using Savvyio.Domain;

namespace Savvyio.Extensions.DependencyInjection.Domain
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of persistent data access based on the Repository pattern.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}" />
    /// <seealso cref="IRepository{TEntity,TKey}"/>
    public interface IRepository<in TEntity, TKey, TMarker> : IRepository<TEntity, TKey>, IDependencyInjectionMarker<TMarker> where TEntity : class, IIdentity<TKey>
    {
    }
}
