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

        Assert.Equivalent(direct, microsoft, strict: true);
        Assert.Equivalent(direct, manual, strict: true);
        // Characterize the current separate eager instance before the Cuemon migration.
        Assert.NotSame(direct, microsoft);
        using var scope = provider.CreateScope();
        Assert.Same(direct, scope.ServiceProvider.GetRequiredService<TOptions>());
        Assert.Same(callback, scope.ServiceProvider.GetRequiredService<Action<TOptions>>());
    }
}
