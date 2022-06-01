using Savvyio.Extensions.Dapper;

namespace Savvyio.Extensions.DependencyInjection.Dapper
{
    /// <summary>
    /// Defines a generic way to support multiple implementations that does the actual I/O communication with a source of data optimized for Dapper.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDapperDataSource" />
    /// <seealso cref="IDataSource{TMarker}"/>
    public interface IDapperDataSource<TMarker> : IDapperDataSource, IDataSource<TMarker>
    {
    }
}
