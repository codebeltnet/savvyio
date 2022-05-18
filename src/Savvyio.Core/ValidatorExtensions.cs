using System;
using Cuemon;

namespace Savvyio
{
    /// <summary>
    /// Extension methods for the <see cref="Validator"/> class.
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Provides a convenient way to validate a parameter while returning the specified <paramref name="value"/> unaltered.
        /// </summary>
        /// <typeparam name="T">The type of the object to evaluate.</typeparam>
        /// <param name="_">The <see cref="Validator"/> to extend.</param>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="validator">The delegate that must throw an <see cref="Exception"/> if the specified <paramref name="value"/> is not valid.</param>
        /// <returns>The specified <paramref name="value"/> unaltered.</returns>
        public static T InvalidState<T>(this Validator _, T value, Action<T> validator)
        {
            validator(value);
            return value;
        }
    }
}
