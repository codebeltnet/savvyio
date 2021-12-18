using System.Threading.Tasks;

namespace Savvyio.Queries
{
    public interface IQuery : IMetadata
    {
    }

    public interface IQuery<TResult> : IRequestReply<TResult>, IQuery
    {
    }
}
