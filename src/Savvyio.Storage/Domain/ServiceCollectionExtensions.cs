using System;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Domain.EventSourcing;

namespace Savvyio.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddActiveRecordRepository<TAggregate, TKey>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TAggregate : class, IAggregateRoot<IDomainEvent, TKey>
        {
            return services.Add<IActiveRecordRepository<TAggregate, TKey>, ActiveRecordRepository<TAggregate, TKey>>(lifetime);
        }

        public static IServiceCollection AddActiveRecordRepository<TAggregate, TKey, TOptions>(this IServiceCollection services, Action<TOptions> setup, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TAggregate : class, IAggregateRoot<IDomainEvent, TKey>
            where TOptions : class, new()
        {
            return services.Add<IActiveRecordRepository<TAggregate, TKey>, ActiveRecordRepository<TAggregate, TKey>, TOptions>(lifetime, setup);
        }

        
        //public static IServiceCollection AddActiveRecordStore<TImplementation>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        //    where TImplementation : class, IActiveRecordStore
        //{
        //    return services.Add<IActiveRecordStore, TImplementation>(lifetime);
        //}

        //public static IServiceCollection AddActiveRecordStore<TImplementation, TOptions>(this IServiceCollection services, Action<TOptions> setup, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        //    where TImplementation : class, IActiveRecordStore
        //    where TOptions : class, new()
        //{
        //    return services.Add<IActiveRecordStore, TImplementation, TOptions>(lifetime, setup);
        //}

        public static IServiceCollection AddActiveRecordStore<TAggregate, TKey, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TAggregate : class, IAggregateRoot<IDomainEvent, TKey>
            where TImplementation : class, IActiveRecordStore<TAggregate, TKey>
        {
            return services.Add<IActiveRecordStore<TAggregate, TKey>, TImplementation>(lifetime);
        }

        public static IServiceCollection AddActiveRecordStore<TAggregate, TKey, TImplementation, TOptions>(this IServiceCollection services, Action<TOptions> setup, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TAggregate : class, IAggregateRoot<IDomainEvent, TKey>
            where TImplementation : class, IActiveRecordStore<TAggregate, TKey>
            where TOptions : class, new()
        {
            return services.Add<IActiveRecordStore<TAggregate, TKey>, TImplementation, TOptions>(lifetime, setup);
        }

        public static IServiceCollection AddEventSourcingRepository<TAggregate, TKey>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TAggregate : class, ITracedAggregateRoot<TKey>
        {
            return services.Add<IEventSourcingRepository<TAggregate, TKey>, EventSourcingRepository<TAggregate, TKey>>(lifetime);
        }

        public static IServiceCollection AddEventSourcingRepository<TAggregate, TKey, TOptions>(this IServiceCollection services, Action<TOptions> setup, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TAggregate : class, ITracedAggregateRoot<TKey>
            where TOptions : class, new()
        {
            return services.Add<IEventSourcingRepository<TAggregate, TKey>, EventSourcingRepository<TAggregate, TKey>, TOptions>(lifetime, setup);
        }

        public static IServiceCollection AddEventSourcingStore<TImplementation>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TImplementation : class, IEventSourcingStore
        {
            return services.Add<IEventSourcingStore, TImplementation>(lifetime);
        }

        public static IServiceCollection AddEventSourcingStore<TImplementation, TOptions>(this IServiceCollection services, Action<TOptions> setup, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TImplementation : class, IEventSourcingStore
            where TOptions : class, new()
        {
            return services.Add<IEventSourcingStore, TImplementation, TOptions>(lifetime, setup);
        }
    }
}
