using System;
using System.Linq;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Cuemon;
using Cuemon.Configuration;

namespace Savvyio.Extensions.SimpleQueueService
{
    /// <summary>
    /// Configuration options that is related to AWS SQS and AWS SNS.
    /// </summary>
    /// <seealso cref="IValidatableParameterObject" />
    public class AmazonMessageOptions : IValidatableParameterObject
    {
        /// <summary>
        /// The maximum number of messages that AWS SQS supports when sending or retrieving.
        /// </summary>
        public const int MaxNumberOfMessages = 10;

        /// <summary>
        /// The maximum amount of seconds that a call waits for a message to arrive in AWS SQS.
        /// </summary>
        public const int MaxPollingWaitTimeInSeconds = 20;

        /// <summary>
        /// The maximum amount of seconds that a message can be hidden from other consumers in AWS SQS after it has been received.
        /// </summary>
        public const int MaxVisibilityTimeoutInSeconds = 12 * 60 * 60;

        /// <summary>
        /// The default amount of seconds that a message can be hidden from other consumers in AWS SQS after it has been received.
        /// </summary>
        public const int DefaultVisibilityTimeoutInSeconds = 30;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonMessageOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AmazonMessageOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Credentials"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Endpoint"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SourceQueue"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ClientConfigurations"/></term>
        ///         <description><c>Enumerable.Empty&lt;ClientConfig&gt;().ToArray();</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public AmazonMessageOptions()
        {
            ReceiveContext = new AmazonMessageReceiveOptions();
            ClientConfigurations = Enumerable.Empty<ClientConfig>().ToArray();
        }

        /// <summary>
        /// Gets or sets the credentials required to connect to AWS SQS/SNS.
        /// </summary>
        /// <value>The credentials required to connect to AWS SQS/SNS.</value>
        public AWSCredentials Credentials { get; set; }

        /// <summary>
        /// Gets or sets the endpoint required to connect to AWS SQS/SNS.
        /// </summary>
        /// <value>The endpoint required to connect to AWS SQS/SNS.</value>
        public RegionEndpoint Endpoint { get; set; }

        /// <summary>
        /// Gets the client configuration for AWS SQS/SNS provided by <see cref="ConfigureClient"/>.
        /// </summary>
        /// <value>The configuration of the AWS SQS/SNS.</value>
        /// <remarks>When this property is set, it will take precedence over the <see cref="Endpoint"/> property. Useful for testing with a local SQS/SNS instance such as LocalStack or require full configuration flexibility.</remarks>
        public ClientConfig[] ClientConfigurations { get; private set; }

        /// <summary>
        /// Provides a flexible way to configure client configurations for both AWS SQS/SNS in a single call.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        /// <remarks>When this method is invoked, it will set the <see cref="ClientConfigurations"/> property that take precedence over the <see cref="Endpoint"/> property. Useful for testing with a local SQS/SNS instance such as LocalStack or require full configuration flexibility.</remarks>
        public AmazonMessageOptions ConfigureClient(Action<ClientConfig> setup)
        {
            Validator.ThrowIfNull(setup);
            var sqsConfig = new AmazonSQSConfig();
            var snsConfig = new AmazonSimpleNotificationServiceConfig();
            setup(sqsConfig);
            setup(snsConfig);
            ClientConfigurations = new ClientConfig[] { sqsConfig, snsConfig };
            return this;
        }

        /// <summary>
        /// Gets or sets the URI that represents an AWS SQS/SNS endpoint.
        /// </summary>
        /// <value>The URI that represents an AWS SQS/SNS endpoint.</value>
        public Uri SourceQueue { get; set; }

        /// <summary>
        /// Gets the options related to receive operations on AWS SQS.
        /// </summary>
        /// <value>The options related to receive operations on AWS SQS.</value>
        public AmazonMessageReceiveOptions ReceiveContext { get; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Credentials"/> cannot be null - or -
        /// <see cref="Endpoint"/> cannot be null - or -
        /// <see cref="SourceQueue"/> cannot be null - or -
        /// <see cref="ReceiveContext"/> cannot be null -or-
        /// <see cref="ClientConfigurations"/> is initialized but does not have a length of 2 or the expected elements of type <see cref="AmazonSQSConfig"/> or <see cref="AmazonSimpleNotificationServiceConfig"/>.
        /// </exception>
        public void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(Credentials == null);
            Validator.ThrowIfInvalidState(Endpoint == null);
            Validator.ThrowIfInvalidState(SourceQueue == null);
            Validator.ThrowIfInvalidState(ReceiveContext == null);
            Validator.ThrowIfInvalidState(ClientConfigurations.Length > 0 && (ClientConfigurations.Length != 2 || !(ClientConfigurations[0] is AmazonSQSConfig && ClientConfigurations[1] is AmazonSimpleNotificationServiceConfig)));
        }
    }
}
