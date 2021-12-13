using System.Threading;
using System.Threading.Tasks;

namespace Savvyio
{
    public interface IResponseHandlerActivator<in TModel>
    {
        bool TryInvoke<TResult>(TModel model, out TResult result);

        Task<ConditionalOperation<TResult>> TryInvokeAsync<TResult>(TModel model, CancellationToken ct = default);
    }
}
