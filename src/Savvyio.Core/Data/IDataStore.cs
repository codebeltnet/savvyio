namespace Savvyio.Data
{
    /// <summary>
    /// A marker interface that specifies an abstraction of data persistence based on the Data Access Object pattern aka DAO.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    public interface IDataStore<in T> where T : class
    {
    }
}
