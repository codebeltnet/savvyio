using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Savvyio.Storage
{
    /// <summary>
    /// Specifies options that is related to setting up Savvy I/O services.
    /// </summary>
    public class SavvyioDependencyInjectionOptions : SavvyioOptions
    {
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
        ///         <term><see cref="SavvyioOptions.AutoResolveDispatchers"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SavvyioOptions.AutoResolveHandlers"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SavvyioOptions.IncludeHandlerServicesDescriptor"/></term>
        ///         <description><c>false</c></description>
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
            DispatcherServicesLifetime = ServiceLifetime.Scoped;
        }
        
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
        /// Gets or sets the assemblies to scan for dispatcher- and handler- types.
        /// </summary>
        /// <value>The assemblies to scan for dispatcher- and handler- types.</value>
        public IEnumerable<Assembly> AssembliesToScan { get; set; }
    }
}
