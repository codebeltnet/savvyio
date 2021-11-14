using System;
using Cuemon.Extensions;

namespace Savvyio.Domain
{
    public static class DomainEventExtensions
    {
        public static T SetCausationId<T>(this T model, string causationId) where T : IDomainEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.CausationId, causationId);
            return model;
        }

        public static T SetCorrelationId<T>(this T model, string correlationId) where T : IDomainEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.CorrelationId, correlationId);
            return model;
        }

        public static T SetEventId<T>(this T model, string eventId) where T : IDomainEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.EventId, eventId);
            return model;
        }

        public static T SetTimestamp<T>(this T model) where T : IDomainEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.Timestamp, DateTime.UtcNow);
            return model;
        }

        public static string GetCausationId<T>(this T model) where T : IDomainEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.CausationId).As<string>();
        }
        public static string GetCorrelationId<T>(this T model) where T : IDomainEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.CorrelationId).As<string>();
        }

        public static string GetEventId<T>(this T model) where T : IDomainEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.EventId).As<string>();
        }
    }
}
