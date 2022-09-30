using System.Linq;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Handlers;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Extensions;
using Savvyio.Queries;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio
{
    public class SavvyioOptionsExtensionsTest : Test
    {
        public SavvyioOptionsExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddDispatchers_ShouldNotAddAnyDispatchersFromCurrentDomain()
        {
            var sut = new SavvyioOptions().AddDispatchers();
            Assert.Empty(sut.DispatcherServiceTypes);
            Assert.Empty(sut.DispatcherImplementationTypes);
        }

        [Fact]
        public void AddDispatchers_ShouldAddDispatchersFromSpecifiedAssemblies()
        {
            var sut = new SavvyioOptions().AddDispatchers(typeof(ICommand).Assembly, typeof(IDomainEvent).Assembly, typeof(IIntegrationEvent).Assembly, typeof(IQuery).Assembly);

            Assert.NotEmpty(sut.DispatcherServiceTypes);
            Assert.NotEmpty(sut.DispatcherImplementationTypes);
            Assert.Collection(sut.DispatcherServiceTypes,
                type => Assert.Equal(typeof(ICommandDispatcher), type),
                type => Assert.Equal(typeof(IDomainEventDispatcher), type),
                type => Assert.Equal(typeof(IIntegrationEventDispatcher), type),
                type => Assert.Equal(typeof(IQueryDispatcher), type));
            Assert.Collection(sut.DispatcherImplementationTypes,
                type => Assert.Equal(typeof(CommandDispatcher), type),
                type => Assert.Equal(typeof(DomainEventDispatcher), type),
                type => Assert.Equal(typeof(IntegrationEventDispatcher), type),
                type => Assert.Equal(typeof(QueryDispatcher), type));
        }

        [Fact]
        public void AddDispatchers_ShouldAutomaticallyAddDispatchersFromReferencedAssemblies()
        {
            var sut = new SavvyioOptions().UseAutomaticDispatcherDiscovery();

            Assert.NotEmpty(sut.DispatcherServiceTypes);
            Assert.NotEmpty(sut.DispatcherImplementationTypes);
            Assert.Collection(sut.DispatcherServiceTypes.OrderBy(type => type.Name),
                type => Assert.Equal(typeof(ICommandDispatcher), type),
                type => Assert.Equal(typeof(IDomainEventDispatcher), type),
                type => Assert.Equal(typeof(IIntegrationEventDispatcher), type),
                type => Assert.Equal(typeof(IMediator), type),
                type => Assert.Equal(typeof(IQueryDispatcher), type));
            Assert.Collection(sut.DispatcherImplementationTypes.OrderBy(type => type.Name),
                type => Assert.Equal(typeof(CommandDispatcher), type),
                type => Assert.Equal(typeof(DomainEventDispatcher), type),
                type => Assert.Equal(typeof(IntegrationEventDispatcher), type),
                type => Assert.Equal(typeof(Mediator), type),
                type => Assert.Equal(typeof(QueryDispatcher), type));
        }

        [Fact]
        public void AddHandlers_ShouldNotAddAnyHandlersFromCurrentDomain()
        {
            var sut = new SavvyioOptions().AddHandlers();
            Assert.Empty(sut.HandlerServiceTypes);
            Assert.Empty(sut.HandlerImplementationTypes);
        }

        [Fact]
        public void AddHandlers_ShouldAddHandlersFromSpecifiedAssemblies()
        {
            var sut = new SavvyioOptions().AddHandlers(typeof(ICommand).Assembly, typeof(IDomainEvent).Assembly, typeof(IIntegrationEvent).Assembly, typeof(IQuery).Assembly, typeof(Account).Assembly);

            Assert.NotEmpty(sut.HandlerServiceTypes);
            Assert.NotEmpty(sut.HandlerImplementationTypes);
            Assert.Collection(sut.HandlerServiceTypes,
                type => Assert.Equal(typeof(ICommandHandler), type),
                type => Assert.Equal(typeof(IDomainEventHandler), type),
                type => Assert.Equal(typeof(IIntegrationEventHandler), type),
                type => Assert.Equal(typeof(IQueryHandler), type));
            Assert.Collection(sut.HandlerImplementationTypes,
                type => Assert.Equal(typeof(AccountCommandHandler), type),
                type => Assert.Equal(typeof(PlatformProviderHandler), type),
                type => Assert.Equal(typeof(AccountDomainEventHandler), type),
                type => Assert.Equal(typeof(PlatformProviderDomainEventHandler), type),
                type => Assert.Equal(typeof(AccountEventHandler), type),
                type => Assert.Equal(typeof(AccountQueryHandler), type));
        }

        [Fact]
        public void AddHandlers_ShouldAddHandlersAutomaticallyFromReferencedAssemblies()
        {
            var sut = new SavvyioOptions().UseAutomaticHandlerDiscovery();

            Assert.NotEmpty(sut.HandlerServiceTypes);
            Assert.NotEmpty(sut.HandlerImplementationTypes);
            Assert.Collection(sut.HandlerServiceTypes.OrderBy(type => type.Name),
                type => Assert.Equal(typeof(ICommandHandler), type),
                type => Assert.Equal(typeof(IDomainEventHandler), type),
                type => Assert.Equal(typeof(IIntegrationEventHandler), type),
                type => Assert.Equal(typeof(IQueryHandler), type));
            Assert.Collection(sut.HandlerImplementationTypes.OrderBy(type => type.Name),
                type => Assert.Equal(typeof(AccountCommandHandler), type),
                type => Assert.Equal(typeof(AccountDomainEventHandler), type),
                type => Assert.Equal(typeof(AccountEventHandler), type),
                type => Assert.Equal(typeof(AccountHandlers), type),
                type => Assert.Equal(typeof(AccountQueryHandler), type),
                type => Assert.Equal(typeof(PlatformProviderDomainEventHandler), type),
                type => Assert.Equal(typeof(PlatformProviderHandler), type)
                );
        }
    }
}
