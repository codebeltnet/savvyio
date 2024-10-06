using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Domain.Events;
using Savvyio.Handlers;

namespace Savvyio.Domain.Assets
{
    public class AccountHandlers : IDomainEventHandler
    {
        private readonly ITestStore<IDomainEvent> _domainEventStore;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public AccountHandlers(ITestStore<IDomainEvent> domainEventStore = null, IDomainEventDispatcher domainEventDispatcher = null)
        {
            _domainEventStore = domainEventStore;
            _domainEventDispatcher = domainEventDispatcher;
        }

        IFireForgetActivator<IDomainEvent> IFireForgetHandler<IDomainEvent>.Delegates => HandlerFactory.CreateFireForget<IDomainEvent>(registry =>
        {
            registry.Register<AccountInitiated>(a => _domainEventStore.Add(a));
            registry.Register<UserAccountInitiated>(a => _domainEventStore.Add(a));
            registry.Register<UserAccountPasswordInitiated>(a => _domainEventStore.Add(a));
            registry.Register<AccountFullNameChanged>(a => _domainEventStore.Add(a));
            registry.Register<AccountEmailAddressChanged>(a => _domainEventStore.Add(a));
            registry.RegisterAsync<AccountInitiated>(a =>
            {
                _domainEventStore.Add(a);
                return Task.CompletedTask;
            });
            registry.RegisterAsync<UserAccountInitiated>(a =>
            {
                _domainEventStore.Add(a);
                return Task.CompletedTask;
            });
            registry.RegisterAsync<UserAccountPasswordInitiated>(a =>
            {
                _domainEventStore.Add(a);
                return Task.CompletedTask;
            });
            registry.RegisterAsync<AccountFullNameChanged>(a =>
            {
                _domainEventStore.Add(a);
                return Task.CompletedTask;
            });
            registry.RegisterAsync<AccountEmailAddressChanged>(a =>
            {
                _domainEventStore.Add(a);
                return Task.CompletedTask;
            });
        });
    }
}
