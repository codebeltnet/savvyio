using System;
using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;

namespace Savvyio.Events
{
    public static class IntegrationEventExtensions
    {
        public static T SetCausationId<T>(this T model, string causationId) where T : IIntegrationEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.CausationId, causationId);
            return model;
        }

        public static T SetCorrelationId<T>(this T model, string correlationId) where T : IIntegrationEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.CorrelationId, correlationId);
            return model;
        }

        public static T SetEventId<T>(this T model, string eventId) where T : IIntegrationEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.EventId, eventId);
            return model;
        }

        public static T SetTimestamp<T>(this T model) where T : IIntegrationEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.Timestamp, DateTime.UtcNow);
            return model;
        }

        public static T SetMemberType<T>(this T model, Type type) where T : IIntegrationEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.MemberType, type.ToFullNameIncludingAssemblyName());
            return model;
        }

        public static string GetCausationId<T>(this T model) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.CausationId).As<string>();
        }
        public static string GetCorrelationId<T>(this T model) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.CorrelationId).As<string>();
        }

        public static string GetEventId<T>(this T model) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.EventId).As<string>();
        }

        /// <summary>
        /// Gets the string representation of the type from the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <returns>The string representation of the type from the <paramref name="model"/>.</returns>
        public static string GetMemberType<T>(this T model) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.MemberType).As<string>();
        }
    }
}
