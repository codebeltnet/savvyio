namespace Savvyio.Messaging
{
    /// <summary>
    /// Specifies an interface for a bus that is used for interacting with other subsystems (out-process/inter-application) to do something (e.g., change the state).
    /// </summary>
    /// <seealso cref="ISender{TRequest}" />
    /// <seealso cref="IReceiver{TRequest}" />
    public interface IPointToPointChannel<TRequest> : ISender<TRequest>, IReceiver<TRequest> where TRequest : IRequest
    {
    }
}
