using System.Data;

namespace Savvyio.Extensions.Dapper
{
    /// <summary>
    /// Defines a generic way to support the actual I/O communication with a source of data - tailored to Dapper.
    /// </summary>
    /// <seealso cref="IDataSource" />
    public interface IDapperDataSource : IDataSource, IDbConnection
    {
    }
}
