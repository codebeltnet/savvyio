using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Reflection;

namespace Savvyio.Domain
{
    /// <summary>
    /// Represents an object whose equality is based on the value rather than identity as specified in Domain Driven Design.
    /// </summary>
    /// <seealso cref="T:IEquatable{ValueObject}" />
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        private const int NullHashCode = 472074819;
        private readonly object _locker = new();
        private IEnumerable<object> _equalityComponents;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObject"/> class.
        /// </summary>
        protected ValueObject()
        {
        }

        /// <summary>
        /// Indicates whether two <see cref="ValueObject"/> instances are equal.
        /// </summary>
        /// <param name="vo1">The first <see cref="ValueObject"/> to compare.</param>
        /// <param name="vo2">The second <see cref="ValueObject"/> to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="vo1"/> and <paramref name="vo2"/> are equal; otherwise, false. </returns>
        public static bool operator ==(ValueObject vo1, ValueObject vo2)
        {
            if (ReferenceEquals(vo1, null) ^ ReferenceEquals(vo2, null)) { return false; }
            return ReferenceEquals(vo1, null) || vo1.Equals(vo2);
        }

        /// <summary>
        /// Indicates whether two <see cref="ValueObject"/> instances are not equal.
        /// </summary>
        /// <param name="vo1">The first <see cref="ValueObject"/> to compare.</param>
        /// <param name="vo2">The second <see cref="ValueObject"/> to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="vo1"/> and <paramref name="vo2"/> are not equal; otherwise, false.</returns>
        public static bool operator !=(ValueObject vo1, ValueObject vo2)
        {
            return !(vo1 == vo2);
        }

        /// <summary>
        /// Gets the equality components of this instance. Default is all public properties having a simple signature.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the equality components of this instance.</returns>
        protected virtual IEnumerable<object> GetEqualityComponents()
        {
            if (_equalityComponents == null)
            {
                lock (_locker)
                {
                    if (_equalityComponents == null)
                    {
                        var equalityComponents = new List<object>();
                        var properties = GetType().GetProperties(new MemberReflection(true, true)).Where(pi => pi.PropertyType.IsSimple() && pi.CanRead);
                        foreach (var property in properties)
                        {
                            equalityComponents.Add(property.GetValue(this));
                        }
                        _equalityComponents = equalityComponents;
                    }
                }
            }
            return _equalityComponents;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ValueObject)) { return false; }
            return Equals((ValueObject)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public virtual bool Equals(ValueObject other)
        {
            if (ReferenceEquals(this, other)) { return true; }
            if (ReferenceEquals(null, other)) { return false; }
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return GetEqualityComponents().Select(x => x != null ? x.GetHashCode() : NullHashCode).GetHashCode32();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return DelimitedString.Create(GetEqualityComponents());
        }
    }
}
