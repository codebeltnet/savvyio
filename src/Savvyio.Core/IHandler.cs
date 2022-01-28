namespace Savvyio
{
    /// <summary>
    /// A marker interface that specifies a handler.
    /// </summary>
    public interface IHandler
    {
    }

    /// <summary>
    /// Defines a marker interface for a generic handler.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to handle.</typeparam>
    public interface IHandler<TRequest> : IHandler where TRequest : IRequest
    {
    }
}
