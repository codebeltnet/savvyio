using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.Events;
using Savvyio.Queries;

namespace Savvyio
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSavvyioMediator(this IServiceCollection services, Action<MediatorRegistry> handlersInitializer, Action<MediatorOptions> setup = null)
        {
            Validator.ThrowIfNull(handlersInitializer, nameof(handlersInitializer));
            var handlers = handlersInitializer.Configure();
            return AddMediatorCore(services, handlers.HandlerTypes, setup);
        }

        private static IServiceCollection AddMediatorCore(IServiceCollection services, IEnumerable<Type> handlerTypes, Action<MediatorOptions> setup)
        {
            Validator.ThrowIfNull(services, nameof(services));
            var options = setup.Configure();
            services.AddScoped<Func<Type, IEnumerable<object>>>(p => p.GetServices);
            var descriptors = new Dictionary<Type, List<IHierarchy<object>>>();
            foreach (var handlerType in handlerTypes)
            {
                var handlerTypeInterfaces = handlerType.GetInterfaces().Where(type => type.HasInterfaces(typeof(ICommandHandler), typeof(IIntegrationEventHandler), typeof(IDomainEventHandler), typeof(IQueryHandler)));
                foreach (var handlerTypeInterface in handlerTypeInterfaces)
                {
                    var handlers = new Hierarchy<object>();
                    if (options.IncludeMediatorDescriptor) { handlers.Add(handlerType); }

                    var handlerTypeInterfaceModel = handlerTypeInterface.GetInterface("IHandler`1")?.GenericTypeArguments.SingleOrDefault();
                    foreach (var method in handlerType.GetTypeInfo().DeclaredMethods.Where(m =>
                    {
                        var parameters = m.GetParameters();
                        return parameters.Any(p => p.ParameterType.HasInterfaces(handlerTypeInterfaceModel));
                    }))
                    {
                        if (options.IncludeMediatorDescriptor) { handlers.Add(method); }
                        if (!services.Any(sd => sd.ServiceType == handlerTypeInterface && sd.ImplementationType == handlerType))
                        {
                            services.Add(handlerTypeInterface, handlerType, options.HandlersLifetime);
                        }
                    }

                    if (options.IncludeMediatorDescriptor)
                    {
                        if (descriptors.TryGetValue(handlerTypeInterface, out var list))
                        {
                            list.Add(handlers);
                        }
                        else
                        {
                            descriptors.Add(handlerTypeInterface, new List<IHierarchy<object>>() { handlers });
                        }
                    }   
                }
            }
            if (options.IncludeMediatorDescriptor)
            {
                services.AddSingleton(new MediatorDescriptor(
                    descriptors.Where(pair => pair.Key.HasInterfaces(typeof(ICommandHandler))).SelectMany(pair => pair.Value).ToList(),
                    descriptors.Where(pair => pair.Key.HasInterfaces(typeof(IIntegrationEventHandler))).SelectMany(pair => pair.Value).ToList(), 
                    descriptors.Where(pair => pair.Key.HasInterfaces(typeof(IDomainEventHandler))).SelectMany(pair => pair.Value).ToList(),
                    descriptors.Where(pair => pair.Key.HasInterfaces(typeof(IQueryHandler))).SelectMany(pair => pair.Value).ToList()));
            }
            services.Add(typeof(IMediator), options.MediatorImplementationType, options.MediatorLifetime);
            return services;
        }
    }
}
