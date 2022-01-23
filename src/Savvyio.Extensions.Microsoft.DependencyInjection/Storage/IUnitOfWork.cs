using Cuemon.Extensions.DependencyInjection;
using Savvyio.Storage;

namespace Savvyio.Extensions.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations that bundles transactions from multiple <see cref="IPersistentRepository{TEntity,TKey}"/> calls into a single unit.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this UoW represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}" />
    /// <seealso cref="IUnitOfWork" />
    public interface IUnitOfWork<TMarker> : IUnitOfWork, IDependencyInjectionMarker<TMarker>
    {
    }
}
