using System.Threading;
using System.Threading.Tasks;

namespace Savvyio.Diagnostics
{
    /// <summary>
    /// Defines a contract for asynchronously providing an underlying target that can be probed to assess health status.
    /// </summary>
    /// <typeparam name="T">The type of the underlying target used for health probing.</typeparam>
    public interface IAsyncHealthCheckProvider<T>
    {
        /// <summary>
        /// Asynchronously gets the underlying target used for probing health status.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the target instance of type <typeparamref name="T"/> used for health probing.</returns>
        Task<T> GetHealthCheckTargetAsync(CancellationToken cancellationToken);
    }
}
