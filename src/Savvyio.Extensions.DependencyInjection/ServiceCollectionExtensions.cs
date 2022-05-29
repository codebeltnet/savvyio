using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;
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
        /// Adds an implementation of <see cref="IDataSource" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="IDataSource"/> to add.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TService"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IDataSource"/>
        /// <seealso cref="IDataSource{TMarker}"/>
        public static IServiceCollection AddDataSource<TService>(this IServiceCollection services, Action<ServiceOptions> setup = null) where TService : class, IDataSource
        {
            Validator.ThrowIfNull(services, nameof(services));
            var options = setup.Configure();
            services.TryAdd<TService, TService>(options.Lifetime);
            var hasMarkerType = typeof(TService).TryGetDependencyInjectionMarker(out _);
            var dataSourceGroupTypes = typeof(TService).GetInterfaces().Where(type => type.IsAssignableTo(typeof(IDataSource))).GroupBy(type => type.ToFriendlyName(o => o.ExcludeGenericArguments = true));
            foreach (var dataSourceGroupType in dataSourceGroupTypes)
            {
                services.TryAdd(hasMarkerType ? dataSourceGroupType.Last() : dataSourceGroupType.First(), p => p.GetRequiredService<TService>(), options.Lifetime);
            }
            return services;
        }

        /// <summary>
        /// Adds Savvy I/O service locator used to resolve necessary dependencies.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="ServiceLocatorOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddServiceLocator(this IServiceCollection services, Action<ServiceLocatorOptions> setup = null)
        {
            var options = setup.Configure();
            services.TryAdd(typeof(IServiceLocator), options.ImplementationFactory, options.Lifetime);
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
                AddHandlersWithOptionalDescriptors(services, handlerType, handlerTypeServices, descriptors, options);
            }

            if (options.IncludeHandlerServicesDescriptor)
            {
                services.AddSingleton(new HandlerServicesDescriptor(descriptors.Where(pair => pair.Key.HasInterfaces(options.HandlerServiceTypes.ToArray())).GroupBy(pair => pair.Key), options.HandlerServiceTypes));
            }

            AddDispatchers(services, options);

            return services.AddServiceLocator(o =>
            {
                o.ImplementationFactory = options.ServiceLocatorImplementationFactory;
                o.Lifetime = options.ServiceLocatorLifetime;
            });
        }

        private static void AddHandlersWithOptionalDescriptors(IServiceCollection services, Type handlerType, IEnumerable<Type> handlerTypeServices, Dictionary<Type, List<IHierarchy<object>>> descriptors, SavvyioDependencyInjectionOptions options)
        {
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

                AddDescriptors(handlers, handlerTypeService, descriptors, options);
            }
        }

        private static void AddDescriptors(Hierarchy<object> handlers, Type handlerTypeService, Dictionary<Type, List<IHierarchy<object>>> descriptors, SavvyioDependencyInjectionOptions options)
        {
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

        private static void AddDispatchers(IServiceCollection services, SavvyioDependencyInjectionOptions options)
        {
            foreach (var dispatcherType in options.DispatcherImplementationTypes)
            {
                var dispatcherTypeServices = dispatcherType.GetInterfaces().Where(type => type.HasInterfaces(options.DispatcherServiceTypes.ToArray()));
                foreach (var dispatcherTypeService in dispatcherTypeServices)
                {
                    services.TryAdd(dispatcherTypeService, dispatcherType, options.DispatcherServicesLifetime);
                }
            }
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
