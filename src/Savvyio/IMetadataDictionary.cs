using System.Collections.Generic;

namespace Savvyio
{
    /// <summary>
    /// Defines a generic way to support metadata capabilities.
    /// </summary>
    /// <seealso cref="IDictionary{TKey,TValue}" />
    public interface IMetadataDictionary : IDictionary<string, object>
    {
        internal IMetadataDictionary AddUnristricted(string key, object value);
    }
}
