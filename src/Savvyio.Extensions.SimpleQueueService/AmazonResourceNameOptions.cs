using System;
using System.Globalization;
using Amazon;
using Cuemon;
using Cuemon.Configuration;
using Cuemon.Extensions;

namespace Savvyio.Extensions.SimpleQueueService
{
    /// <summary>
    /// Configuration options that is related to Amazon Resource Name (ARN).
    /// </summary>
    /// <remarks>https://docs.aws.amazon.com/general/latest/gr/aws-arns-and-namespaces.html</remarks>
    public class AmazonResourceNameOptions : IValidatableParameterObject
    {
        /// <summary>
        /// Gets or sets the default partition in which the resource is located to embed in the ARN. Default is "aws".
        /// </summary>
        /// <value>The default partition in which the resource is located to embed in the ARN.</value>
        public static string DefaultPartition { get; set; } = RegionEndpoint.EUWest1.PartitionName;

        /// <summary>
        /// Gets or sets the default region code to embed in the ARN. Default is "eu-west-1".
        /// </summary>
        /// <value>The default region code to embed in the ARN.</value>
        public static string DefaultRegion { get; set; } = RegionEndpoint.EUWest1.SystemName;

        /// <summary>
        /// Gets or sets the default ID of the AWS account that owns the resource (without the hyphens) to embed in the ARN. Default is "000000000000".
        /// </summary>
        /// <value>The default ID of the AWS account that owns the resource to embed in the ARN.</value>
        /// <remarks>For example, 123456789012.</remarks>
        public static string DefaultAccountId { get; set; } = "000000000000";

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonResourceNameOptions"/> class.
        /// </summary>
        public AmazonResourceNameOptions()
        {
            Partition = DefaultPartition;
            Region = DefaultRegion;
            AccountId = DefaultAccountId;
        }

        /// <summary>
        /// Gets or sets the partition in which the resource is located to embed in the ARN.
        /// </summary>
        /// <value>The partition to embed in the ARN.</value>
        public string Partition { get; set; }

        /// <summary>
        /// Gets or sets the default region code to embed in the ARN.
        /// </summary>
        /// <value>The region code to embed in the ARN.</value>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the default ID of the AWS account that owns the resource (without the hyphens) to embed in the ARN.
        /// </summary>
        /// <value>The ID of the AWS account that owns the resource to embed in the ARN.</value>
        public string AccountId { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Partition"/> cannot be null, empty or consist only of white-space characters - or -
        /// <see cref="Region"/> cannot be null, empty or consist only of white-space characters - or -
        /// <see cref="AccountId"/> cannot be null, have a length different from 12 or not be parseable to a numeric value.
        /// </exception>
        public void ValidateOptions()
        {
            Validator.ThrowIfObjectInDistress(Partition.IsNullOrWhiteSpace());
            Validator.ThrowIfObjectInDistress(Region.IsNullOrWhiteSpace());
            Validator.ThrowIfObjectInDistress(AccountId.IsNullOrWhiteSpace() || AccountId.Length != 12 || !AccountId.IsNumeric(NumberStyles.Integer));
        }
    }
}
