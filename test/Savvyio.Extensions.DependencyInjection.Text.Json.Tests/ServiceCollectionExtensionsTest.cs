using System;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Text.Json;
using Savvyio.Extensions.Text.Json;
using Xunit;

namespace Savvyio.Extensions.DependencyInjection.Text.Json.Tests
{
    public class ServiceCollectionExtensionsTest : Test
    {
        [Fact]
        public void AddJsonMarshaller_ShouldThrowOnNullServices()
        {
            // Arrange
            IServiceCollection services = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                ServiceCollectionExtensions.AddJsonMarshaller(services));
        }

        [Fact]
        public void AddJsonMarshaller_ShouldRegisterJsonMarshallerWithDefaultOptions()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddJsonMarshaller();

            // Assert
            var provider = services.BuildServiceProvider();
            var marshaller = provider.GetService<JsonMarshaller>();
            Assert.NotNull(marshaller);

            // Check that options are registered and default WriteIndented is false
            var options = provider.GetService<JsonFormatterOptions>();
            Assert.NotNull(options);
            Assert.False(options.Settings.WriteIndented);
        }

        [Fact]
        public void AddJsonMarshaller_ShouldApplyCustomJsonSetup()
        {
            // Arrange
            var services = new ServiceCollection();
            var called = false;

            // Act
            services.AddJsonMarshaller(o =>
            {
                o.Settings.WriteIndented = true;
                called = true;
            });

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetService<JsonFormatterOptions>();
            Assert.NotNull(options);
            Assert.True(called);
            Assert.True(options.Settings.WriteIndented);
        }

        [Fact]
        public void AddJsonMarshaller_ShouldApplyCustomServiceSetup()
        {
            // Arrange
            var services = new ServiceCollection();
            ServiceLifetime? lifetime = null;

            // Act
            services.AddJsonMarshaller(null, o =>
            {
                o.Lifetime = ServiceLifetime.Scoped;
                lifetime = o.Lifetime;
            });

            // Assert
            var descriptor = Assert.Single(services, d => d.ServiceType == typeof(JsonMarshaller));
            Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
            Assert.Equal(ServiceLifetime.Scoped, lifetime);
        }
    }
}
