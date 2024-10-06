using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cuemon;
using Cuemon.Extensions;

namespace Savvyio
{
    /// <summary>
    /// Provides information, in a developer friendly way, about implementations of the <see cref="IHandler{TRequest}"/> interface such as name, declared members and what type of request they handle.
    /// </summary>
    /// <remarks>
    /// An example of the output available when calling <see cref="ToString"/>:<br/>
    /// <br/>
    /// Discovered 1 ICommandHandler implementation covering a total of 5 ICommand methods<br/>
    /// <br/>
    /// <br/>
    /// Assembly: Savvyio.Assets.Tests<br/>
    /// Namespace: Savvyio.Assets<br/>
    /// <br/>
    /// &lt;AccountCommandHandler&gt;<br/>
    ///    *UpdateAccount --> &amp;&lt;RegisterDelegates&gt;b__5_0<br/>
    ///    *CreateAccount --> &amp;CreateAccountAsync<br/>
    /// </remarks>>
    public class HandlerServicesDescriptor : IHandlerServicesDescriptor
    {
        private List<HandlerDiscoveryModel> _models;
        private readonly object _locker = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerServicesDescriptor"/> class.
        /// </summary>
        /// <param name="discoveredServices">The discovered implementations of the <see cref="IHandler{TRequest}"/> interface.</param>
        /// <param name="serviceTypes">The registered <see cref="IHandler{TRequest}"/> service types.</param>
        public HandlerServicesDescriptor(IEnumerable<IGrouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>>> discoveredServices, IEnumerable<Type> serviceTypes)
        {
            DiscoveredServices = new List<IGrouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>>>(discoveredServices ?? Enumerable.Empty<IGrouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>>>());
            ServiceTypes = new List<Type>(serviceTypes ?? Enumerable.Empty<Type>()).OrderBy(type => type.Name).ToList();
        }

        private IReadOnlyList<IGrouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>>> DiscoveredServices { get; }

        private IReadOnlyList<Type> ServiceTypes { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var model in GenerateHandlerDiscoveries())
            {
                var discovery = $"Discovered {model.ImplementationsCount} {model.AbstractionType} implementation{(model.ImplementationsCount > 1 ? "s" : "")} covering a total of {model.DelegatesCount} {model.DelegateType} method{(model.DelegatesCount > 1 ? "s" : "")}";
                builder.Append(discovery);
                builder.AppendLine();
                foreach (var assembly in model.Assemblies)
                {
                    builder.AppendLine();
                    builder.AppendLine($"Assembly: {assembly.Name}");
                    builder.AppendLine($"Namespace: {assembly.Namespace}");
                    builder.AppendLine();

                    foreach (var implementation in assembly.Implementations)
                    {
                        builder.AppendLine($"<{implementation.Name}>");
                        foreach (var @delegate in implementation.Delegates)
                        {
                            builder.AppendLine($"\t*{@delegate.Type} --> &{@delegate.Handler}");
                        }
                        builder.AppendLine();
                    }
                }
                builder.AppendLine(Generate.FixedString('-', discovery.Length.Max(discovery.Length.Max(discovery.Length))));
                builder.AppendLine();
            }
            return builder.ToString().TrimEnd();
        }

        /// <summary>
        /// Generates the handler discoveries.
        /// </summary>
        /// <returns>A collection of <see cref="HandlerDiscoveryModel" /> representing the handler discoveries.</returns>
        public IEnumerable<HandlerDiscoveryModel> GenerateHandlerDiscoveries()
        {
            if (_models == null)
            {
                lock (_locker)
                {
                    if (_models == null)
                    {
                        _models = new List<HandlerDiscoveryModel>();
                        foreach (var serviceType in ServiceTypes)
                        {
                            var serviceRequestType = serviceType.GetInterface("IHandler`1")?.GenericTypeArguments.Single();
                            if (serviceRequestType == null) { continue; }
                            foreach (var discoveredServicesGroup in DiscoveredServices)
                            {
                                var model = new HandlerDiscoveryModel(serviceType, serviceRequestType, discoveredServicesGroup);
                                if (model.ImplementationsCount > 0)
                                {
                                    _models.Add(model);
                                }
                            }
                        }
                    }
                }
            }
            return _models;
        }
    }
}
