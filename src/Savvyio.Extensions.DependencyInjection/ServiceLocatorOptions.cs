using System;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Dispatchers;

namespace Savvyio.Extensions.DependencyInjection
{
    /// <summary>
    /// Configuration options for <see cref="IServiceLocator"/>.
    /// </summary>
    public class ServiceLocatorOptions : ServiceOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ServiceLocatorOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="ImplementationFactory"/></term>
        ///         <description><c>p => new ServiceLocator(p.GetServices)</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ServiceLocatorOptions()
        {
            ImplementationFactory = p => new ServiceLocator(p.GetServices);
        }

        /// <summary>
        /// Gets or sets the function delegate that creates an instance of an <see cref="IServiceLocator"/> implementation.
        /// </summary>
        /// <value>The function delegate that creates an instance of an <see cref="IServiceLocator"/> implementation.</value>
        public Func<IServiceProvider, IServiceLocator> ImplementationFactory { get; set; }
    }
}
