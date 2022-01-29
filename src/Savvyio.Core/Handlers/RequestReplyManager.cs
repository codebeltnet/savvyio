using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace Savvyio.Handlers
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

        public bool TryInvoke<TResult>(TModel request, out TResult result)
        {
            Validator.ThrowIfNull(request, nameof(request));
            if (_handlers.TryGetValue(request.GetType(), out var handler))
            {
                result = (TResult)handler(request);
                return true;
            }
            result = default;
            return false;
        }

        public async Task<ConditionalValue<TResult>> TryInvokeAsync<TResult>(TModel request, CancellationToken ct = default)
        {
            Validator.ThrowIfNull(request, nameof(request));
            if (_asyncHandlers.TryGetValue(request.GetType(), out var handler))
            {
                var result = await handler(request, ct).ConfigureAwait(false);
                return new SuccessfulValue<TResult>((TResult)result);
            }
            return new UnsuccessfulValue<TResult>();
        }
    }
}
