using System;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Queues;
using Cuemon;
using Cuemon.Configuration;

namespace Savvyio.Extensions.QueueStorage
{
    /// <summary>  
    /// Configuration options for <see cref="AzureQueue{TRequest}"/>.
    /// </summary>  
    public class AzureQueueOptions : IValidatableParameterObject
    {
        /// <summary>  
        /// The maximum number of messages that Azure Queue Storage supports when retrieving.
        /// </summary>  
        public const int MaxNumberOfMessages = 32;

        /// <summary>  
        /// The maximum visibility timeout for messages in Azure Queue Storage.
        /// </summary>  
        public static readonly TimeSpan MaxVisibilityTimeout = TimeSpan.FromDays(7);

        private Action<QueueClient> _clientCallback;
        private StorageSharedKeyCredential _storageKeyCredential;
        private AzureSasCredential _sasCredential;
        private TokenCredential _credential;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueueOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AzureQueueOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Credential"/></term>
        ///         <description><c>new DefaultAzureCredential()</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="StorageAccountName"/></term>
        ///         <description><c>new QueueClientOptions()</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="QueueName"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SasCredential"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="StorageKeyCredential"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ConnectionString"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public AzureQueueOptions()
        {
            Credential = new DefaultAzureCredential();
            Settings = new QueueClientOptions();
            ReceiveContext = new AzureQueueReceiveOptions();
            SendContext = new AzureQueueSendOptions();
        }

        /// <summary>  
        /// Gets the options for receiving messages from the queue.
        /// </summary>  
        public AzureQueueReceiveOptions ReceiveContext { get; }

        /// <summary>  
        /// Gets the options for sending messages to the queue.
        /// </summary>  
        public AzureQueueSendOptions SendContext { get; }

        /// <summary>  
        /// Gets or sets the name of the storage account.
        /// </summary>  
        public string StorageAccountName { get; set; }

        /// <summary>  
        /// Gets or sets the name of the queue.
        /// </summary>  
        public string QueueName { get; set; }

        /// <summary>  
        /// Gets or sets the credential used to authenticate with Azure.
        /// </summary>
        /// <remarks>Setting this property will nullify <see cref="SasCredential"/> and <see cref="StorageKeyCredential"/> as they are mutually exclusive.</remarks>
        public TokenCredential Credential
        {
            get => _credential;
            set
            {
                _credential = value;
                _sasCredential = null;
                _storageKeyCredential = null;
            }
        }

        /// <summary>  
        /// Gets or sets the SAS credential used to authenticate with Azure.
        /// </summary>
        /// <remarks>Setting this property will nullify <see cref="Credential"/> and <see cref="StorageKeyCredential"/> as they are mutually exclusive.</remarks>
        public AzureSasCredential SasCredential
        {
            get => _sasCredential;
            set
            {
                _sasCredential = value;
                _credential = null;
                _storageKeyCredential = null;
            }
        }

        /// <summary>  
        /// Gets or sets the storage key credential used to authenticate with Azure.
        /// </summary>
        /// <remarks>Setting this property will nullify <see cref="Credential"/> and <see cref="SasCredential"/> as they are mutually exclusive.</remarks>
        public StorageSharedKeyCredential StorageKeyCredential
        {
            get => _storageKeyCredential;
            set
            {
                _storageKeyCredential = value;
                _credential = null;
                _sasCredential = null;
            }
        }

        /// <summary>  
        /// Gets or sets the connection string used to connect to the Azure storage account.
        /// </summary>  
        public string ConnectionString { get; set; }

        /// <summary>  
        /// Gets the settings for the <see cref="QueueClient"/>>.
        /// </summary>  
        public QueueClientOptions Settings { get; }

        /// <summary>
        /// Provides a way to post-configure the client after it has been created.
        /// </summary>
        /// <param name="factory">The delegate to post-configure the client.</param>
        /// <returns>A reference to this instance.</returns>
        /// <remarks>This was added to support invoking methods of interest: https://learn.microsoft.com/en-us/dotnet/api/azure.storage.queues.queueclient?view=azure-dotnet#methods</remarks>
        public AzureQueueOptions PostConfigureClient(Action<QueueClient> factory)
        {
            _clientCallback = factory;
            return this;
        }

        internal void SetConfiguredClient(QueueClient client)
        {
            _clientCallback?.Invoke(client);
        }

        /// <summary>  
        /// Validates the options to ensure they are in a valid state.  
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="QueueName"/> cannot be null, empty, or consists only of white-space characters when <see cref="ConnectionString"/> was specified -or-
        /// <see cref="Credential"/> and <see cref="SasCredential"/> and <see cref="StorageKeyCredential"/> cannot be null -or-
        /// <see cref="StorageAccountName"/> and <see cref="QueueName"/> cannot be null, empty, or consists only of white-space characters.
        /// </exception>
        public void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(!string.IsNullOrWhiteSpace(ConnectionString) && string.IsNullOrWhiteSpace(QueueName), $"{nameof(QueueName)} cannot be null, empty, or consists only of white-space when a {nameof(ConnectionString)} has been specified.");

            Validator.ThrowIfInvalidState(Credential == null && SasCredential == null && StorageKeyCredential == null, "A credential type has to be specified for Azure authentication.");

            Validator.ThrowIfInvalidState(string.IsNullOrWhiteSpace(StorageAccountName) && string.IsNullOrWhiteSpace(QueueName), $"{nameof(StorageAccountName)} and {nameof(QueueName)} cannot be null, empty, or consists only of white-space characters.");

        }
    }
}
