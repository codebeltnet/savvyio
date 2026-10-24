#nullable enable
using System;
using System.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Cuemon.Configuration;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Savvyio.Extensions.DependencyInjection;

public class ConfiguredOptionsTest : Test
{
    public ConfiguredOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddConfiguredOptions_ShouldRejectNullServicesBeforeInvokingSetup()
    {
        var called = false;

        var error = Assert.Throws<ArgumentNullException>(() => Cuemon.Extensions.DependencyInjection.ServiceCollectionExtensions.AddConfiguredOptions<Options>(null!, _ => called = true));

        Assert.Equal("services", error.ParamName);
        Assert.False(called);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldRejectNullSetupWithoutRegisteringServices()
    {
        var services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() => services.AddConfiguredOptions<Options>(null!));

        Assert.Empty(services);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldReturnSameCollectionAndRegisterThreeConsumptionForms()
    {
        var services = new ServiceCollection();
        Action<Options> setup = options => options.Value = "configured";

        Assert.Same(services, services.AddConfiguredOptions(setup));
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var direct = provider.GetRequiredService<Options>();
        var microsoft = provider.GetRequiredService<IOptions<Options>>();
        var callback = provider.GetRequiredService<Action<Options>>();
        var manual = new Options();
        callback(manual);

        Assert.Equal("configured", direct.Value);
        Assert.Equal(direct.Value, microsoft.Value.Value);
        Assert.Equal(direct.Value, manual.Value);
        Assert.Same(setup, callback);
        Assert.Same(direct, scope.ServiceProvider.GetRequiredService<Options>());
        Assert.Same(callback, scope.ServiceProvider.GetRequiredService<Action<Options>>());
        Assert.Same(microsoft, scope.ServiceProvider.GetRequiredService<IOptions<Options>>());
        Assert.Same(direct, microsoft.Value);
        Assert.Equal(ServiceLifetime.Singleton, Assert.Single(services, descriptor => descriptor.ServiceType == typeof(Options)).Lifetime);
        Assert.Equal(ServiceLifetime.Singleton, Assert.Single(services, descriptor => descriptor.ServiceType == typeof(Action<Options>)).Lifetime);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AddConfiguredOptions_ShouldConfigureOnceWhenDefaultOptionsAreMaterialized(bool resolveDirectFirst)
    {
        var services = new ServiceCollection();
        var invocations = 0;
        services.AddConfiguredOptions<Options>(options => options.Value = (++invocations).ToString());

        Assert.Equal(0, invocations);
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
        var options = provider.GetRequiredService<IOptions<Options>>();
        Assert.Equal(0, invocations);
        var first = resolveDirectFirst ? provider.GetRequiredService<Options>() : options.Value;
        var second = resolveDirectFirst ? options.Value : provider.GetRequiredService<Options>();

        Assert.Equal("1", first.Value);
        Assert.Same(first, second);
        Assert.Same(first, provider.GetRequiredService<Options>());
        Assert.Same(first, options.Value);
        Assert.Equal(1, invocations);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AddConfiguredOptions_ShouldReportInvalidStateWhenOptionsAreMaterialized(bool resolveDirect)
    {
        var services = new ServiceCollection();

        services.AddConfiguredOptions<ValidatedOptions>(options => options.Value = "invalid");
        using var provider = services.BuildServiceProvider();

        var error = Assert.Throws<OptionsValidationException>(() =>
        {
            if (resolveDirect) { provider.GetRequiredService<ValidatedOptions>(); }
            else { _ = provider.GetRequiredService<IOptions<ValidatedOptions>>().Value; }
        });

        Assert.Equal(Microsoft.Extensions.Options.Options.DefaultName, error.OptionsName);
        Assert.Equal(typeof(ValidatedOptions), error.OptionsType);
        Assert.Equal("Value must be valid.", Assert.Single(error.Failures));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AddConfiguredOptions_ShouldPropagateSetupFailureWhenOptionsAreMaterialized(bool resolveDirect)
    {
        var services = new ServiceCollection();
        var expected = new InvalidOperationException("Configuration failed.");

        services.AddConfiguredOptions<Options>(_ => throw expected);
        using var provider = services.BuildServiceProvider();

        var actual = Record.Exception(() =>
        {
            if (resolveDirect) { provider.GetRequiredService<Options>(); }
            else { _ = provider.GetRequiredService<IOptions<Options>>().Value; }
        });

        Assert.Same(expected, actual);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldPostConfigureThenValidateSharedOptionsAndExposeRawSetup()
    {
        var services = new ServiceCollection();
        services.AddConfiguredOptions<ValidatedOptions>(options =>
        {
            options.Value = "valid";
            options.Stages.Add("configure");
        });
        using var provider = services.BuildServiceProvider();

        var direct = provider.GetRequiredService<ValidatedOptions>();
        var microsoft = provider.GetRequiredService<IOptions<ValidatedOptions>>().Value;
        var manual = new ValidatedOptions();
        provider.GetRequiredService<Action<ValidatedOptions>>()(manual);

        Assert.Equal(new[] { "configure", "postconfigure", "validate" }, direct.Stages);
        Assert.Same(direct, microsoft);
        Assert.Equal(new[] { "configure" }, manual.Stages);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldKeepFirstSetupAndRegisterEachConsumptionFormOnce()
    {
        var services = new ServiceCollection();
        Action<Options> first = options => options.Value += "first";
        Action<Options> second = options => options.Value += "second";
        services.AddConfiguredOptions(first);
        services.AddConfiguredOptions(second);
        services.AddConfiguredOptions(first);
        using var provider = services.BuildServiceProvider();

        var direct = Assert.Single(provider.GetServices<Options>());
        Assert.Equal("first", direct.Value);
        Assert.Same(first, Assert.Single(provider.GetServices<Action<Options>>()));
        Assert.Same(direct, provider.GetRequiredService<IOptions<Options>>().Value);
        var manual = new Options();
        provider.GetRequiredService<Action<Options>>()(manual);
        Assert.Equal("first", manual.Value);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldComposeMicrosoftConfigureAndPostConfigureForSharedOptions()
    {
        var services = new ServiceCollection();
        services.Configure<Options>(options => options.Value += "before;");
        services.AddConfiguredOptions<Options>(options => options.Value += "primary;");
        services.Configure<Options>(options => options.Value += "after;");
        services.PostConfigure<Options>(options => options.Value += "post;");
        using var provider = services.BuildServiceProvider();

        var direct = provider.GetRequiredService<Options>();
        Assert.Equal("before;primary;after;post;", direct.Value);
        Assert.Same(direct, provider.GetRequiredService<IOptions<Options>>().Value);
        var manual = new Options();
        provider.GetRequiredService<Action<Options>>()(manual);
        Assert.Equal("primary;", manual.Value);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldPreserveSnapshotAndMonitorLifetimesAndNamedOptions()
    {
        var services = new ServiceCollection();
        services.AddConfiguredOptions<Options>(options => options.Value = "default");
        services.Configure<Options>("named", options => options.Value = "named");
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        using var firstScope = provider.CreateScope();
        using var secondScope = provider.CreateScope();
        var first = firstScope.ServiceProvider.GetRequiredService<IOptionsSnapshot<Options>>();
        var second = secondScope.ServiceProvider.GetRequiredService<IOptionsSnapshot<Options>>();
        var monitor = provider.GetRequiredService<IOptionsMonitor<Options>>();

        Assert.Same(first, firstScope.ServiceProvider.GetRequiredService<IOptionsSnapshot<Options>>());
        Assert.NotSame(first, second);
        Assert.Same(first.Value, first.Value);
        Assert.NotSame(first.Value, second.Value);
        Assert.Equal("default", first.Value.Value);
        Assert.Equal("named", first.Get("named").Value);
        Assert.Equal("default", monitor.CurrentValue.Value);
        Assert.Equal("named", monitor.Get("named").Value);
        Assert.Same(monitor, secondScope.ServiceProvider.GetRequiredService<IOptionsMonitor<Options>>());
        Assert.NotSame(provider.GetRequiredService<Options>(), monitor.CurrentValue);
    }

    private sealed class Options : IParameterObject
    {
        public string Value { get; set; } = string.Empty;
    }

    private sealed class ValidatedOptions : IPostConfigurableParameterObject, IValidatableParameterObject
    {
        public string Value { get; set; } = string.Empty;

        public List<string> Stages { get; } = [];

        public void PostConfigureOptions() => Stages.Add("postconfigure");

        public void ValidateOptions()
        {
            Stages.Add("validate");
            if (Value != "valid") { throw new InvalidOperationException("Value must be valid."); }
        }
    }
}
