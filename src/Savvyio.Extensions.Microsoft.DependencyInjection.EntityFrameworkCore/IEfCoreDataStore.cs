using Savvyio.Extensions.Microsoft.DependencyInjection.Storage;
using Savvyio.Extensions.Microsoft.EntityFrameworkCore;

namespace Savvyio.Extensions.Microsoft.DependencyInjection.EntityFrameworkCore
{
    /// <summary>
    /// Defines a generic way to support multiple implementations that does the actual I/O communication towards a data store using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IEfCoreDataStore" />
    /// <seealso cref="IDataStore{TMarker}"/>
    public interface IEfCoreDataStore<TMarker> : IEfCoreDataStore, IDataStore<TMarker>
    {
    }
}
