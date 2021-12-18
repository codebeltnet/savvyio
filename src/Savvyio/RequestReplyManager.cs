using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace Savvyio
{
    internal class RequestReplyManager<TModel> : IRequestReplyRegistry<TModel>, IRequestReplyActivator<TModel>
    {
        private readonly ConcurrentDictionary<Type, Func<TModel, object>> _handlers = new();
        private readonly ConcurrentDictionary<Type, Func<TModel, CancellationToken, Task<object>>> _asyncHandlers = new();
        
        public RequestReplyManager()
        {
        }

        public void Register<T, TResult>(Func<T, TResult> handler) where T : class, TModel //, IRequestReply<TResult>
        {
            Validator.ThrowIfNull(handler, nameof(handler));
            _handlers.TryAdd(typeof(T), e => handler(e as T));
        }

        public void RegisterAsync<T, TResult>(Func<T, CancellationToken, Task<TResult>> handler) where T : class, TModel //, IRequestReply<TResult>
        {
            Validator.ThrowIfNull(handler, nameof(handler));
            _asyncHandlers.TryAdd(typeof(T), async (h, t) => await handler(h as T, t));
        }

        public bool TryInvoke<TResult>(TModel model, out TResult result)
        {
            Validator.ThrowIfNull(model, nameof(model));
            if (_handlers.TryGetValue(model.GetType(), out var handler))
            {
                result = (TResult)handler(model);
                return true;
            }
            result = default;
            return false;
        }

        public async Task<ConditionalValue<TResult>> TryInvokeAsync<TResult>(TModel model, CancellationToken ct = default)
        {
            Validator.ThrowIfNull(model, nameof(model));
            if (_asyncHandlers.TryGetValue(model.GetType(), out var handler))
            {
                var result = await handler(model, ct).ConfigureAwait(false);
                return new SuccessfulValue<TResult>((TResult)result);
            }
            return new UnsuccessfulValue<TResult>();
        }
    }
}
