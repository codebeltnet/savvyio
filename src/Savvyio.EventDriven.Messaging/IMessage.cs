using System;

namespace Savvyio.EventDriven.Messaging
{
    public interface IMessage<out TData>
    {
        /// <summary>
        /// Gets the time of occurrence for the message.
        /// </summary>
        /// <value>The time of occurrence for the message.</value>
        DateTime Timestamp { get; }

        public TData Data { get; }
    }
}
