using Savvyio.Assets.Events;
using Savvyio.Queries;

namespace Savvyio.Assets.Queries
{
    public class GetAccount : Query<AccountProjection>
    {
        public GetAccount(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
