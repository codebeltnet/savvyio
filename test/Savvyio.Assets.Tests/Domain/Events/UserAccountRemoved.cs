using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public record UserAccountRemoved : DomainEvent
    {
        public UserAccountRemoved()
        {
        }

        public UserAccountRemoved(UserAccount account)
        {
            Id = account.Id;
        }

        public long Id { get; private set; }
    }
}
