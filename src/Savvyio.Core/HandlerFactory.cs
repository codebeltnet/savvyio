using System;
using Cuemon;
using Savvyio.Handlers;

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

    public static IRequestReplyActivator<T> CreateRequestReply<T>(Action<IRequestReplyRegistry<T>> handlerRegistrar)
    {
        Validator.ThrowIfNull(handlerRegistrar, nameof(handlerRegistrar));
        var handlerManager = new RequestReplyManager<T>();
        handlerRegistrar(handlerManager);
        return handlerManager;
    }
}
