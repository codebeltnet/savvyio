using Savvyio.Assets.Events;
using Savvyio.Queries;

namespace Savvyio.Assets.Queries
{
    public record GetFakeAccount : Query<AccountCreated>
    {
        public GetFakeAccount(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
