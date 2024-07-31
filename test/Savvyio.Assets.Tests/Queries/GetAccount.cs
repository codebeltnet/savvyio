using Savvyio.Queries;

namespace Savvyio.Assets.Queries
{
    public record GetAccount : Query<AccountProjection>
    {
        public GetAccount(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
