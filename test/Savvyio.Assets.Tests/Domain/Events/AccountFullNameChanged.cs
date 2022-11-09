using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public record AccountFullNameChanged : DomainEvent
    {
        public AccountFullNameChanged(Account account)
        {
            FullName = account.FullName;
        }

        public string FullName { get; private set; }
    }
}
