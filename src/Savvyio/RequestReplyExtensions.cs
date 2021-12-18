using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savvyio
{
    public static class RequestReplyExtensions
    {
        public static void RegisterAsync<TModel, TResult>(this IRequestReplyRegistry<TModel> registry, Func<TModel, Task<TResult>> handler) where TModel : class, IRequestReply<TResult>
        {
            registry.RegisterAsync<TModel, TResult>((h, _) => handler(h));
        }
    }
}
