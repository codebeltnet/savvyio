namespace Savvyio.Messaging
{
    /// <summary>
    /// Specifies an interface for a bus that is used for interacting with other subsystems (out-process/inter-application) to do something (e.g., change the state).
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
    /// <seealso cref="ISender{TRequest}" />
    /// <seealso cref="IReceiver{TRequest}" />
    public interface IPointToPointChannel<TRequest> : ISender<TRequest>, IReceiver<TRequest> where TRequest : IRequest
    {
    }
}
