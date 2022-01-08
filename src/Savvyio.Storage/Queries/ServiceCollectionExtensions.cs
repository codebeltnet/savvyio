using System;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Domain;

namespace Savvyio.Queries
{
    //public static class ServiceCollectionExtensions
    //{
    //    public static IServiceCollection AddQueryRepository<TProjection, TKey>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TProjection : class, IIdentity<TKey>
    //    {
    //        return services.Add<IQueryRepository<TProjection, TKey>, QueryRepository<TProjection, TKey>>(lifetime);
    //    }

    //    public static IServiceCollection AddQueryRepository<TProjection, TKey, TOptions>(this IServiceCollection services, Action<TOptions> setup, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    //        where TProjection : class, IIdentity<TKey>
    //        where TOptions : class, new()
    //    {
    //        return services.Add<IQueryRepository<TProjection, TKey>, QueryRepository<TProjection, TKey>, TOptions>(lifetime, setup);
    //    }

    //    public static IServiceCollection AddQueryStore<TImplementation>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    //        where TImplementation : class, IQueryStore
    //    {
    //        return services.Add<IQueryStore, TImplementation>(lifetime);
    //    }

    //    public static IServiceCollection AddQueryStore<TImplementation, TOptions>(this IServiceCollection services, Action<TOptions> setup, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    //        where TImplementation : class, IQueryStore
    //        where TOptions : class, new()
    //    {
    //        return services.Add<IQueryStore, TImplementation, TOptions>(lifetime, setup);
    //    }
    //}
}
