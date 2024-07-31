using Savvyio.Commands;
using Savvyio.Dispatchers;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Queries;
using System.Linq;
using System.Reflection;
using Savvyio.Reflection;
using System.Runtime.CompilerServices;

namespace Savvyio.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="SavvyioOptions"/> class.
    /// </summary>
    public static class SavvyioOptionsExtensions
    {
        /// <summary>
        /// Adds an implementation of the <see cref="IMediator"/> interface.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded to: <see cref="ICommandDispatcher"/>, <see cref="IDomainEventDispatcher"/>, <see cref="IIntegrationEventDispatcher"/> and <see cref="IQueryDispatcher"/>.</remarks>
        public static SavvyioOptions AddMediator<TImplementation>(this SavvyioOptions options) where TImplementation : class, IMediator
        {
            options.AddDispatcher<IMediator, TImplementation>();
            options.AddDispatcher<ICommandDispatcher, TImplementation>();
            options.AddDispatcher<IDomainEventDispatcher, TImplementation>();
            options.AddDispatcher<IIntegrationEventDispatcher, TImplementation>();
            options.AddDispatcher<IQueryDispatcher, TImplementation>();
            return options;
        }

        /// <summary>
        /// Enforce automatic discovery of handlers implementing the <see cref="IDispatcher"/> interface using either <paramref name="bruteAssemblyScanning"/> or lightweight <see cref="Assembly.GetCallingAssembly"/>.
        /// </summary>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <param name="bruteAssemblyScanning"><c>true</c> to use brute assembly scanning for all <see cref="IDispatcher"/> interface implementations throughout the application domain; otherwise, <c>false</c>.</param>
        /// <returns>A reference to <paramref name="options"/> so that additional configuration calls can be chained.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static SavvyioOptions UseAutomaticDispatcherDiscovery(this SavvyioOptions options, bool bruteAssemblyScanning = false)
        {
            if (bruteAssemblyScanning)
            {
                options.AddDispatchers(AssemblyContext.CurrentDomainAssemblies.ToArray());
            }
            else
            {
                options.AddDispatchers(Assembly.GetCallingAssembly());
            }
            return options;
        }

        /// <summary>
        /// Enforce automatic discovery of dispatchers implementing the <see cref="IHandler"/> interface using either <paramref name="bruteAssemblyScanning"/> or lightweight <see cref="Assembly.GetCallingAssembly"/>.
        /// </summary>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <param name="bruteAssemblyScanning"><c>true</c> to use brute assembly scanning for all <see cref="IHandler"/> interface implementations throughout the application domain; otherwise, <c>false</c>.</param>
        /// <returns>A reference to <paramref name="options"/> so that additional configuration calls can be chained.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static SavvyioOptions UseAutomaticHandlerDiscovery(this SavvyioOptions options, bool bruteAssemblyScanning = false)
        {
            if (bruteAssemblyScanning)
            {
                options.AddHandlers(AssemblyContext.CurrentDomainAssemblies.ToArray());
            }
            else
            {
                options.AddHandlers(Assembly.GetCallingAssembly());
            }
            return options;

        }
    }
}
