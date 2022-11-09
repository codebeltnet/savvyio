using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Savvyio.Extensions.DependencyInjection.EFCore
{
    /// <summary>
    /// Configuration options for Microsoft Dependency Injection.
    /// </summary>
    /// <seealso cref="ServiceOptions"/>
    public class EfCoreServiceOptions : ServiceOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreServiceOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="EfCoreServiceOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="ServiceOptions.Lifetime"/></term>
        ///         <description><see cref="ServiceLifetime.Scoped"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public EfCoreServiceOptions()
        {
            Lifetime = ServiceLifetime.Scoped;
        }
    }
}
