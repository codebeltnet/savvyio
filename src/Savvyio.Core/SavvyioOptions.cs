using System;
using System.Collections.Generic;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;
using Savvyio.Dispatchers;

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
        /// <typeparam name="T">The type of the service to validate.</typeparam>
        /// <param name="provider">The provider type.</param>
        /// <param name="contract">The contract type.</param>
        /// <returns><c>true</c> if the specified provider is a valid dependency; otherwise, <c>false</c>.</returns>
        /// <remarks>Dependency formula: <c>provider.HasInterfaces(contract) &amp;&amp; provider.IsClass &amp;&amp; !provider.IsAbstract &amp;&amp; !provider.IsInterface</c></remarks>
        public static bool IsValid<T>(Type provider, Type contract)
        {
            if (provider.HasInterfaces(typeof(T)))
            {
                return provider.HasInterfaces(contract) && provider.IsClass && !provider.IsAbstract && !provider.IsInterface;
            }
            return false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SavvyioOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="SavvyioOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="AutomaticDispatcherDiscovery"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AutomaticHandlerDiscovery"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="IncludeHandlerServicesDescriptor"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public SavvyioOptions()
        {
        }

        /// <summary>
        /// Gets a value indicating whether automatic discovery of <see cref="IHandler"/> implementations is enabled.
        /// </summary>
        /// <value><c>true</c> automatic discovery of <see cref="IHandler"/> implementations is enabled; otherwise, <c>false</c>.</value>
        public bool AutomaticHandlerDiscovery { get; private set; } = false;

        /// <summary>
        /// Gets a value indicating whether automatic discovery of <see cref="IDispatcher"/> implementations is enabled.
        /// </summary>
        /// <value><c>true</c> automatic discovery of <see cref="IDispatcher"/> implementations is enabled; otherwise, <c>false</c>.</value>
        public bool AutomaticDispatcherDiscovery { get; private set; } = false;


        /// <summary>
        /// Gets a value indicating whether the <see cref="HandlerServicesDescriptor"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if the <see cref="HandlerServicesDescriptor"/> is enabled; otherwise, <c>false</c>.</value>
        public bool IncludeHandlerServicesDescriptor { get; private set; } = false;

        /// <summary>
        /// Enables the automatic discovery of handlers implementing the <see cref="IHandler"/> interface.
        /// </summary>
        /// <param name="autoDiscoverHandlers"><c>true</c> if handlers implementing the <see cref="IHandler"/> interface should be automatically discovered; otherwise, <c>false</c>.</param>
        /// <returns>A reference to this instance so that additional configuration calls can be chained.</returns>
        public SavvyioOptions EnableAutomaticHandlerDiscovery(bool autoDiscoverHandlers = true)
        {
            AutomaticHandlerDiscovery = autoDiscoverHandlers;
            return this;
        }

        /// <summary>
        /// Enables the automatic discovery of handlers implementing the <see cref="IDispatcher"/> interface.
        /// </summary>
        /// <param name="autoDiscoverDispatchers"><c>true</c> if handlers implementing the <see cref="IDispatcher"/> interface should be automatically discovered; otherwise, <c>false</c>.</param>
        /// <returns>A reference to this instance so that additional configuration calls can be chained.</returns>
        public SavvyioOptions EnableAutomaticDispatcherDiscovery(bool autoDiscoverDispatchers = true)
        {
            AutomaticDispatcherDiscovery = autoDiscoverDispatchers;
            return this;
        }

        /// <summary>
        /// Enables the inclusion of the <see cref="HandlerServicesDescriptor"/> that provides an overview of all handlers presented in a detailed and developer friendly way.
        /// </summary>
        /// <param name="includeHandlerServicesDescriptor"><c>true</c> enables the inclusion of the <see cref="HandlerServicesDescriptor"/> that provides an overview of all handlers presented in a detailed and developer friendly way; otherwise, <c>false</c>.</param>
        /// <returns>A reference to this instance so that additional configuration calls can be chained.</returns>
        public SavvyioOptions EnableHandlerServicesDescriptor(bool includeHandlerServicesDescriptor = true)
        {
            IncludeHandlerServicesDescriptor = includeHandlerServicesDescriptor;
            return this;
        }

        /// <summary>
        /// Adds a dispatcher of type <typeparamref name="TDispatcher"/> to <see cref="DispatcherServiceTypes"/> (if not already registered) and <see cref="DispatcherImplementationTypes"/> (if not already registered).
        /// </summary>
        /// <typeparam name="TDispatcher">The type of the <see cref="IDispatcher"/> interface to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance so that additional configuration calls can be chained.</returns>
        public SavvyioOptions AddDispatcher<TDispatcher, TImplementation>()
            where TDispatcher : IDispatcher
            where TImplementation : class, TDispatcher
        {
            AddDispatcher(typeof(TDispatcher), typeof(TImplementation));
            return this;
        }

        /// <summary>
        /// Adds a dispatcher of type <see cref="IDispatcher"/> to <see cref="DispatcherServiceTypes"/> (if not already registered) and <see cref="DispatcherImplementationTypes"/> (if not already registered).
        /// </summary>
        /// <param name="service">The <see cref="Type"/> of the <see cref="IDispatcher"/> interface to add.</param>
        /// <param name="implementation">The type of the implementation to use.</param>
        /// <returns>A reference to this instance so that additional configuration calls can be chained.</returns>
        public SavvyioOptions AddDispatcher(Type service, Type implementation)
        {
            if (IsValid<IDispatcher>(implementation, service))
            {
                _dispatcherServiceTypes.TryAdd(service);
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
        /// <returns>A reference to this instance so that additional configuration calls can be chained.</returns>
        /// <remarks>Handler is only added if the following conditions are true: <c>typeof(THandler).HasInterfaces(typeof(IHandler&lt;TRequest&gt;)) &amp;&amp; typeof(T).IsClass &amp;&amp; !typeof(T).IsAbstract &amp;&amp; !typeof(T).IsInterface</c></remarks>
        public SavvyioOptions AddHandler<THandler, TRequest, TImplementation>()
            where THandler : IHandler<TRequest>
            where TRequest : IRequest
            where TImplementation : class, THandler
        {
            return AddHandler(typeof(THandler), typeof(TImplementation));
        }

        /// <summary>
        /// Adds a dispatcher of type <see cref="IHandler"/> to <see cref="HandlerServiceTypes"/> (if not already registered) and <see cref="HandlerImplementationTypes"/> (if not already registered).
        /// </summary>
        /// <param name="service">The <see cref="Type"/> of the <see cref="IHandler"/> interface to add.</param>
        /// <param name="implementation">The type of the implementation to use.</param>
        /// <returns>A reference to this instance so that additional configuration calls can be chained.</returns>
        public SavvyioOptions AddHandler(Type service, Type implementation)
        {
            if (IsValid<IHandler>(implementation, service))
            {
                _handlerServiceTypes.TryAdd(service);
                _handlerImplementationTypes.TryAdd(implementation);
            }
            return this;
        }

        /// <summary>
        /// Gets the types associated with an <see cref="IHandler"/> service.
        /// </summary>
        /// <value>The types associated with an <see cref="IHandler"/> service.</value>
        public IReadOnlyCollection<Type> HandlerServiceTypes => _handlerServiceTypes;

        /// <summary>
        /// Gets the types implementing an <see cref="IHandler"/> service.
        /// </summary>
        /// <value>The types implementing an <see cref="IHandler"/> service.</value>
        public IReadOnlyCollection<Type> HandlerImplementationTypes => _handlerImplementationTypes;

        /// <summary>
        /// Gets the types associated with an <see cref="IDispatcher"/> service.
        /// </summary>
        /// <value>The types dispatcher with an <see cref="IDispatcher"/> service.</value>
        public IReadOnlyCollection<Type> DispatcherServiceTypes => _dispatcherServiceTypes;

        /// <summary>
        /// Gets the types implementing an <see cref="IDispatcher"/> service.
        /// </summary>
        /// <value>The types implementing an <see cref="IDispatcher"/> service.</value>
        public IReadOnlyCollection<Type> DispatcherImplementationTypes => _dispatcherImplementationTypes;
    }
}
