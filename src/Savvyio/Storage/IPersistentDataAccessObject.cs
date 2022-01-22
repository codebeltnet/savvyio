namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way of abstracting persistent data access (CRUD).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    public interface IPersistentDataAccessObject<T> : IWritableDataAccessObject<T>, IReadableDataAccessObject<T>, IDeletableDataAccessObject<T> where T : class
    {
    }
}
