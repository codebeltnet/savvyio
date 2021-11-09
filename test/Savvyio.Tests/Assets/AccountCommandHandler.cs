using System.Threading.Tasks;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Events;
using Savvyio.Commands;
using Savvyio.Domain;

namespace Savvyio.Assets
{
    public class AccountCommandHandler : CommandHandler
    {
        private readonly IMediator _mediator;
        private readonly IActiveRecordRepository<Account, long> _activeRecordRepository;

        public AccountCommandHandler(IMediator mediator, IActiveRecordRepository<Account, long> activeRecordRepository)
        {
            _mediator = mediator;
            _activeRecordRepository = activeRecordRepository;
        }

        protected override void RegisterCommandHandlers(IHandlerRegistry<ICommand> handler)
        {
            handler.RegisterAsync<CreateAccount>(CreateAccountAsync);
            handler.RegisterAsync<UpdateAccount>(c =>
            {
                var account = new Account(c.Id);
                account.ChangeFullName(c.FullName);
                account.ChangeEmailAddress(c.EmailAddress);
                _activeRecordRepository.SaveAsync(account); // store in db
                return _mediator.PublishAsync(new AccountUpdated(account.Id, account.FullName, account.EmailAddress)); // raise integration event
            });
        }

        public async Task CreateAccountAsync(CreateAccount c)
        {
            var account = new Account(c.PlatformProviderId, c.FullName, c.EmailAddress);

            // check unique email

            await _activeRecordRepository.SaveAsync(account); // store in db
            await _mediator.PublishAsync(new AccountCreated(account.Id, account.FullName, account.EmailAddress)); // raise integration event
        }
    }
}
