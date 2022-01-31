using Savvyio.Storage;

namespace Savvyio.Extensions.DependencyInjection.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of readable data access objects (cRud).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    public interface IReadableDataAccessObject<T, TMarker> : IDataAccessObject<T, TMarker>, IReadableDataAccessObject<T> where T : class
    {
    }
}
