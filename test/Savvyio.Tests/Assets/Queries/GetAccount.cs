using Savvyio.Queries;

namespace Savvyio.Assets.Queries
{
    public class GetAccount : Query<string>
    {
        public GetAccount(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
