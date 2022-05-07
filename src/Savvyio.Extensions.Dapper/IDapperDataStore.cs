using System.Data;

namespace Savvyio.Extensions.Dapper
{
    /// <summary>
    /// Defines a generic way to support the actual I/O communication towards a data store optimized for Dapper.
    /// </summary>
    /// <seealso cref="IDataStore" />
    public interface IDapperDataStore : IDataStore, IDbConnection
    {
    }
}
