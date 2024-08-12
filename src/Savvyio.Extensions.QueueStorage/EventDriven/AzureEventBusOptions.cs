using System;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Messaging.EventGrid;
using Cuemon;
using Cuemon.Configuration;

namespace Savvyio.Extensions.QueueStorage.EventDriven
{
    /// <summary>  
    /// Configuration options for <see cref="AzureEventBus"/>.
    /// </summary>  
    public class AzureEventBusOptions : IValidatableParameterObject
    {
        private TokenCredential _credential;
        private AzureKeyCredential _keyCredential;
        private AzureSasCredential _sasCredential;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureEventBusOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AzureEventBusOptions"/>.
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
        ///         <term><see cref="KeyCredential"/></term>
        ///         <description><c>new QueueClientOptions()</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SasCredential"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="TopicEndpoint"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public AzureEventBusOptions()
        {
            Credential = new DefaultAzureCredential();
            Settings = new EventGridPublisherClientOptions();
        }

        /// <summary>
        /// Gets or sets the topic endpoint URI.
        /// </summary>
        /// <value>The topic endpoint URI.</value>
        public Uri TopicEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the token credential for Azure authentication.
        /// </summary>
        /// <value>The token credential.</value>
        /// <remarks>Setting this property will nullify <see cref="SasCredential"/> and <see cref="KeyCredential"/> as they are mutually exclusive.</remarks>
        public TokenCredential Credential
        {
            get => _credential;
            set
            {
                _credential = value;
                _sasCredential = null;
                _keyCredential = null;
            }
        }

        /// <summary>
        /// Gets or sets the key credential for Azure authentication.
        /// </summary>
        /// <value>The key credential.</value>
        /// <remarks>Setting this property will nullify <see cref="Credential"/> and <see cref="SasCredential"/> as they are mutually exclusive.</remarks>
        public AzureKeyCredential KeyCredential
        {
            get => _keyCredential;
            set
            {
                _keyCredential = value;
                _credential = null;
                _sasCredential = null;
            }
        }

        /// <summary>
        /// Gets or sets the SAS credential for Azure authentication.
        /// </summary>
        /// <value>The SAS credential.</value>
        /// <remarks>Setting this property will nullify <see cref="Credential"/> and <see cref="KeyCredential"/> as they are mutually exclusive.</remarks>
        public AzureSasCredential SasCredential
        {
            get => _sasCredential;
            set
            {
                _sasCredential = value;
                _credential = null;
                _keyCredential = null;
            }
        }

        /// <summary>
        /// Gets the settings for the Event Grid publisher client.
        /// </summary>
        /// <value>The Event Grid publisher client options.</value>
        public EventGridPublisherClientOptions Settings { get; }

        /// <summary>
        /// Validates the options to ensure they are in a valid state.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the options are in an invalid state.</exception>
        public void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(TopicEndpoint == null, $"A {nameof(TopicEndpoint)} is required.");

            Validator.ThrowIfInvalidState(Credential == null && SasCredential == null && KeyCredential == null, "A credential type has to be specified for Azure authentication.");
        }
    }
}
