namespace Savvyio.Queries
{
    /// <summary>
    /// A marker interface that specifies something that returns data.
    /// </summary>
    public interface IQuery : IRequest, IMetadata
    {
    }

    /// <summary>
    /// A marker interface that specifies something that returns data.
    /// </summary>
    /// <typeparam name="TResult">The type of the result to return.</typeparam>
    public interface IQuery<TResult> : IQuery
    {
    }
}
