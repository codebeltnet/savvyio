using System;
using Cuemon;
using Cuemon.Configuration;
using Cuemon.Security.Cryptography;

namespace Savvyio.Messaging.Cryptography
{
    /// <summary>
    /// Configuration options that is related to wrapping an <see cref="IRequest"/> implementation inside a cryptographically signed message.
    /// </summary>
    /// <seealso cref="IValidatableParameterObject" />
	public class SignedMessageOptions : IValidatableParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignedMessageOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="MessageOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="SignatureAlgorithm"/></term>
        ///         <description><see cref="KeyedCryptoAlgorithm.HmacSha256"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public SignedMessageOptions()
        {
            SignatureAlgorithm = KeyedCryptoAlgorithm.HmacSha256;
            SignatureSecret = null;
        }

        /// <summary>
        /// Gets or sets the cryptographical algorithm used when signing the serialized <see cref="ISignedMessage{T}"/>.
        /// </summary>
        /// <value>The cryptographical algorithm used when signing the serialized <see cref="ISignedMessage{T}"/>.</value>
        public KeyedCryptoAlgorithm SignatureAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the secret key used when signing the serialized <see cref="ISignedMessage{T}"/>.
        /// </summary>
        /// <value>The secret key used when signing the serialized <see cref="ISignedMessage{T}"/>.</value>
        public byte[] SignatureSecret { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        /// <exception cref="InvalidOperationException">
        /// <see cref="SignatureSecret"/> cannot be null - or -
        /// <see cref="SignatureSecret"/> cannot have a length of zero.
        /// </exception>
		public void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(SignatureSecret == null || SignatureSecret.Length == 0);
        }
    }
}
