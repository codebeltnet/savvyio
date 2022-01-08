using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Savvyio
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSavvyIO(this IServiceCollection services, Action<SavvyioOptions> setup)
        {
            services.AddScoped<Func<Type, IEnumerable<object>>>(p => p.GetServices);
            var options = setup.Configure();
            if (options.AutoResolveDispatchers) { options.AddDispatchers(); }
            if (options.AutoResolveHandlers) { options.AddHandlers(); }
            var descriptors = new Dictionary<Type, List<IHierarchy<object>>>();
            foreach (var handlerType in options.HandlerImplementationTypes)
            {
                var handlerTypeServices = handlerType.GetInterfaces().Where(type => type.HasInterfaces(options.HandlerServiceTypes.ToArray()));
                foreach (var handlerTypeService in handlerTypeServices)
                {
                    var handlers = new Hierarchy<object>();
                    if (options.IncludeServicesDescriptor) { handlers.Add(handlerType); }

                    var handlerTypeInterfaceModel = handlerTypeService.GetInterface("IHandler`1")?.GenericTypeArguments.SingleOrDefault();
                    foreach (var method in handlerType.GetTypeInfo().DeclaredMethods.Where(m =>
                    {
                        var parameters = m.GetParameters();
                        return parameters.Any(p => p.ParameterType.HasInterfaces(handlerTypeInterfaceModel));
                    }))
                    {
                        if (options.IncludeServicesDescriptor) { handlers.Add(method); }
                        if (!services.Any(sd => sd.ServiceType == handlerTypeService && sd.ImplementationType == handlerType))
                        {
                            services.Add(handlerTypeService, handlerType, options.ServicesLifetime);
                        }
                    }

                    if (options.IncludeServicesDescriptor)
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

            if (options.IncludeServicesDescriptor)
            {
                services.AddSingleton(new SavvyioServiceDescriptor(descriptors.Where(pair => pair.Key.HasInterfaces(options.HandlerServiceTypes.ToArray())).GroupBy(pair => pair.Key), options.HandlerServiceTypes));
            }

            foreach (var dispatcherType in options.DispatcherImplementationTypes)
            {
                var dispatcherTypeServices = dispatcherType.GetInterfaces().Where(type => type.HasInterfaces(options.DispatcherServiceTypes.ToArray()));
                foreach (var dispatcherTypeService in dispatcherTypeServices)
                {
                    services.Add(dispatcherTypeService, dispatcherType, options.DispatchersLifetime);
                }
            }

            return services;
        }
    }
}
