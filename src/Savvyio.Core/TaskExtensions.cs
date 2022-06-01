using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Savvyio
{
    /// <summary>
    /// Extension methods for the <see cref="Task{TResult}"/> class.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Asynchronously returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Task{TResult}"/> to return the single element of.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the single element of the input sequence, or default (TSource) if the sequence contains no elements.</returns>
        public static async Task<T> SingleOrDefaultAsync<T>(this Task<IEnumerable<T>> source)
        {
            var presult = await source.ConfigureAwait(false);
            return presult.SingleOrDefault();
        }
    }
}
