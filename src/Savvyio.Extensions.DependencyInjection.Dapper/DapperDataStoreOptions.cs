using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.Dapper;

namespace Savvyio.Extensions.DependencyInjection.Dapper
{
    /// <summary>
    /// Configuration options for <see cref="IDapperDataStore{TMarker}"/>.
    /// </summary>
    
    /// <seealso cref="DapperDataStoreOptions" />
    /// <seealso cref="IDependencyInjectionMarker{TMarker}" />
    public class DapperDataStoreOptions<TMarker> : DapperDataStoreOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
