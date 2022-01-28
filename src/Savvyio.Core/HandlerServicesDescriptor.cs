using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cuemon;
using Cuemon.Extensions;

namespace Savvyio
{
    public class HandlerServicesDescriptor
    {
        public HandlerServicesDescriptor(IEnumerable<IGrouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>>> discoveredServices, IEnumerable<Type> serviceTypes)
        {
            DiscoveredServices = new List<IGrouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>>>(discoveredServices ?? Enumerable.Empty<IGrouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>>>());
            ServiceTypes = new List<Type>(serviceTypes ?? Enumerable.Empty<Type>()).OrderBy(type => type.Name).ToList();
        }

        public IReadOnlyList<IGrouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>>> DiscoveredServices { get; }

        public IReadOnlyList<Type> ServiceTypes { get; }

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
                    var handlers = discoveredServicesGroup.Where(pair => pair.Key == serviceType).SelectMany(pair => pair.Value).ToList(); // DiscoveredServices.Where(h => h.InstanceAs<Type>().HasInterfaces(serviceType)).Distinct().ToList();
                    if (handlers.Count == 0) { continue; }
                    var text = $"Discovered {handlers.Count} {serviceType.Name} implementations covering a total of {handlers.SelectMany(h => h.GetChildren()).Count()} {serviceRequestType.Name} methods";
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
            }
            return builder.ToString();
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
