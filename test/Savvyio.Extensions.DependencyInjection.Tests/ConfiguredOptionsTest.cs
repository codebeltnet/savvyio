#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Cuemon.Configuration;
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

        var error = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddConfiguredOptions<Options>(null!, _ => called = true));

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
        Assert.NotSame(direct, microsoft.Value);
        Assert.Equal(ServiceLifetime.Singleton, Assert.Single(services, descriptor => descriptor.ServiceType == typeof(Options)).Lifetime);
        Assert.Equal(ServiceLifetime.Singleton, Assert.Single(services, descriptor => descriptor.ServiceType == typeof(Action<Options>)).Lifetime);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldConfigureEagerlyAndAgainWhenMicrosoftOptionsAreMaterialized()
    {
        var services = new ServiceCollection();
        var invocations = 0;
        services.AddConfiguredOptions<Options>(options => options.Value = (++invocations).ToString());

        Assert.Equal(1, invocations);
        using var provider = services.BuildServiceProvider();
        Assert.Equal("1", provider.GetRequiredService<Options>().Value);
        Assert.Equal(1, invocations);
        var options = provider.GetRequiredService<IOptions<Options>>();
        Assert.Equal(1, invocations);
        Assert.Equal("2", options.Value.Value);
        Assert.Same(options.Value, options.Value);
        Assert.Equal(2, invocations);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldValidateEagerlyAndWrapInvalidStateWithoutRegisteringServices()
    {
        var services = new ServiceCollection();

        var error = Assert.Throws<ArgumentException>(() => services.AddConfiguredOptions<ValidatedOptions>(options => options.Value = "invalid"));

        Assert.Equal("setup", error.ParamName);
        Assert.IsType<InvalidOperationException>(error.InnerException);
        Assert.Empty(services);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldPropagateSetupFailureWithoutRegisteringServices()
    {
        var services = new ServiceCollection();
        var expected = new InvalidOperationException("Configuration failed.");

        var actual = Record.Exception(() => services.AddConfiguredOptions<Options>(_ => throw expected));

        Assert.Same(expected, actual);
        Assert.Empty(services);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldPostConfigureThenValidateOnlyTheEagerInstance()
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
        Assert.Equal(new[] { "configure" }, microsoft.Stages);
        Assert.Equal(new[] { "configure" }, manual.Stages);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldAppendRepeatedRegistrationsAndComposeMicrosoftConfigurations()
    {
        var services = new ServiceCollection();
        Action<Options> first = options => options.Value += "first";
        Action<Options> second = options => options.Value += "second";
        services.AddConfiguredOptions(first);
        services.AddConfiguredOptions(second);
        using var provider = services.BuildServiceProvider();

        Assert.Equal(new[] { "first", "second" }, provider.GetServices<Options>().Select(options => options.Value));
        Assert.Equal("second", provider.GetRequiredService<Options>().Value);
        Assert.Same(second, provider.GetRequiredService<Action<Options>>());
        Assert.Equal(new[] { first, second }, provider.GetServices<Action<Options>>());
        Assert.Equal("firstsecond", provider.GetRequiredService<IOptions<Options>>().Value.Value);
    }

    [Fact]
    public void AddConfiguredOptions_ShouldComposeMicrosoftConfigureAndPostConfigureWithoutChangingDirectOptions()
    {
        var services = new ServiceCollection();
        services.Configure<Options>(options => options.Value += "before;");
        services.AddConfiguredOptions<Options>(options => options.Value += "primary;");
        services.Configure<Options>(options => options.Value += "after;");
        services.PostConfigure<Options>(options => options.Value += "post;");
        using var provider = services.BuildServiceProvider();

        Assert.Equal("primary;", provider.GetRequiredService<Options>().Value);
        Assert.Equal("before;primary;after;post;", provider.GetRequiredService<IOptions<Options>>().Value.Value);
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
