using Savvyio.Data;
using Savvyio.Domain;

namespace Savvyio
{
    /// <summary>
    /// A marker interface that specifies the actual I/O communication with a source of data.
    /// </summary>
    /// <remarks>A data source can be anything that holds data, eg. high level DbContext (EFCore) to low level ADO.NET. It could also be as simple as files, services or anything that can expose data.</remarks>
    /// <seealso cref="IDataStore{T}"/>
    /// <seealso cref="IRepository{TEntity,TKey}"/>
    public interface IDataSource
    {
    }
}
