using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;
using Savvyio.Dispatchers;

namespace Savvyio
{
    /// <summary>
    /// Extension methods for the <see cref="SavvyioOptions"/> class.
    /// </summary>
    public static class SavvyioOptionsExtensions
    {
        /// <summary>
        /// Adds handlers of type <see cref="IDispatcher"/> from the specified <paramref name="assemblies"/> to the extended <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <param name="assemblies">The assemblies to scan for <see cref="IDispatcher"/> implementations.</param>
        /// <returns>A reference to <paramref name="options"/> after the operation has completed.</returns>
        public static SavvyioOptions AddDispatchers(this SavvyioOptions options, params Assembly[] assemblies)
        {
            return AddDependenciesCore<IDispatcher>(options, (service, implementation) => options.AddDispatcher(service, implementation), assemblies);
        }

        /// <summary>
        /// Adds handlers of type <see cref="IHandler"/> from the specified <paramref name="assemblies"/> to the extended <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <param name="assemblies">The assemblies to scan for <see cref="IHandler"/> implementations.</param>
        /// <returns>A reference to <paramref name="options"/> after the operation has completed.</returns>
        public static SavvyioOptions AddHandlers(this SavvyioOptions options, params Assembly[] assemblies)
        {
            return AddDependenciesCore<IHandler>(options, (service, implementation) => options.AddHandler(service, implementation), assemblies);
        }

        private static SavvyioOptions AddDependenciesCore<T>(SavvyioOptions options, Action<Type, Type> servicesManager, IEnumerable<Assembly> assembliesToScan)
        {
            var definedTypes = (assembliesToScan ?? AppDomain.CurrentDomain.GetAssemblies().Except(typeof(SavvyioOptions).Assembly.Yield())).Distinct().SelectMany(assembly => assembly.DefinedTypes).Where(type => type.HasInterfaces(typeof(T))).ToList();
            foreach (var contract in definedTypes.Where(type => type.IsInterface))
            {
                var filtered = definedTypes.Where(type => SavvyioOptions.IsValid<T>(type, contract)).ToList();
                if (filtered.Count == 0) { continue; }
                foreach (var filteredType in filtered)
                {
                    servicesManager(contract, filteredType);
                }
            }
            return options;
        }
    }
}
