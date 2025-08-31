namespace Savvyio.Diagnostics
{
    /// <summary>
    /// Defines a contract for providing an underlying target that can be probed to assess health status.
    /// </summary>
    /// <typeparam name="T">The type of the underlying target used for health probing.</typeparam>
    public interface IHealthCheckProvider<out T>
    {
        /// <summary>
        /// Gets the underlying target used for probing health status.
        /// </summary>
        /// <returns>The target instance of type <typeparamref name="T"/> used for health probing.</returns>
        T GetHealthCheckTarget();
    }
}
