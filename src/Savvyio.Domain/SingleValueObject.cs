using System.Collections.Generic;

namespace Savvyio.Domain
{
    /// <summary>
    /// Provides an implementation of <see cref="ValueObject"/> tailored for handling a single value.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <seealso cref="ValueObject" />
    public abstract class SingleValueObject<T> : ValueObject
    {
        /// <summary>
        /// Performs an implicit conversion from <see cref="SingleValueObject{T}"/> to <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><typeparamref name="T"/> that is equivalent to <paramref name="value"/>.</returns>
        public static implicit operator T(SingleValueObject<T> value)
        {
            return value.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleValueObject{T}"/> class.
        /// </summary>
        /// <param name="value">The value to associate to <see cref="Value"/>.</param>
        protected SingleValueObject(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value of this instance.
        /// </summary>
        /// <value>The value of this instance.</value>
        public T Value { get; }

        /// <summary>
        /// Gets the equality components of this instance.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains the equality components of this instance.</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }

    internal class DefaultSingleValueObject<T> : SingleValueObject<T>
    {
        public DefaultSingleValueObject(T value) : base(value)
        {
        }
    }
}
