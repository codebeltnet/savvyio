namespace Savvyio.Messaging.Cryptography
{
    /// <summary>
    /// Defines a generic way to wrap an <see cref="IRequest" /> inside a cryptographically signed message.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IRequest"/> interface.</typeparam>
	public interface ISignedMessage<out T> : IMessage<T> where T : IRequest
	{
        /// <summary>
        /// Gets the cryptographic signature of the message.
        /// </summary>
        /// <value>The cryptographic signature of the message.</value>
        string Signature { get; }
	}
}
