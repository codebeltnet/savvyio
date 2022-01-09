namespace Savvyio.Repositories
{
    /// <summary>
    /// A marker interface that specifies the strategy for abstracting data access.
    /// </summary>
    public interface IRepository
    {
    }

    /// <summary>
    /// Defines a marker interface for a generic way of abstracting data access.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="IRepository" />
    public interface IRepository<TModel> : IRepository
    {
    }
}
