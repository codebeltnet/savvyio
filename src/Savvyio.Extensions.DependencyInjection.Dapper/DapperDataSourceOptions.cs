using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.Dapper;

namespace Savvyio.Extensions.DependencyInjection.Dapper
{
    /// <summary>
    /// Configuration options for <see cref="IDapperDataSource{TMarker}"/>.
    /// </summary>
    /// <seealso cref="DapperDataSourceOptions" />
    /// <seealso cref="IDependencyInjectionMarker{TMarker}" />
    public class DapperDataSourceOptions<TMarker> : DapperDataSourceOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
