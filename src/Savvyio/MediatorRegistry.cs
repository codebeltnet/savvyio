using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Extensions;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.Events;
using Savvyio.Queries;

namespace Savvyio
{
    public class MediatorRegistry
    {
        private static readonly IEnumerable<Type> Abstractions = new [] { typeof(ICommandHandler), typeof(IIntegrationEventHandler), typeof(IDomainEventHandler), typeof(IQueryHandler) };
        private readonly List<Type> _handlerTypes = new();

        public MediatorRegistry AddHandlersFromCurrentDomain()
        {
            AddHandlers(AppDomain.CurrentDomain.GetAssemblies());
            return this;
        }

        public MediatorRegistry AddHandlers(params Assembly[] assemblies)
        {
            if (assemblies != null)
            {
                foreach (var abstraction in Abstractions)
                {
                    var filteredImplementations = assemblies.Distinct().SelectMany(assembly => assembly.DefinedTypes).Where(type => IsValidHandlerType(type, abstraction)).ToList();
                    if (filteredImplementations.Count == 0) { continue; }
                    foreach (var handlerType in filteredImplementations)
                    {
                        if (!_handlerTypes.Contains(handlerType))
                        {
                            _handlerTypes.Add(handlerType);
                        }
                    }
                }
            }
            return this;
        }

        public MediatorRegistry AddDomainEventHandler<T>() where T : IDomainEventHandler
        {
            var handlerType = typeof(T);
            if (IsValidHandlerType(handlerType, typeof(IDomainEventHandler)))
            {
                _handlerTypes.Add(handlerType);
            }
            return this;
        }

        public MediatorRegistry AddIntegrationEventHandler<T>() where T : IIntegrationEventHandler
        {
            var handlerType = typeof(T);
            if (IsValidHandlerType(handlerType, typeof(IIntegrationEventHandler)))
            {
                _handlerTypes.Add(handlerType);
            }
            return this;
        }

        public MediatorRegistry AddCommandHandler<T>() where T : ICommandHandler
        {
            var handlerType = typeof(T);
            if (IsValidHandlerType(handlerType, typeof(ICommandHandler)))
            {
                _handlerTypes.Add(handlerType);
            }
            return this;
        }

        public MediatorRegistry AddQueryHandler<T>() where T : IQueryHandler
        {
            var handlerType = typeof(T);
            if (IsValidHandlerType(handlerType, typeof(IQueryHandler)))
            {
                _handlerTypes.Add(handlerType);
            }
            return this;
        }

        private static bool IsValidHandlerType(Type type, Type abstraction)
        {
            return type.HasInterfaces(abstraction) && type.IsClass && !type.IsAbstract && !type.IsInterface;
        }

        public IReadOnlyList<Type> HandlerTypes => _handlerTypes;
    }
}
