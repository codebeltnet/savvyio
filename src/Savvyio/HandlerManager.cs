using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;

namespace Savvyio
{
    public class HandlerManager<TModel> : IHandlerRegistry<TModel>, IHandlerActivator<TModel>
    {
        private readonly ConcurrentDictionary<Type, Action<TModel>> _handlers = new();
        private readonly ConcurrentDictionary<Type, Func<TModel, CancellationToken, Task>> _asyncHandlers = new();

        public HandlerManager()
        {
        }

        public void Register<T>(Action<T> handler) where T : class, TModel
        {
            Validator.ThrowIfNull(handler, nameof(handler));
            _handlers.TryAdd(typeof(T), e => handler(e as T));
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

        public void RegisterAsync<T>(Func<T, Task> handler) where T : class, TModel
        {
            Validator.ThrowIfNull(handler, nameof(handler));
            _asyncHandlers.TryAdd(typeof(T), (h, _) => handler(h as T));
        }

        public void RegisterAsync<T>(Func<T, CancellationToken, Task> handler) where T : class, TModel
        {
            Validator.ThrowIfNull(handler, nameof(handler));
            _asyncHandlers.TryAdd(typeof(T), (h, t) => handler(h as T, t));
        }

        public async Task<bool> TryInvokeAsync(TModel model, CancellationToken ct = default)
        {
            Validator.ThrowIfNull(model, nameof(model));
            if (_asyncHandlers.TryGetValue(model.GetType(), out var handler))
            {
                await handler(model, ct).ConfigureAwait(false);
                return true;
            }
            return false;
        }
    }

    public static class HandlerManager
    {
        public static IHandlerActivator<T> Create<T>(Action<IHandlerRegistry<T>> handlerRegistrar)
        {
            Validator.ThrowIfNull(handlerRegistrar, nameof(handlerRegistrar));
            var handlerManager = new HandlerManager<T>();
            handlerRegistrar(handlerManager);
            return handlerManager;
        }
    }
}
