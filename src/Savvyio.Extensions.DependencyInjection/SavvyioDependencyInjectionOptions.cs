using System;
using System.Collections.Generic;
using System.Reflection;
using Cuemon.Extensions.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Dispatchers;

namespace Savvyio.Extensions.DependencyInjection
{
    /// <summary>
    /// Specifies options that is related to setting up Savvy I/O services.
    /// </summary>
    public class SavvyioDependencyInjectionOptions : SavvyioOptions
    {
        private readonly IList<Assembly> _assemblies = new List<Assembly>();
        private readonly ServiceLocatorOptions _serviceLocatorOptions = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SavvyioDependencyInjectionOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="SavvyioOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="SavvyioOptions.AutomaticDispatcherDiscovery"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SavvyioOptions.AutomaticHandlerDiscovery"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SavvyioOptions.IncludeHandlerServicesDescriptor"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ServiceLocatorLifetime"/></term>
        ///         <description><see cref="ServiceLifetime.Scoped"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ServiceLocatorImplementationFactory"/></term>
        ///         <description><c>p => new ServiceLocator(p.GetServices)</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="DispatcherServicesLifetime"/></term>
        ///         <description><see cref="ServiceLifetime.Scoped"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="HandlerServicesLifetime"/></term>
        ///         <description><see cref="ServiceLifetime.Transient"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AssembliesToScan"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public SavvyioDependencyInjectionOptions()
        {
            HandlerServicesLifetime = ServiceLifetime.Transient;
            DispatcherServicesLifetime = ServiceLifetime.Transient;
            ServiceLocatorImplementationFactory = _serviceLocatorOptions.ImplementationFactory;
            ServiceLocatorLifetime = _serviceLocatorOptions.Lifetime;
        }
        
        /// <summary>
        /// Gets or sets the function delegate that creates an instance of an <see cref="IServiceLocator"/> implementation.
        /// </summary>
        /// <value>The function delegate that creates an instance of an <see cref="IServiceLocator"/> implementation.</value>
        public Func<IServiceProvider, IServiceLocator> ServiceLocatorImplementationFactory { get; set; }

        /// <summary>
        /// Gets or sets the lifetime of the <see cref="IServiceLocator"/>.
        /// </summary>
        /// <value>The lifetime of the <see cref="IServiceLocator"/>.</value>
        public ServiceLifetime ServiceLocatorLifetime { get; set; }

        /// <summary>
        /// Gets or sets the lifetime of the handler services.
        /// </summary>
        /// <value>The lifetime of the handler services.</value>
        public ServiceLifetime HandlerServicesLifetime { get; set; }

        /// <summary>
        /// Gets or sets the lifetime of the dispatcher services.
        /// </summary>
        /// <value>The lifetime of the dispatcher services.</value>
        public ServiceLifetime DispatcherServicesLifetime { get; set; }

        /// <summary>
        /// Gets the assemblies to scan for dispatcher- and handler- types.
        /// </summary>
        /// <value>The assemblies to scan for dispatcher- and handler- types.</value>
        public IEnumerable<Assembly> AssembliesToScan => _assemblies.Count == 0 ? null : _assemblies;

        /// <summary>
        /// Adds the specified range of <paramref name="assemblies"/> to be included when scanning for dispatcher- and handler- types.
        /// </summary>
        /// <param name="assemblies">The assemblies to include in the scan.</param>
        /// <returns>A reference to this instance so that additional configuration calls can be chained.</returns>
        public SavvyioDependencyInjectionOptions AddAssemblyRangeToScan(params Assembly[] assemblies)
        {
            _assemblies.AddRange(assemblies);
            return this;
        }

        /// <summary>
        /// Adds the specified <paramref name="assembly"/> to be included when scanning for dispatcher- and handler- types.
        /// </summary>
        /// <param name="assembly">The assembly to include in the scan.</param>
        /// <returns>A reference to this instance so that additional configuration calls can be chained.</returns>
        public SavvyioDependencyInjectionOptions AddAssemblyToScan(Assembly assembly)
        {
            _assemblies.Add(assembly);
            return this;
        }
    }
}
