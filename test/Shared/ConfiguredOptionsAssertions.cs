#nullable enable
using System;
using Cuemon.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Savvyio.Extensions.DependencyInjection;

internal static class ConfiguredOptionsAssertions
{
    internal static void Verify<TOptions>(IServiceProvider provider) where TOptions : class, IParameterObject, new()
    {
        var direct = provider.GetRequiredService<TOptions>();
        var microsoft = provider.GetRequiredService<IOptions<TOptions>>().Value;
        var callback = provider.GetRequiredService<Action<TOptions>>();
        var manual = new TOptions();
        callback(manual);

        Assert.Same(direct, microsoft);
        Assert.Equivalent(direct, manual, strict: true);
        using var scope = provider.CreateScope();
        Assert.Same(direct, scope.ServiceProvider.GetRequiredService<TOptions>());
        Assert.Same(callback, scope.ServiceProvider.GetRequiredService<Action<TOptions>>());
    }
}
