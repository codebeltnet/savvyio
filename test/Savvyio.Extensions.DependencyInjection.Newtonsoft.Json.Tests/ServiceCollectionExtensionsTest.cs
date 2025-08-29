using System;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Savvyio.Extensions.DependencyInjection.Newtonsoft.Json;
using Savvyio.Extensions.Newtonsoft.Json;
using Xunit;

namespace Savvyio.Extensions.DependencyInjection.Newtonsoft.Json.Tests
{
    public class ServiceCollectionExtensionsTest : Test
    {
        [Fact]
        public void AddNewtonsoftJsonMarshaller_ShouldThrowOnNullServices()
        {
            // Arrange
            IServiceCollection services = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                ServiceCollectionExtensions.AddNewtonsoftJsonMarshaller(services));
        }

        [Fact]
        public void AddNewtonsoftJsonMarshaller_ShouldRegisterNewtonsoftJsonMarshallerWithDefaultOptions()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddNewtonsoftJsonMarshaller();

            // Assert
            var provider = services.BuildServiceProvider();
            var marshaller = provider.GetService<NewtonsoftJsonMarshaller>();
            Assert.NotNull(marshaller);

            // Check that options are registered and default formatting is None
            var options = provider.GetService<NewtonsoftJsonFormatterOptions>();
            Assert.NotNull(options);
            Assert.Equal(Formatting.None, options.Settings.Formatting);
        }

        [Fact]
        public void AddNewtonsoftJsonMarshaller_ShouldApplyCustomJsonSetup()
        {
            // Arrange
            var services = new ServiceCollection();
            var called = false;

            // Act
            services.AddNewtonsoftJsonMarshaller(o =>
            {
                o.Settings.Formatting = Formatting.Indented;
                called = true;
            });

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetService<NewtonsoftJsonFormatterOptions>();
            Assert.NotNull(options);
            Assert.True(called);
            Assert.Equal(Formatting.Indented, options.Settings.Formatting);
        }

        [Fact]
        public void AddNewtonsoftJsonMarshaller_ShouldApplyCustomServiceSetup()
        {
            // Arrange
            var services = new ServiceCollection();
            ServiceLifetime? lifetime = null;

            // Act
            services.AddNewtonsoftJsonMarshaller(null, o =>
            {
                o.Lifetime = ServiceLifetime.Scoped;
                lifetime = o.Lifetime;
            });

            // Assert
            var descriptor = Assert.Single(services, d => d.ServiceType == typeof(NewtonsoftJsonMarshaller));
            Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
            Assert.Equal(ServiceLifetime.Scoped, lifetime);
        }
    }
}
