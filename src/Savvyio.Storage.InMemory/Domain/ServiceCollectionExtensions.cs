using System;
using Microsoft.Extensions.DependencyInjection;

namespace Savvyio.Domain
{
    public static class ServiceCollectionExtensions
    {
        //public static IServiceCollection AddInMemoryActiveRecordStore(this IServiceCollection services)
        //{
        //    return services.AddActiveRecordStore<InMemoryActiveRecordStore>(ServiceLifetime.Singleton);
        //}

        //public static IServiceCollection AddInMemoryActiveRecordStore(this IServiceCollection services, Action<InMemoryActiveRecordStoreOptions> setup)
        //{
        //    return services.AddActiveRecordStore<InMemoryActiveRecordStore, InMemoryActiveRecordStoreOptions>(setup, ServiceLifetime.Singleton);
        //}

        public static IServiceCollection AddInMemoryActiveRecordStore<TAggregate, TKey>(this IServiceCollection services) where TAggregate : class, IAggregateRoot<TKey>
        {
            return services.AddActiveRecordStore<TAggregate, TKey, InMemoryActiveRecordStore<TAggregate, TKey>>(ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddInMemoryActiveRecordStore<TAggregate, TKey>(this IServiceCollection services, Action<InMemoryActiveRecordStoreOptions<TAggregate, TKey>> setup) where TAggregate : class, IAggregateRoot<TKey>
        {
            return services.AddActiveRecordStore<TAggregate, TKey, InMemoryActiveRecordStore<TAggregate, TKey>, InMemoryActiveRecordStoreOptions<TAggregate, TKey>>(setup, ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddInMemoryEventSourcingStore(this IServiceCollection services)
        {
            return services.AddEventSourcingStore<InMemoryEventSourcingStore>(ServiceLifetime.Singleton);
        }
    }
}
