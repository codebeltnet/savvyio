using System;
using Amazon;
using Amazon.Runtime;
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
        /// Initializes a new instance of the <see cref="AmazonMessageOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="EfCoreServiceOptions"/>.
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
        }

        /// <summary>
        /// Gets or sets the credentials required to connect to AWS.
        /// </summary>
        /// <value>The credentials required to connect to AWS.</value>
        public AWSCredentials Credentials { get; set; }

        /// <summary>
        /// Gets or sets the endpoint required to connect to AWS.
        /// </summary>
        /// <value>The endpoint required to connect to AWS.</value>
        public RegionEndpoint Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the URI that represents an AWS SQS endpoint.
        /// </summary>
        /// <value>The URI that represents an AWS SQS endpoint.</value>
        public Uri SourceQueue { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Credentials"/> cannot be null - or -
        /// <see cref="Endpoint"/> cannot be null - or -
        /// <see cref="SourceQueue"/> cannot be null.
        /// </exception>
        public void ValidateOptions()
        {
            Validator.ThrowIfObjectInDistress(Credentials == null);
            Validator.ThrowIfObjectInDistress(Endpoint == null);
            Validator.ThrowIfObjectInDistress(SourceQueue == null);
        }
    }
}
