using System;
using Cuemon.Extensions;
using Microsoft.Extensions.Options;
using Savvyio.Domain;
using Savvyio.Extensions.EFCore;
using Savvyio.Extensions.EFCore.Domain;

namespace Savvyio.Extensions.DependencyInjection.EFCore.Domain
{
    /// <summary>
    /// Provides an implementation of the <see cref="EfCoreDataStore"/> that is optimized for Domain Driven Design and the concept of Aggregate Root.
    /// </summary>
    /// <seealso cref="EfCoreDataStore" />
    public class EfCoreAggregateDataStore<TMarker> : EfCoreAggregateDataStore, IEfCoreDataStore<TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreAggregateDataStore"/> class.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher" /> that are responsible for raising domain events.</param>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions" /> which need to be configured.</param>
        public EfCoreAggregateDataStore(IDomainEventDispatcher dispatcher, IOptions<EfCoreDataStoreOptions<TMarker>> setup) : base(dispatcher, new EfCoreDbContext<TMarker>(setup))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreAggregateDataStore"/> class.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher" /> that are responsible for raising domain events.</param>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions" /> which need to be configured.</param>
        public EfCoreAggregateDataStore(IDomainEventDispatcher dispatcher, Action<EfCoreDataStoreOptions<TMarker>> setup) : this(dispatcher, Options.Create(setup.Configure()))
        {
        }
    }
}
