using System;
using System.Threading.Tasks;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Events;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.Extensions;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Handlers;

namespace Savvyio.Assets
{
    public class AccountCommandHandler : CommandHandler
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<Account> _uow;
        private readonly IPersistentRepository<Account, long, Account> _accountRepository;

        public AccountCommandHandler(IMediator mediator = null, IUnitOfWork<Account> uow = null, IPersistentRepository<Account, long, Account> accountRepository = null)
        {
            _mediator = mediator;
            _uow = uow;
            _accountRepository = accountRepository;
        }

        protected override void RegisterDelegates(IFireForgetRegistry<ICommand> handlers)
        {
            handlers.RegisterAsync<CreateAccount>(CreateAccountAsync);
            handlers.RegisterAsync<UpdateAccount>(async c =>
            {
                var account = new Account(c.Id);
                account.ChangeFullName(c.FullName);
                account.ChangeEmailAddress(c.EmailAddress);
                _accountRepository?.Add(account); // store in db
                await _uow.SaveChangesAsync();
                await _mediator.PublishAsync(new AccountUpdated(account.Id, account.FullName, account.EmailAddress)); // raise integration event
            });
        }

        public async Task CreateAccountAsync(CreateAccount c)
        {
            var account = new Account(c.PlatformProviderId, c.FullName, c.EmailAddress).MergeMetadata(c);

            // check unique email
            //await _mediator.RaiseManyAsync(account).ConfigureAwait(false);

            try
            {
                _accountRepository.Add(account);
                await _uow.SaveChangesAsync(); // store in db
            }
            catch (Exception e)
            {
                _accountRepository.Remove(account); // undo add
                throw;
            }

            await _mediator.PublishAsync(new AccountCreated(account.Id, account.FullName, account.EmailAddress).MergeMetadata(account)); // raise integration event
        }
    }
}
