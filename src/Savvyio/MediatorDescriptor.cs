using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cuemon;
using Cuemon.Extensions;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.Events;
using Savvyio.Queries;

namespace Savvyio
{
    public class MediatorDescriptor
    {
        public MediatorDescriptor(IList<IHierarchy<object>> discoveredCommandHandlers, IList<IHierarchy<object>> discoveredEventHandlers, IList<IHierarchy<object>> discoveredDomainEventHandlers, IList<IHierarchy<object>> discoveredQueryHandlers)
        {
            DiscoveredCommandHandlers = discoveredCommandHandlers ?? new List<IHierarchy<object>>();
            DiscoveredEventHandlers = discoveredEventHandlers ?? new List<IHierarchy<object>>();
            DiscoveredDomainEventHandlers = discoveredDomainEventHandlers ?? new List<IHierarchy<object>>();
            DiscoveredQueryHandlers = discoveredQueryHandlers ?? new List<IHierarchy<object>>();
        }

        public IList<IHierarchy<object>> DiscoveredCommandHandlers { get; }

        public IList<IHierarchy<object>> DiscoveredEventHandlers { get; }

        public IList<IHierarchy<object>> DiscoveredDomainEventHandlers { get; }

        public IList<IHierarchy<object>> DiscoveredQueryHandlers { get; }

        public override string ToString()
        {
            var chDis = $"Discovered {DiscoveredCommandHandlers.Count} {nameof(ICommandHandler)} implementations covering a total of {DiscoveredCommandHandlers.SelectMany(h => h.GetChildren()).Count()} {nameof(ICommand)} methods";
            var ehDis = $"Discovered {DiscoveredEventHandlers.Count} {nameof(IIntegrationEventHandler)} implementations covering a total of {DiscoveredEventHandlers.SelectMany(h => h.GetChildren()).Count()} {nameof(IIntegrationEvent)} methods";
            var dehDis = $"Discovered {DiscoveredDomainEventHandlers.Count} {nameof(IDomainEventHandler)} implementations covering a total of {DiscoveredDomainEventHandlers.SelectMany(h => h.GetChildren()).Count()} {nameof(IDomainEvent)} methods";
            var qhDis = $"Discovered {DiscoveredQueryHandlers.Count} {nameof(IQueryHandler)} implementations covering a total of {DiscoveredQueryHandlers.SelectMany(h => h.GetChildren()).Count()} {nameof(IQuery)} methods";
            var fixedString = Generate.FixedString('-', chDis.Length.Max(ehDis.Length.Max(dehDis.Length)));
            var builder = new StringBuilder(chDis);
            builder.AppendLine();
            foreach (var group in DiscoveredCommandHandlers.GroupBy(h =>
            {
                var ti = h.Instance.As<Type>();
                return new Tuple<string, string>(ti.Assembly.GetName().Name, ti.Namespace);
            })) { AppendHandler(builder, group, typeof(ICommand)); }
            builder.AppendLine();
            builder.AppendLine(fixedString);
            builder.AppendLine();
            builder.AppendLine(ehDis);
            foreach (var group in DiscoveredEventHandlers.GroupBy(h =>
            {
                var ti = h.Instance.As<Type>();
                return new Tuple<string, string>(ti.Assembly.GetName().Name, ti.Namespace);
            })) { AppendHandler(builder, group, typeof(IIntegrationEvent)); }
            builder.AppendLine();
            builder.AppendLine(fixedString);
            builder.AppendLine();
            builder.AppendLine(dehDis);
            foreach (var group in DiscoveredDomainEventHandlers.GroupBy(h =>
            {
                var ti = h.Instance.As<Type>();
                return new Tuple<string, string>(ti.Assembly.GetName().Name, ti.Namespace);
            })) { AppendHandler(builder, group, typeof(IDomainEvent)); }
            builder.AppendLine();
            builder.AppendLine(fixedString);
            builder.AppendLine();
            builder.AppendLine(qhDis);
            foreach (var group in DiscoveredQueryHandlers.GroupBy(h =>
            {
                var ti = h.Instance.As<Type>();
                return new Tuple<string, string>(ti.Assembly.GetName().Name, ti.Namespace);
            })) { AppendHandler(builder, group, typeof(IQuery)); }
            return builder.ToString();
        }

        private static void AppendHandler(StringBuilder builder, IGrouping<Tuple<string, string>, IHierarchy<object>> group, Type interfaceModelType)
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
                            var p = mi.GetParameters().Single(p => p.ParameterType.HasInterfaces(interfaceModelType));
                            builder.AppendLine($"\t*{p.ParameterType.Name} --> &{mi.Name}");
                        }
                    }
                    builder.AppendLine();
                }
            }
        }
    }
}
