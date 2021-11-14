using Cuemon.Extensions;

namespace Savvyio.Commands
{
    public static class CommandExtensions
    {
        public static T SetCausationId<T>(this T model, string causationId) where T : ICommand
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.CausationId, causationId);
            return model;
        }

        public static T SetCorrelationId<T>(this T model, string correlationId) where T : ICommand
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.CorrelationId, correlationId);
            return model;
        }

        public static string GetCausationId<T>(this T model) where T : ICommand
        {
            return MetadataFactory.Get(model, MetadataDictionary.CausationId).As<string>();
        }
        public static string GetCorrelationId<T>(this T model) where T : ICommand
        {
            return MetadataFactory.Get(model, MetadataDictionary.CorrelationId).As<string>();
        }
    }
}
