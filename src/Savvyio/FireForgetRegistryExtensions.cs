using System;
using System.Threading.Tasks;

namespace Savvyio
{
    public static class FireForgetRegistryExtensions
    {
        public static void RegisterAsync<TModel>(this IFireForgetRegistry<TModel> registry, Func<TModel, Task> handler) where TModel : class
        {
            registry.RegisterAsync<TModel>((h, _) => handler(h));
        }
    }
}
