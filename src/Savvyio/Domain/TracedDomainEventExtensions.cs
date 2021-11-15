using System;
using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;

namespace Savvyio.Domain
{
    /// <summary>
    /// Extension methods for the <see cref="ITracedDomainEvent"/> interface.
    /// </summary>
    public static class TracedDomainEventExtensions
    {
        /// <summary>
        /// Assigns a new <paramref name="version"/> to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="ITracedDomainEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="ITracedDomainEvent"/> to extend.</param>
        /// <param name="version">The aggregate version of the model.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        public static T SetAggregateVersion<T>(this T model, long version) where T : ITracedDomainEvent
        {
            return MetadataFactory.Set(model, MetadataDictionary.AggregateVersion, version);
        }

        /// <summary>
        /// Assigns a new <paramref name="type"/> to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="ITracedDomainEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="ITracedDomainEvent"/> to extend.</param>
        /// <param name="type">The type of the model.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        /// <remarks>The <paramref name="type"/> is converted to its equivalent string representation (fully qualified name of the type, including its namespace, comma delimited with the simple name of the assembly).</remarks>
        public static T SetMemberType<T>(this T model, Type type) where T : ITracedDomainEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.MemberType, type.ToFullNameIncludingAssemblyName());
            return model;
        }

        /// <summary>
        /// Gets the aggregate version from the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="ITracedDomainEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="ITracedDomainEvent"/> to extend.</param>
        /// <returns>The version of the associated <see cref="ITracedAggregateRoot{TKey}"/>.</returns>
        public static long GetAggregateVersion<T>(this T model) where T : ITracedDomainEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.AggregateVersion).As<long>();
        }

        /// <summary>
        /// Gets the string representation of the type from the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="ITracedDomainEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="ITracedDomainEvent"/> to extend.</param>
        /// <returns>The string representation of the type from the <paramref name="model"/>.</returns>
        public static string GetMemberType<T>(this T model) where T : ITracedDomainEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.MemberType).As<string>();
        }
    }
}
