using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.EFCore;

namespace Savvyio.Extensions.DependencyInjection.EFCore
{
    /// <summary>
    /// Defines a generic way to support multiple implementations that does the actual I/O communication with a source of data using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IEfCoreDataSource" />
    /// <seealso cref="IDataSource{TMarker}"/>
    public interface IEfCoreDataSource<TMarker> : IEfCoreDataSource, IDataSource<TMarker>, IUnitOfWork<TMarker>
    {
    }
}
