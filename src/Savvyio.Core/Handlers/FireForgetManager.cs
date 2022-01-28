using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace Savvyio.Handlers
{
    internal class FireForgetManager<TRequest> : IFireForgetRegistry<TRequest>, IFireForgetActivator<TRequest>
    {
        private readonly ConcurrentDictionary<Type, Action<TRequest>> _handlers = new();
        private readonly ConcurrentDictionary<Type, Func<TRequest, CancellationToken, Task>> _asyncHandlers = new();

        public FireForgetManager()
        {
        }

        public void Register<T>(Action<T> handler) where T : class, TRequest
        {
            Validator.ThrowIfNull(handler, nameof(handler));
            _handlers.TryAdd(typeof(T), e => handler(e as T));
        }

        public void RegisterAsync<T>(Func<T, CancellationToken, Task> handler) where T : class, TRequest
        {
            Validator.ThrowIfNull(handler, nameof(handler));
            _asyncHandlers.TryAdd(typeof(T), async (h, t) => await handler(h as T, t));
        }

        public bool TryInvoke(TRequest model)
        {
            Validator.ThrowIfNull(model, nameof(model));
            if (_handlers.TryGetValue(model.GetType(), out var handler))
            {
                handler.Invoke(model);
                return true;
            }
            return false;
        }

        public async Task<ConditionalValue> TryInvokeAsync(TRequest model, CancellationToken ct = default)
        {
            Validator.ThrowIfNull(model, nameof(model));
            if (_asyncHandlers.TryGetValue(model.GetType(), out var handler))
            {
                await handler(model, ct).ConfigureAwait(false);
                return new SuccessfulValue();
            }
            return new UnsuccessfulValue();
        }
    }
}
