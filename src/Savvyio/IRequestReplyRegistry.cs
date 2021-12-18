using System;
using System.Threading;
using System.Threading.Tasks;

namespace Savvyio
{
    public interface IRequestReplyRegistry<in TModel>
    {
        void Register<T, TResult>(Func<T, TResult> handler) where T : class, TModel; //, IRequestReply<TResult>;

        void RegisterAsync<T, TResult>(Func<T, CancellationToken, Task<TResult>> handler) where T : class, TModel; //, IRequestReply<TResult>;
    }
}
