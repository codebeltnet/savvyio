namespace Savvyio.Data
{
    /// <summary>
    /// A marker interface that specifies an abstraction of persistent data access based on the Data Access Object pattern.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    public interface IDataAccessObject<in T>  where T : class
    {
    }
}
