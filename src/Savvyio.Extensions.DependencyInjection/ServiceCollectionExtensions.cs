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
            services.AddServiceLocator();
            var options = setup.Configure();
            if (options.AutoResolveDispatchers) { options.AddDispatchers(options.AssembliesToScan?.ToArray()); }
            if (options.AutoResolveHandlers) { options.AddHandlers(options.AssembliesToScan?.ToArray()); }
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

            return services;
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
