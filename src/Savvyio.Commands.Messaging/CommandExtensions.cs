﻿using System;
using Cuemon;
using Savvyio.Messaging;

namespace Savvyio.Commands.Messaging
{
    /// <summary>
    /// Extension methods for the <see cref="ICommand"/> interface.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Encloses the specified <paramref name="command"/> to an instance of <see cref="Message{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the payload constraint to the <see cref="ICommand"/> interface.</typeparam>
        /// <param name="command">The payload to attach within the message.</param>
        /// <param name="source">The context that describes the origin of the message.</param>
        /// <param name="type">The type that describes the type of <paramref name="command"/> related to the originating occurrence.</param>
        /// <param name="setup">The <see cref="MessageOptions" /> which may be configured.</param>
        /// <returns>An instance of <see cref="Message{T}"/> constraint to the <see cref="ICommand"/> interface.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="command"/> cannot be null - or -
        /// <paramref name="source"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="MessageOptions"/> in a valid state.
        /// </exception>
        public static IMessage<T> ToMessage<T>(this T command, Uri source, string type, Action<MessageOptions> setup = null) where T : ICommand
        {
            Validator.ThrowIfNull(command);
            Validator.ThrowIfNull(source);
            Validator.ThrowIfNullOrWhitespace(type);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            return new Message<T>(options.MessageId, source, type, command, options.Time);
        }
    }
}
