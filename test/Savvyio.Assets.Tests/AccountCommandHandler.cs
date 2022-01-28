using System;
using System.Threading.Tasks;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Events;
using Savvyio.Commands;
using Savvyio.Extensions.Dispatchers;
using Savvyio.Extensions.Microsoft.DependencyInjection.Storage;
using Savvyio.Handlers;
using Xunit.Abstractions;

namespace Savvyio.Assets
{
    public class AccountCommandHandler : CommandHandler
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<DbMarker> _uow;
        private readonly IPersistentRepository<Account, long, DbMarker> _accountRepository;
        private readonly ITestOutputHelper _output;

        public AccountCommandHandler(IMediator mediator = null, IUnitOfWork<DbMarker> uow = null, IPersistentRepository<Account, long, DbMarker> accountRepository = null, ITestOutputHelper output = null)
        {
            _mediator = mediator;
            _uow = uow;
            _accountRepository = accountRepository;
            _output = output;
        }

        protected override void RegisterDelegates(IFireForgetRegistry<ICommand> handlers)
        {
            handlers.RegisterAsync<CreateAccount>(CreateAccountAsync);
            handlers.RegisterAsync<UpdateAccount>(async c =>
            {
                var account = new Account(c.Id);
                account.ChangeFullName(c.FullName);
                account.ChangeEmailAddress(c.EmailAddress);
                _accountRepository.Add(account); // store in db
                await _uow.SaveChangesAsync();
                await _mediator.PublishAsync(new AccountUpdated(account.Id, account.FullName, account.EmailAddress)); // raise integration event
            });
        }

        public async Task CreateAccountAsync(CreateAccount c)
        {
            var account = new Account(c.PlatformProviderId, c.FullName, c.EmailAddress).MergeMetadata(c);

            // check unique email

            _accountRepository.Add(account);
            await _uow.SaveChangesAsync(); // store in db

            await Task.Delay(TimeSpan.FromSeconds(1));

            await _mediator.PublishAsync(new AccountCreated(account.Id, account.FullName, account.EmailAddress).MergeMetadata(account)); // raise integration event
        }
    }
}
