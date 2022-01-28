using Savvyio.Storage;

namespace Savvyio.Extensions.Microsoft.DependencyInjection.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of deletable data access objects (cruD).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    public interface IDeletableDataAccessObject<in T, TMarker> : IDataAccessObject<T, TMarker>, IDeletableDataAccessObject<T> where T : class
    {
    }
}
