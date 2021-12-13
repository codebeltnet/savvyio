using System;
using System.Threading;
using System.Threading.Tasks;

namespace Savvyio
{
    public interface IResponseHandlerRegistry<in TModel>
    {
        void Register<T, TResult>(Func<T, TResult> handler) where T : class, TModel, IResponse<TResult>;

        void RegisterAsync<T, TResult>(Func<T, Task<TResult>> handler) where T : class, TModel, IResponse<TResult>;

        void RegisterAsync<T, TResult>(Func<T, CancellationToken, Task<TResult>> handler) where T : class, TModel, IResponse<TResult>;
    }
}
