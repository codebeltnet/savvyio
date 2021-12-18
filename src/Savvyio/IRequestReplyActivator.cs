using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio
{
    public interface IRequestReplyActivator<in TModel>
    {
        bool TryInvoke<TResult>(TModel model, out TResult result);

        Task<ConditionalValue<TResult>> TryInvokeAsync<TResult>(TModel model, CancellationToken ct = default);
    }
}
