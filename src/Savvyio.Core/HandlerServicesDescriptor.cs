using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public class HandlerServicesDescriptor
    {
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
            foreach (var serviceType in ServiceTypes)
            {
                var serviceRequestType = serviceType.GetInterface("IHandler`1")?.GenericTypeArguments.Single();
                if (serviceRequestType == null) { continue; }
                foreach (var discoveredServicesGroup in DiscoveredServices)
                {
                    AppendServices(builder, serviceType, serviceRequestType, discoveredServicesGroup);
                }
            }
            return builder.ToString();
        }

        private static void AppendServices(StringBuilder builder, Type serviceType, Type serviceRequestType, IGrouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>> discoveredServicesGroup)
        {
            var handlers = discoveredServicesGroup.Where(pair => pair.Key == serviceType).SelectMany(pair => pair.Value).ToList();
            if (handlers.Count == 0) { return; }
            var handlerMethodsCount = handlers.SelectMany(h => h.GetChildren()).Count();
            var text = $"Discovered {handlers.Count} {serviceType.Name} implementation{(handlers.Count > 1 ? "s" : "")} covering a total of {handlerMethodsCount} {serviceRequestType.Name} method{(handlerMethodsCount > 1 ? "s" : "")}";
            var dashes = Generate.FixedString('-', text.Length.Max(text.Length.Max(text.Length)));
            builder.Append(text);
            builder.AppendLine();
            foreach (var group in handlers.GroupBy(h =>
                     {
                         var ti = h.Instance.As<Type>();
                         return new Tuple<string, string>(ti.Assembly.GetName().Name, ti.Namespace);
                     })) { AppendHandler(builder, group, serviceRequestType); }
            builder.AppendLine(dashes);
            builder.AppendLine();
        }

        private static void AppendHandler(StringBuilder builder, IGrouping<Tuple<string, string>, IHierarchy<object>> group, Type serviceRequestType)
        {
            builder.AppendLine();
            builder.AppendLine($"Assembly: {group.Key.Item1}");
            builder.AppendLine($"Namespace: {group.Key.Item2}");
            builder.AppendLine();
            foreach (var node in group)
            {
                if (node.Instance is Type ti)
                {
                    var children = node.GetChildren().ToList();
                    builder.AppendLine($"<{ti.ToFriendlyName()}>");
                    foreach (var child in children.OrderBy(h => h.Instance.As<MethodInfo>().Name, StringComparer.OrdinalIgnoreCase))
                    {
                        if (child.Instance is MethodInfo mi)
                        {
                            var p = mi.GetParameters().Single(p => p.ParameterType.HasInterfaces(serviceRequestType));
                            builder.AppendLine($"\t*{p.ParameterType.Name} --> &{mi.Name}");
                        }
                    }
                    builder.AppendLine();
                }
            }
        }
    }
}
