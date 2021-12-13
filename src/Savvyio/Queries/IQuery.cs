using System.Threading.Tasks;

namespace Savvyio.Queries
{
    public interface IQuery
    {
    }

    public interface IQuery<TResult> : IResponse<TResult>, IQuery
    {
    }
}
