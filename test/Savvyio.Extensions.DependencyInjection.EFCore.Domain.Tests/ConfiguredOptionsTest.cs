#nullable enable
using System;
using Codebelt.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Domain;
using Savvyio.Extensions.EFCore;
using Savvyio.Extensions.EFCore.Domain;
using Xunit;

namespace Savvyio.Extensions.DependencyInjection.EFCore.Domain;

public class ConfiguredOptionsTest : Test
{
    public ConfiguredOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddEfCoreAggregateDataSource_ShouldRegisterAllOptionsConsumptionForms()
    {
        var services = new ServiceCollection();
        services.AddSavvyIO(options => options.AddDomainEventDispatcher());

        Assert.Same(services, services.AddEfCoreAggregateDataSource(options =>
            options.ContextConfigurator = builder => builder.UseInMemoryDatabase("aggregate-options")));
        using var provider = services.BuildServiceProvider();

        ConfiguredOptionsAssertions.Verify<EfCoreDataSourceOptions>(provider);
        Assert.IsType<EfCoreAggregateDataSource>(provider.GetRequiredService<IEfCoreDataSource>());
    }

    [Fact]
    public void AddEfCoreAggregateDataSource_ShouldRegisterAllOptionsConsumptionFormsWithMarker()
    {
        var services = new ServiceCollection();
        services.AddSavvyIO(options => options.AddDomainEventDispatcher());

        Assert.Same(services, services.AddEfCoreAggregateDataSource<DbMarker>(options =>
            options.ContextConfigurator = builder => builder.UseInMemoryDatabase("aggregate-marker-options")));
        using var provider = services.BuildServiceProvider();

        ConfiguredOptionsAssertions.Verify<EfCoreDataSourceOptions<DbMarker>>(provider);
        Assert.IsType<EfCoreAggregateDataSource<DbMarker>>(provider.GetRequiredService<IEfCoreDataSource<DbMarker>>());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AddEfCoreAggregateDataSource_ShouldRejectNullSetupDuringRegistration(bool useMarker)
    {
        var services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() =>
        {
            if (useMarker) { services.AddEfCoreAggregateDataSource<DbMarker>(null!); }
            else { services.AddEfCoreAggregateDataSource(null!); }
        });
    }
}
