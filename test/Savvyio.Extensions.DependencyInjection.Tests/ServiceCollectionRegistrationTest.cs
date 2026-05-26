using System;
using System.Collections.Generic;
using System.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Savvyio.Dispatchers;
using Xunit;

namespace Savvyio.Extensions.DependencyInjection
{
    public class ServiceCollectionRegistrationTest : Test
    {
        public ServiceCollectionRegistrationTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddConfiguredOptions_ShouldRegisterAllSupportedRepresentations()
        {
            var services = new ServiceCollection();
            services.AddConfiguredOptions<TestOptions>(o => o.Name = "Configured");
            var provider = services.BuildServiceProvider();
            var action = provider.GetRequiredService<Action<TestOptions>>();
            var target = new TestOptions();

            action(target);

            Assert.Equal("Configured", provider.GetRequiredService<IOptions<TestOptions>>().Value.Name);
            Assert.Equal("Configured", provider.GetRequiredService<TestOptions>().Name);
            Assert.Equal("Configured", target.Name);
        }

        [Fact]
        public void AddMarshaller_ShouldRegisterFactoryImplementation()
        {
            var services = new ServiceCollection();
            services.AddMarshaller<FakeMarshaller>(_ => new FakeMarshaller(), o => o.Lifetime = ServiceLifetime.Singleton);
            var provider = services.BuildServiceProvider();

            var sut1 = provider.GetRequiredService<IMarshaller>();
            var sut2 = provider.GetRequiredService<IMarshaller>();

            Assert.IsType<FakeMarshaller>(sut1);
            Assert.Same(sut1, sut2);
        }

        [Fact]
        public void AddServiceLocator_ShouldRespectCustomFactoryAndLifetime()
        {
            var services = new ServiceCollection();
            services.AddServiceLocator(o =>
            {
                o.Lifetime = ServiceLifetime.Singleton;
                o.ImplementationFactory = _ => new ServiceLocator(_ => Array.Empty<object>());
            });
            var provider = services.BuildServiceProvider();

            var sut1 = provider.GetRequiredService<IServiceLocator>();
            var sut2 = provider.GetRequiredService<IServiceLocator>();

            Assert.Same(sut1, sut2);
            Assert.Empty(sut1.GetServices(typeof(string)));
        }

        [Fact]
        public void AddHandlerServicesDescriptor_ShouldDoNothingWhenDescriptorIsMissing()
        {
            var services = new ServiceCollection();
            services.AddHandlerServicesDescriptor();
            var provider = services.BuildServiceProvider();

            Assert.Null(provider.GetService<IHandlerServicesDescriptor>());
        }

        private sealed class TestOptions : IParameterObject
        {
            public string Name { get; set; }
        }

        private sealed class FakeMarshaller : IMarshaller
        {
            public Stream Serialize<TValue>(TValue value)
            {
                return new MemoryStream(new byte[] { 1, 2, 3 });
            }

            public Stream Serialize(object value, Type inputType)
            {
                return new MemoryStream(new byte[] { 1, 2, 3 });
            }

            public TValue Deserialize<TValue>(Stream data)
            {
                return default;
            }

            public object Deserialize(Stream data, Type returnType)
            {
                return null;
            }
        }
    }
}
