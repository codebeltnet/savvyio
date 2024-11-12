using Cuemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Extensions;
using Cuemon.Extensions.Runtime;

namespace Savvyio
{
    /// <summary>
    /// Represents a model for handler services discovery.
    /// </summary>
    public record HandlerDiscoveryModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerDiscoveryModel"/> class.
        /// </summary>
        /// <param name="handlerAbstractionType">The <see cref="Type"/> of the handler abstraction.</param>
        /// <param name="delegateAbstractionType">The <see cref="Type"/> of the delegate abstraction.</param>
        /// <param name="groupHandlers">The implementations of the <see cref="IHandler{TRequest}"/> interface.</param>
        public HandlerDiscoveryModel(Type handlerAbstractionType, Type delegateAbstractionType, IGrouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>> groupHandlers)
        {
            Validator.ThrowIfNull(handlerAbstractionType);
            Validator.ThrowIfNull(delegateAbstractionType);
            Validator.ThrowIfNull(groupHandlers);

            var handlers = groupHandlers.Where(pair => pair.Key == handlerAbstractionType).SelectMany(pair => pair.Value).ToList();
            if (handlers.Count == 0) { return; }

            var handlersDelegatesCount = 0;

            var assemblyModels = new List<HandlerServiceAssemblyModel>();
            foreach (var group in handlers.GroupBy(h => new HandlerServiceAssemblyModel(h.Instance.As<Type>())))
            {
                var delegateCount = 0;

                var implementations = new List<HandlerServiceTypeImplementationModel>();
                foreach (var node in group)
                {
                    if (node.Instance is Type handlerImplementationType)
                    {
                        var implementation = new HandlerServiceTypeImplementationModel(node, handlerImplementationType, delegateAbstractionType);
                        delegateCount += implementation.DelegatesCount;
                        implementations.Add(implementation);
                    }
                }

                group.Key.Implementations = implementations;

                handlersDelegatesCount += delegateCount;
                assemblyModels.Add(group.Key);
            }

            ImplementationsCount = handlers.Count;
            DelegatesCount = handlersDelegatesCount;
            AbstractionType = handlerAbstractionType.Name;
            DelegateType = delegateAbstractionType.Name;
            Assemblies = assemblyModels;
        }

        /// <summary>
        /// Gets the name of the handler abstraction.
        /// </summary>
        public string AbstractionType { get; init; }

        /// <summary>
        /// Gets the count of handler implementations.
        /// </summary>
        public int ImplementationsCount { get; init; }

        /// <summary>
        /// Gets the name of the delegate abstraction.
        /// </summary>
        public string DelegateType { get; init; }

        /// <summary>
        /// Gets the count of handler implementations delegates.
        /// </summary>
        public int DelegatesCount { get; init; }

        /// <summary>
        /// Gets the assemblies containing the handler implementations.
        /// </summary>
        public IEnumerable<HandlerServiceAssemblyModel> Assemblies { get; init; }
    }

    /// <summary>
    /// Represents a model for handler service assemblies.
    /// </summary>
    public record HandlerServiceAssemblyModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerServiceAssemblyModel"/> class.
        /// </summary>
        /// <param name="handlerImplementationType">The <see cref="Type"/> of the handler implementation.</param>
        public HandlerServiceAssemblyModel(Type handlerImplementationType)
        {
            Validator.ThrowIfNull(handlerImplementationType);

            Name = handlerImplementationType.Assembly.GetName().Name;
            Namespace = handlerImplementationType.Namespace;
        }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the namespace of the handler implementation.
        /// </summary>
        public string Namespace { get; init; }

        /// <summary>
        /// Gets or sets the implementations of the handler service type.
        /// </summary>
        public IEnumerable<HandlerServiceTypeImplementationModel> Implementations { get; internal set; }
    }

    /// <summary>
    /// Represents a model for handler service type implementations.
    /// </summary>
    public record HandlerServiceTypeImplementationModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerServiceTypeImplementationModel"/> class.
        /// </summary>
        /// <param name="node">The hierarchy node.</param>
        /// <param name="handlerImplementationType">The <see cref="Type"/> of the handler implementation.</param>
        /// <param name="delegateAbstractionType">The <see cref="Type"/> of the delegate abstraction.</param>
        public HandlerServiceTypeImplementationModel(IHierarchy<object> node, Type handlerImplementationType, Type delegateAbstractionType)
        {
            Validator.ThrowIfNull(node);
            Validator.ThrowIfNull(handlerImplementationType);
            Validator.ThrowIfNull(delegateAbstractionType);

            var delegates = new List<HandlerServiceTypeImplementationDelegatesModel>();
            var children = node.GetChildren().ToList();
            foreach (var child in children.Select(h => h.Instance).OrderBy(h => h.As<MethodInfo>()?.Name, StringComparer.OrdinalIgnoreCase))
            {
                if (child is MethodInfo methodInfo)
                {
                    var p = methodInfo.GetParameters().Single(p => p.ParameterType.HasInterfaces(delegateAbstractionType));
                    delegates.Add(new HandlerServiceTypeImplementationDelegatesModel(p.ParameterType.Name, methodInfo.Name));
                    DelegatesCount++;
                }
                else if (child is Type runtimeType) // we will get here when no class dependencies is specified (e.g., isolated 'method')
                {
                    var nestedMethods = runtimeType.GetRuntimeMethods().Where(i => i.GetParameters().Any(p => p.ParameterType.HasInterfaces(delegateAbstractionType)));
                    foreach (var nested in nestedMethods)
                    {
                        var p = nested.GetParameters().Single();
                        delegates.Add(new HandlerServiceTypeImplementationDelegatesModel(p.ParameterType.Name, nested.Name));
                        DelegatesCount++;
                    }
                }
            }

            Name = handlerImplementationType.ToFriendlyName();
            Delegates = delegates;
        }

        /// <summary>
        /// Gets the name of the handler implementation.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the count of delegates for the handler implementation.
        /// </summary>
        internal int DelegatesCount { get; init; }

        /// <summary>
        /// Gets the delegates of the handler implementation.
        /// </summary>
        public IEnumerable<HandlerServiceTypeImplementationDelegatesModel> Delegates { get; init; }
    }

    /// <summary>
    /// Represents a model for handler service type implementation delegates.
    /// </summary>
    public record HandlerServiceTypeImplementationDelegatesModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerServiceTypeImplementationDelegatesModel"/> class.
        /// </summary>
        /// <param type="type">Type of the delegate model.</param>
        /// <param type="handler">Type of the delegate method.</param>
        public HandlerServiceTypeImplementationDelegatesModel(string type, string handler)
        {
            Validator.ThrowIfNull(type);
            Validator.ThrowIfNull(handler);

            Type = type;
            Handler = handler;
        }

        /// <summary>
        /// Gets the name of the delegate model.
        /// </summary>
        public string Type { get; init; }

        /// <summary>
        /// Gets the name of the delegate method.
        /// </summary>
        public string Handler { get; init; }
    }
}
