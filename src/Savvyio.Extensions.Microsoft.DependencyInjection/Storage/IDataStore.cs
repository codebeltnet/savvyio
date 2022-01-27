﻿using Savvyio.Storage;

namespace Savvyio.Extensions.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations that does the actual I/O communication towards a data store.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IUnitOfWork{TMarker}" />
    /// <seealso cref="IDataStore"/>
    public interface IDataStore<TMarker> : IDataStore, IUnitOfWork<TMarker>
    {
    }
}
