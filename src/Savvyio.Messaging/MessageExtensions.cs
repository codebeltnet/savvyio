using System;
using Cuemon;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Extension methods for the <see cref="IMessage{T}"/> interface.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Clones the specified <paramref name="message"/> using either the specified <paramref name="copier"/> or the default implementation that return a new instance of <see cref="Message{TRequest}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the payload constraint to the <see cref="IRequest"/> interface.</typeparam>
        /// <param name="message">The <see cref="IMessage{T}"/> to clone.</param>
        /// <param name="copier">The function delegate that is used to clone the specified <paramref name="message"/>. Default is <c>null</c>.</param>
        /// <returns>An implementation of <see cref="IMessage{T}"/> that is an otherwise clone of the specified <paramref name="message"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> cannot be null.
        /// </exception>
        public static IMessage<T> Clone<T>(this IMessage<T> message, Func<IMessage<T>> copier = null) where T : IRequest
        {
            Validator.ThrowIfNull(message);
            return copier?.Invoke() ?? new Message<T>()
            {
                Data = message.Data,
                Id = message.Id,
                Source = message.Source,
                Time = message.Time,
                Type = message.Type
            };
        }
    }
}
