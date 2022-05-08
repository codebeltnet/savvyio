using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Savvyio.Dispatchers;

namespace Savvyio.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an implementation of <see cref="IDataStore" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TDataStore">The type of the <see cref="IDataStore"/> interface to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDataStore"/>
        public static IServiceCollection AddDataStore<TDataStore, TImplementation>(this IServiceCollection services)
            where TDataStore : IDataStore
            where TImplementation : class, TDataStore
        {
            return services.AddDataStore<TImplementation>();
        }

        /// <summary>
        /// Adds an implementation of <see cref="IDataStore" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TDataStore">The type of the <see cref="IDataStore"/> interface to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the configured implementation to use.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IDataStore{TMarker}"/>
        public static IServiceCollection AddDataStore<TDataStore, TImplementation, TMarker>(this IServiceCollection services)
            where TDataStore : IDataStore<TMarker>
            where TImplementation : class, TDataStore
        {
            return services.AddDataStore<TImplementation>();
        }

        /// <summary>
        /// Adds an implementation of <see cref="IDataStore" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the configured implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TImplementation"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IDataStore"/>
        /// <seealso cref="IDataStore{TMarker}"/>
        public static IServiceCollection AddDataStore<TImplementation>(this IServiceCollection services)
            where TImplementation : class, IDataStore
        {
            Validator.ThrowIfNull(services, nameof(services));
            var dataStoreType = typeof(IDataStore);
            services.TryAddScoped<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                dataStoreType = typeof(IDataStore<>).MakeGenericType(markerType);
            }
            services.TryAddScoped(dataStoreType, p => p.GetRequiredService<TImplementation>());
            return services;
        }

        /// <summary>
        /// Adds Savvy I/O service locator used to resolve necessary dependencies.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddServiceLocator(this IServiceCollection services)
        {
            services.TryAddScoped<IServiceLocator>(p => new ServiceLocator(p.GetServices));
            return services;
        }

        /// <summary>
        /// Adds Savvy I/O related dispatcher- and handler- types to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="SavvyioDependencyInjectionOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddSavvyIO(this IServiceCollection services, Action<SavvyioDependencyInjectionOptions> setup = null)
        {
            var options = setup.Configure();
            if (options.AutomaticDispatcherDiscovery) { options.AddDispatchers(options.AssembliesToScan?.ToArray()); }
            if (options.AutomaticHandlerDiscovery) { options.AddHandlers(options.AssembliesToScan?.ToArray()); }
            var descriptors = new Dictionary<Type, List<IHierarchy<object>>>();
            foreach (var handlerType in options.HandlerImplementationTypes)
            {
                var handlerTypeServices = handlerType.GetInterfaces().Where(type => type.HasInterfaces(options.HandlerServiceTypes.ToArray()));
                foreach (var handlerTypeService in handlerTypeServices)
                {
                    var handlers = new Hierarchy<object>();
                    if (options.IncludeHandlerServicesDescriptor) { handlers.Add(handlerType); }

                    var handlerTypeInterfaceModel = handlerTypeService.GetInterface("IHandler`1")?.GenericTypeArguments.SingleOrDefault();
                    foreach (var method in handlerType.GetTypeInfo().DeclaredMembers.Where(m => MemberIsMethodOrLambdaWithHandlerTypeInterface(m, handlerTypeInterfaceModel)))
                    {
                        if (options.IncludeHandlerServicesDescriptor) { handlers.Add(method); }
                        if (!services.Any(sd => sd.ServiceType == handlerTypeService && sd.ImplementationType == handlerType))
                        {
                            services.Add(handlerTypeService, handlerType, options.HandlerServicesLifetime);
                        }
                    }

                    if (options.IncludeHandlerServicesDescriptor)
                    {
                        if (descriptors.TryGetValue(handlerTypeService, out var list))
                        {
                            list.Add(handlers);
                        }
                        else
                        {
                            descriptors.Add(handlerTypeService, new List<IHierarchy<object>>() { handlers });
                        }
                    }
                }
            }

            if (options.IncludeHandlerServicesDescriptor)
            {
                services.AddSingleton(new HandlerServicesDescriptor(descriptors.Where(pair => pair.Key.HasInterfaces(options.HandlerServiceTypes.ToArray())).GroupBy(pair => pair.Key), options.HandlerServiceTypes));
            }

            foreach (var dispatcherType in options.DispatcherImplementationTypes)
            {
                var dispatcherTypeServices = dispatcherType.GetInterfaces().Where(type => type.HasInterfaces(options.DispatcherServiceTypes.ToArray()));
                foreach (var dispatcherTypeService in dispatcherTypeServices)
                {
                    services.TryAdd(dispatcherTypeService, dispatcherType, options.DispatcherServicesLifetime);
                }
            }

            return services.AddServiceLocator();
        }

        private static bool MemberIsMethodOrLambdaWithHandlerTypeInterface(MemberInfo m, Type handlerTypeInterfaceModel)
        {
            switch (m.MemberType)
            {
                case MemberTypes.Method:
                    if (m is MethodInfo mi)
                    {
                        var parameters = mi.GetParameters();
                        return parameters.Any(p => p.ParameterType.HasInterfaces(handlerTypeInterfaceModel));
                    }
                    break;
                case MemberTypes.NestedType:
                    if (m is Type nt)
                    {
                        foreach (var nestedMethod in nt.GetRuntimeMethods())
                        {
                            var parameters = nestedMethod.GetParameters();
                            if (parameters.Any(p => p.ParameterType.HasInterfaces(handlerTypeInterfaceModel)))
                            {
                                return true;
                            }
                        }
                    }
                    break;
            }
            return false;
        }
    }
}
