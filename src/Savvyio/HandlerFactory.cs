using System;
using Cuemon;

namespace Savvyio;

public static class HandlerFactory
{
    public static IFireForgetActivator<T> CreateFireForget<T>(Action<IFireForgetRegistry<T>> handlerRegistrar)
    {
        Validator.ThrowIfNull(handlerRegistrar, nameof(handlerRegistrar));
        var handlerManager = new FireForgetManager<T>();
        handlerRegistrar(handlerManager);
        return handlerManager;
    }
}
