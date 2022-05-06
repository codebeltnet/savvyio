using Cuemon.Threading;

namespace Savvyio.Data
{
    /// <summary>
    /// A marker interface that specifies an abstraction of persistent data access based on the Data Access Object pattern.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of the options associated with this DTO.</typeparam>
    public interface IDataAccessObject<in T, TOptions>
        where T : class
        where TOptions : AsyncOptions, new()
    {
    }
}
