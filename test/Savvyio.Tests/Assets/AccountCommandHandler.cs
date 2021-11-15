using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Events;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.Events;
using Xunit.Abstractions;

namespace Savvyio.Assets
{
    public class AccountCommandHandler : CommandHandler
    {
        private readonly IMediator _mediator;
        private readonly IActiveRecordRepository<Account, long> _activeRecordRepository;
        private readonly ITestOutputHelper _output;

        public AccountCommandHandler(IMediator mediator, IActiveRecordRepository<Account, long> activeRecordRepository, ITestOutputHelper output)
        {
            _mediator = mediator;
            _activeRecordRepository = activeRecordRepository;
            _output = output;
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
            var account = new Account(c.PlatformProviderId, c.FullName, c.EmailAddress).MergeMetadata(c);

            // check unique email

            await _activeRecordRepository.SaveAsync(account); // store in db

            await Task.Delay(TimeSpan.FromSeconds(1));

            await _mediator.PublishAsync(new AccountCreated(account.Id, account.FullName, account.EmailAddress).MergeMetadata(account)); // raise integration event
        }
    }
}
