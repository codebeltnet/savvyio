namespace Savvyio.Storage
{
    /// <summary>
    /// A marker interface that specifies the actual I/O communication with a data store.
    /// </summary>
    /// <remarks>A store can be anything that holds data, eg. high level DbContext (EFCore) to low level ADO.NET.</remarks>
    public interface IDataStore : IUnitOfWork
    {
    }
}
