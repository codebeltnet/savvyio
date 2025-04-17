using System;

namespace Savvyio.Extensions.SimpleQueueService
{
    /// <summary>
    /// Configuration options that is related to receive operations on AWS SQS.
    /// </summary>
    public class AmazonMessageReceiveOptions
    {
        private TimeSpan _pollingTimeout;
        private int _numberOfMessagesToTakePerRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonMessageReceiveOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AmazonMessageReceiveOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="NumberOfMessagesToTakePerRequest"/></term>
        ///         <description>10</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="PollingTimeout"/></term>
        ///         <description>20 seconds</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AssumeMessageProcessed"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RemoveProcessedMessages"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="UseApproximateNumberOfMessages"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public AmazonMessageReceiveOptions()
        {
            NumberOfMessagesToTakePerRequest = AmazonMessageOptions.MaxNumberOfMessages;
            PollingTimeout = TimeSpan.FromSeconds(AmazonMessageOptions.MaxPollingWaitTimeInSeconds);
            AssumeMessageProcessed = true;
            RemoveProcessedMessages = true;
            UseApproximateNumberOfMessages = false;
        }

        /// <summary>
        /// Gets or sets the number of messages to return per request. Default is 10.
        /// </summary>
        /// <value>The number of messages to return per request.</value>
        /// <remarks>Max. allowed value is limited to <see cref="AmazonMessageOptions.MaxNumberOfMessages"/>.</remarks>
        public int NumberOfMessagesToTakePerRequest
        {
            get => _numberOfMessagesToTakePerRequest;
            set => _numberOfMessagesToTakePerRequest = Math.Clamp(value, 1, AmazonMessageOptions.MaxNumberOfMessages);
        }

        /// <summary>
        /// Gets or sets the polling timeout per request. Default is 20 seconds.
        /// </summary>
        /// <value>The polling timeout per request.</value>
        /// <remarks>Max. allowed value is limited to <see cref="AmazonMessageOptions.MaxPollingWaitTimeInSeconds"/>.</remarks>
        public TimeSpan PollingTimeout
        {
            get => _pollingTimeout;
            set => _pollingTimeout = TimeSpan.FromSeconds(Math.Clamp(value.TotalSeconds, 0, AmazonMessageOptions.MaxPollingWaitTimeInSeconds));
        }

        /// <summary>
        /// Gets or sets a value indicating whether each message should be automatically assumed processed. Default is <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if each message should be assumed automatically processed; otherwise, <c>false</c>.</value>
        public bool AssumeMessageProcessed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether processed messages should be removed after processing. Default is <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if processed messages should be removed after processing; otherwise, <c>false</c>.</value>
        public bool RemoveProcessedMessages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to retrieve the approximate number of messages for the queue in question. Default is <c>false</c>.
        /// </summary>
        /// <value><c>true</c> to retrieve the approximate number of messages for the queue in question; otherwise, <c>false</c>.</value>
        public bool UseApproximateNumberOfMessages { get; set; }
    }
}
