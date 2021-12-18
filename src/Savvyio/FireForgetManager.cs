using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace Savvyio
{
    internal class FireForgetManager<TModel> : IFireForgetRegistry<TModel>, IFireForgetActivator<TModel>
    {
        private readonly ConcurrentDictionary<Type, Action<TModel>> _handlers = new();
        private readonly ConcurrentDictionary<Type, Func<TModel, CancellationToken, Task>> _asyncHandlers = new();

        public FireForgetManager()
        {
        }

        public void Register<T>(Action<T> handler) where T : class, TModel
        {
            Validator.ThrowIfNull(handler, nameof(handler));
            _handlers.TryAdd(typeof(T), e => handler(e as T));
        }

        public void RegisterAsync<T>(Func<T, CancellationToken, Task> handler) where T : class, TModel
        {
            Validator.ThrowIfNull(handler, nameof(handler));
            _asyncHandlers.TryAdd(typeof(T), async (h, t) => await handler(h as T, t));
        }

        public bool TryInvoke(TModel model)
        {
            Validator.ThrowIfNull(model, nameof(model));
            if (_handlers.TryGetValue(model.GetType(), out var handler))
            {
                handler.Invoke(model);
                return true;
            }
            return false;
        }

        public async Task<ConditionalValue> TryInvokeAsync(TModel model, CancellationToken ct = default)
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
