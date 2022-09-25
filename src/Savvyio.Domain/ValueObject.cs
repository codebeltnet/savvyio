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
    public abstract record ValueObject
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
                        var properties = GetType().GetProperties(new MemberReflection(true, true)).Where(pi => (pi.PropertyType.IsSimple() || pi.PropertyType.IsAssignableTo(typeof(ValueObject))) && pi.CanRead);
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
