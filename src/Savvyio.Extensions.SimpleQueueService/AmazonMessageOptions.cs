using System;
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
		/// The maximum seconds that a call waits for a message to arrive in AWS SQS.
		/// </summary>
		public const int MaxPollingWaitTimeInSeconds = 20;

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
		/// </list>
		/// </remarks>
		public AmazonMessageOptions()
		{
			ReceiveContext = new AmazonMessageReceiveOptions();
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
        /// Gets or sets the configuration of the AWS SQS.
        /// </summary>
        /// <value>The configuration of the AWS SQS.</value>
        /// <remarks>When this property is set, it will take precedence over the <see cref="Endpoint"/> property. Useful for testing with a local SQS instance such as LocalStack or require full configuration flexibility.</remarks>
        public AmazonSQSConfig SqsConfig { get; set; }

        /// <summary>
        /// Gets or sets the configuration of the AWS SNS.
        /// </summary>
        /// <value>The configuration of the AWS SNS.</value>
        /// <remarks>When this property is set, it will take precedence over the <see cref="Endpoint"/> property. Useful for testing with a local SNS instance such as LocalStack or require full configuration flexibility.</remarks>
        public AmazonSimpleNotificationServiceConfig SnsConfig { get; set; }

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
        /// <see cref="ReceiveContext"/> cannot be null.
        /// </exception>
        public void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(Credentials == null);
            Validator.ThrowIfInvalidState(Endpoint == null);
            Validator.ThrowIfInvalidState(SourceQueue == null);
            Validator.ThrowIfInvalidState(ReceiveContext == null);
        }
    }
}
