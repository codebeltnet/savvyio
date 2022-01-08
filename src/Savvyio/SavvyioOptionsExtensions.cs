using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;

namespace Savvyio
{
    /// <summary>
    /// Extension methods for the <see cref="SavvyioOptions"/> class.
    /// </summary>
    public static class SavvyioOptionsExtensions
    {
        internal static SavvyioOptions AddDispatchers(this SavvyioOptions options, params Assembly[] assembliesToScan)
        {
            return AddDependenciesCore<IDispatcher>(options, action => options.ManageDispatcherServiceTypes(action), action => options.ManageDispatcherImplementationTypes(action), assembliesToScan);
        }

        internal static SavvyioOptions AddHandlers(this SavvyioOptions options, params Assembly[] assembliesToScan)
        {
            return AddDependenciesCore<IHandler>(options, action => options.ManageHandlerServiceTypes(action), action => options.ManageHandlerImplementationTypes(action), assembliesToScan);
        }

        private static SavvyioOptions AddDependenciesCore<TFilter>(SavvyioOptions options, Action<Action<IList<Type>>> serviceTypeManager, Action<Action<IList<Type>>> implementationTypeManager, params Assembly[] assembliesToScan)
        {
            if (assembliesToScan?.Length == 0) { assembliesToScan = null; }
            var definedTypes = (assembliesToScan ?? AppDomain.CurrentDomain.GetAssemblies().Except(typeof(SavvyioOptions).Assembly.Yield())).Distinct().SelectMany(assembly => assembly.DefinedTypes).Where(type => type.HasInterfaces(typeof(TFilter))).ToList();
            foreach (var contract in definedTypes.Where(type => type.IsInterface))
            {
                serviceTypeManager(list => list.TryAdd(contract));
                var filtered = definedTypes.Where(type => SavvyioOptions.IsValid(type, contract)).ToList();
                if (filtered.Count == 0) { continue; }
                foreach (var filteredType in filtered)
                {
                    implementationTypeManager(list => list.TryAdd(filteredType));
                }
            }
            return options;
        }
    }
}
