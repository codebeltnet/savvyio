using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cuemon;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets;
using Savvyio.Handlers;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio
{
    public class HandlerFactoryTest : Test
    {
        public HandlerFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CreateFireForget_ShouldInvokeRegisteredHandlers()
        {
            var store = new List<FakeCommand>();
            var sut1 = HandlerFactory.CreateFireForget<IRequest>(registry =>
            {
                registry.Register<FakeCommand>(c => store.Add(c));
                registry.Register<FakeCommand<int>>(c => store.Add(c));
                registry.Register<FakeCommand<Guid>>(c => store.Add(c));
                registry.Register<FakeCommand<DateTime>>(c => store.Add(c));
                registry.Register<FakeCommand<Stream>>(c => store.Add(c));
            });

            Assert.NotNull(sut1);
            Assert.True(sut1.TryInvoke(new FakeCommand()));
            Assert.True(sut1.TryInvoke(new FakeCommand<int>()));
            Assert.True(sut1.TryInvoke(new FakeCommand<Guid>()));
            Assert.True(sut1.TryInvoke(new FakeCommand<DateTime>()));
            Assert.True(sut1.TryInvoke(new FakeCommand<Stream>()));
            Assert.False(sut1.TryInvoke(new FakeCommand<long>()));
            Assert.False(sut1.TryInvoke(new FakeCommand<TimeRange>()));
            Assert.IsType<FakeCommand>(store.Single(c => c.Type == null));
            Assert.IsType<FakeCommand<int>>(store.Single(c => c.Type == typeof(int)));
            Assert.IsType<FakeCommand<Guid>>(store.Single(c => c.Type == typeof(Guid)));
            Assert.IsType<FakeCommand<DateTime>>(store.Single(c => c.Type == typeof(DateTime)));
            Assert.IsType<FakeCommand<Stream>>(store.Single(c => c.Type == typeof(Stream)));
        }

        [Fact]
        public Task CreateFireForget_ShouldInvokeRegisteredHandlersAsync()
        {
            var store = new List<FakeCommand>();
            var sut1 = HandlerFactory.CreateFireForget<IRequest>(registry =>
            {
                registry.RegisterAsync<FakeCommand>((c, _) =>
                {
                    store.Add(c);
                    return Task.CompletedTask;
                });
                registry.RegisterAsync<FakeCommand<int>>(c =>
                {
                    store.Add(c);
                    return Task.CompletedTask;
                });
                registry.RegisterAsync<FakeCommand<Guid>>(c =>
                {
                    store.Add(c);
                    return Task.CompletedTask;
                });
                registry.RegisterAsync<FakeCommand<DateTime>>(c =>
                {
                    store.Add(c);
                    return Task.CompletedTask;
                });
                registry.RegisterAsync<FakeCommand<Stream>>(c =>
                {
                    store.Add(c);
                    return Task.CompletedTask;
                });
            });

            Assert.NotNull(sut1);
            Assert.True(sut1.TryInvokeAsync(new FakeCommand()).GetAwaiter().GetResult().Succeeded);
            Assert.True(sut1.TryInvokeAsync(new FakeCommand<int>()).GetAwaiter().GetResult().Succeeded);
            Assert.True(sut1.TryInvokeAsync(new FakeCommand<Guid>()).GetAwaiter().GetResult().Succeeded);
            Assert.True(sut1.TryInvokeAsync(new FakeCommand<DateTime>()).GetAwaiter().GetResult().Succeeded);
            Assert.True(sut1.TryInvokeAsync(new FakeCommand<Stream>()).GetAwaiter().GetResult().Succeeded);
            Assert.False(sut1.TryInvokeAsync(new FakeCommand<long>()).GetAwaiter().GetResult().Succeeded);
            Assert.False(sut1.TryInvokeAsync(new FakeCommand<TimeRange>()).GetAwaiter().GetResult().Succeeded);
            Assert.IsType<FakeCommand>(store.Single(c => c.Type == null));
            Assert.IsType<FakeCommand<int>>(store.Single(c => c.Type == typeof(int)));
            Assert.IsType<FakeCommand<Guid>>(store.Single(c => c.Type == typeof(Guid)));
            Assert.IsType<FakeCommand<DateTime>>(store.Single(c => c.Type == typeof(DateTime)));
            Assert.IsType<FakeCommand<Stream>>(store.Single(c => c.Type == typeof(Stream)));

            return Task.CompletedTask;
        }

        [Fact]
        public void CreateRequestReply_ShouldInvokeRegisteredHandlers()
        {
            var sut1 = HandlerFactory.CreateRequestReply<IRequest>(registry =>
            {
                registry.Register<FakeCommand, Type>(c => c.Type);
                registry.Register<FakeCommand<int>, Type>(c => c.Type);
                registry.Register<FakeCommand<Guid>, Type>(c => c.Type);
                registry.Register<FakeCommand<DateTime>, Type>(c => c.Type);
                registry.Register<FakeCommand<Stream>, Type>(c => c.Type);
            });

            Assert.NotNull(sut1);
            Assert.True(sut1.TryInvoke<Type>(new FakeCommand(), out var fc1));
            Assert.True(sut1.TryInvoke<Type>(new FakeCommand<int>(), out var fc2));
            Assert.True(sut1.TryInvoke<Type>(new FakeCommand<Guid>(), out var fc3));
            Assert.True(sut1.TryInvoke<Type>(new FakeCommand<DateTime>(), out var fc4));
            Assert.True(sut1.TryInvoke<Type>(new FakeCommand<Stream>(), out var fc5));
            Assert.False(sut1.TryInvoke<Type>(new FakeCommand<long>(), out var fc6));
            Assert.False(sut1.TryInvoke<Type>(new FakeCommand<TimeRange>(), out var fc7));
            Assert.Null(fc1);
            Assert.Equal(typeof(int), fc2);
            Assert.Equal(typeof(Guid), fc3);
            Assert.Equal(typeof(DateTime), fc4);
            Assert.Equal(typeof(Stream), fc5);
            Assert.Null(fc6);
            Assert.Null(fc7);
        }

        [Fact]
        public async Task CreateRequestReply_ShouldInvokeRegisteredHandlersAsync()
        {
            var sut1 = HandlerFactory.CreateRequestReply<IRequest>(registry =>
            {
                registry.RegisterAsync<FakeCommand, Type>((c, _) => Task.FromResult(c.Type));
                registry.RegisterAsync<FakeCommand<int>, Type>(c => Task.FromResult(c.Type));
                registry.RegisterAsync<FakeCommand<Guid>, Type>(c => Task.FromResult(c.Type));
                registry.RegisterAsync<FakeCommand<DateTime>, Type>(c => Task.FromResult(c.Type));
                registry.RegisterAsync<FakeCommand<Stream>, Type>(c => Task.FromResult(c.Type));
            });

            var sut2 = await sut1.TryInvokeAsync<Type>(new FakeCommand());
            var sut3 = await sut1.TryInvokeAsync<Type>(new FakeCommand<int>());
            var sut4 = await sut1.TryInvokeAsync<Type>(new FakeCommand<Guid>());
            var sut5 = await sut1.TryInvokeAsync<Type>(new FakeCommand<DateTime>());
            var sut6 = await sut1.TryInvokeAsync<Type>(new FakeCommand<Stream>());
            var sut7 = await sut1.TryInvokeAsync<Type>(new FakeCommand<long>());
            var sut8 = await sut1.TryInvokeAsync<Type>(new FakeCommand<TimeRange>());

            Assert.NotNull(sut1);
            Assert.True(sut2.Succeeded);
            Assert.True(sut3.Succeeded);
            Assert.True(sut4.Succeeded);
            Assert.True(sut5.Succeeded);
            Assert.True(sut6.Succeeded);
            Assert.False(sut7.Succeeded);
            Assert.False(sut8.Succeeded);
            Assert.Null(sut2.Result);
            Assert.Equal(typeof(int), sut3.Result);
            Assert.Equal(typeof(Guid), sut4.Result);
            Assert.Equal(typeof(DateTime), sut5.Result);
            Assert.Equal(typeof(Stream), sut6.Result);
            Assert.Null(sut7.Result);
            Assert.Null(sut8.Result);
        }
    }
}
