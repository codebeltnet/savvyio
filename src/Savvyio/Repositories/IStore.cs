namespace Savvyio.Repositories
{
    /// <summary>
    /// A marker interface that specifies the actual I/O communication towards a data store.
    /// </summary>
    /// <remarks>A store can be anything that holds data. Store reminds of a simplified version of DbContext (EFCore) / UnitOfWork (UoW).</remarks>
    public interface IStore
    {
    }
}
