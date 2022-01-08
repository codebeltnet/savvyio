using System;
using System.Collections.Generic;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Savvyio
{
    /// <summary>
    /// Specifies options that is related to setting up Savvy I/O services.
    /// </summary>
    public class SavvyioOptions
    {
        private readonly List<Type> _handlerServiceTypes = new(); // the handler service types to tie one or more handler implementation types
        private readonly List<Type> _handlerImplementationTypes = new(); // the handler implementation types to tie one or more handler service types

        private readonly List<Type> _dispatcherServiceTypes = new(); // the dispatchers service types to tie one or more dispatcher implementation types
        private readonly List<Type> _dispatcherImplementationTypes = new(); // the dispatcher implementation types to tie one or more dispatchers service types

        /// <summary>
        /// Determines if a given pair of types are a valid as a dependency.
        /// </summary>
        /// <param name="provider">The provider type.</param>
        /// <param name="contract">The contract type.</param>
        /// <returns><c>true</c> if the specified provider is a valid dependency; otherwise, <c>false</c>.</returns>
        /// <remarks>Dependency formula: <c>provider.HasInterfaces(contract) &amp;&amp; provider.IsClass &amp;&amp; !provider.IsAbstract &amp;&amp; !provider.IsInterface</c></remarks>
        public static bool IsValid(Type provider, Type contract)
        {
            return provider.HasInterfaces(contract) && provider.IsClass && !provider.IsAbstract && !provider.IsInterface;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SavvyioOptions"/> class.
        /// </summary>
        public SavvyioOptions()
        {
            AutoResolveHandlers = true;
            AutoResolveDispatchers = true;
            IncludeServicesDescriptor = false;
            ServicesLifetime = ServiceLifetime.Transient;
            DispatchersLifetime = ServiceLifetime.Scoped;
        }

        public SavvyioOptions AddDispatcher<TDispatcher, TImplementation>()
            where TDispatcher : IDispatcher
            where TImplementation : class, TDispatcher
        {
            AddDispatcher(typeof(TDispatcher), typeof(TImplementation));
            return this;
        }

        public SavvyioOptions AddDispatcher(Type dispatcher, Type implementation)
        {
            _dispatcherServiceTypes.TryAdd(dispatcher);
            if (IsValid(implementation, dispatcher))
            {
                _dispatcherImplementationTypes.TryAdd(implementation);
            }
            return this;
        }

        /// <summary>
        /// Adds a handler of type <typeparamref name="THandler"/> to <see cref="HandlerServiceTypes"/> (if not already registered) and <see cref="HandlerImplementationTypes"/> (if not already registered).
        /// </summary>
        /// <typeparam name="THandler">The type of the service to add.</typeparam>
        /// <typeparam name="TRequest">The type of the model to handle.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance.</returns>
        /// <remarks>Handler is only added if the following conditions are true: <c>typeof(THandler).HasInterfaces(typeof(IHandler&lt;TRequest&gt;)) &amp;&amp; typeof(T).IsClass &amp;&amp; !typeof(T).IsAbstract &amp;&amp; !typeof(T).IsInterface</c></remarks>
        public SavvyioOptions AddHandler<THandler, TRequest, TImplementation>()
            where THandler : IHandler<TRequest>
            where TRequest : IRequest
            where TImplementation : class, THandler
        {
            return AddHandler(typeof(THandler), typeof(TImplementation));
        }

        public SavvyioOptions AddHandler(Type handler, Type implementation)
        {
            _handlerServiceTypes.TryAdd(handler);
            if (IsValid(implementation, handler))
            {
                _handlerImplementationTypes.TryAdd(implementation);
            }
            return this;
        }

        internal SavvyioOptions ManageHandlerServiceTypes(Action<IList<Type>> configurator)
        {
            configurator?.Invoke(_handlerServiceTypes);
            return this;
        }

        internal SavvyioOptions ManageHandlerImplementationTypes(Action<IList<Type>> configurator)
        {
            configurator?.Invoke(_handlerImplementationTypes);
            return this;
        }

        internal SavvyioOptions ManageDispatcherServiceTypes(Action<IList<Type>> configurator)
        {
            configurator?.Invoke(_dispatcherServiceTypes);
            return this;
        }

        internal SavvyioOptions ManageDispatcherImplementationTypes(Action<IList<Type>> configurator)
        {
            configurator?.Invoke(_dispatcherImplementationTypes);
            return this;
        }

        public IReadOnlyCollection<Type> HandlerServiceTypes => _handlerServiceTypes;

        public IReadOnlyCollection<Type> HandlerImplementationTypes => _handlerImplementationTypes;

        public IReadOnlyCollection<Type> DispatcherServiceTypes => _dispatcherServiceTypes;

        public IReadOnlyCollection<Type> DispatcherImplementationTypes => _dispatcherImplementationTypes;

        public bool AutoResolveHandlers { get; set; }

        public bool AutoResolveDispatchers { get; set; }

        public bool IncludeServicesDescriptor { get; set; }

        public ServiceLifetime ServicesLifetime { get; set; }

        public ServiceLifetime DispatchersLifetime { get; set; }
    }
}
