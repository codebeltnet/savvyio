﻿using System;
using Cuemon.Configuration;

namespace Savvyio.Extensions.QueueStorage
{
    /// <summary>
    /// Configuration options that is related to receiving messages from an Azure Storage Queue.
    /// </summary>
    public class AzureQueueReceiveOptions : IParameterObject
    {
        private int _numberOfMessagesToTakePerRequest;
        private TimeSpan _visibilityTimeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueueReceiveOptions"/> class.
        /// </summary>
        internal AzureQueueReceiveOptions()
        {
            NumberOfMessagesToTakePerRequest = 10;
            VisibilityTimeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Gets or sets the number of messages to return per request. Default is 10.
        /// </summary>
        /// <value>The number of messages to return per request.</value>
        /// <remarks>Max. allowed value is limited to <see cref="AzureQueueOptions.MaxNumberOfMessages"/>.</remarks>
        public int NumberOfMessagesToTakePerRequest
        {
            get => _numberOfMessagesToTakePerRequest;
            set => _numberOfMessagesToTakePerRequest = Math.Clamp(value, 1, AzureQueueOptions.MaxNumberOfMessages);
        }

        /// <summary>
        /// Gets or sets the visibility timeout for the messages. Default is 30 seconds.
        /// </summary>
        /// <value>The visibility timeout for the messages.</value>
        /// <remarks>
        /// The minimum allowed value is 1 second, and the maximum allowed value is limited to <see cref="AzureQueueOptions.MaxVisibilityTimeout"/>.
        /// <br/>
        /// Reference: https://learn.microsoft.com/en-us/rest/api/storageservices/get-messages#uri-parameters
        /// </remarks>
        public TimeSpan VisibilityTimeout
        {
            get => _visibilityTimeout;
            set
            {
                if (value < TimeSpan.FromSeconds(1)) { value = TimeSpan.FromSeconds(1); }
                if (value > AzureQueueOptions.MaxVisibilityTimeout) { value = AzureQueueOptions.MaxVisibilityTimeout; }
                _visibilityTimeout = value;
            }
        }
    }
}
