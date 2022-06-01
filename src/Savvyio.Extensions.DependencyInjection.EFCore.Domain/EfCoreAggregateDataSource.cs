using System;
using Cuemon.Extensions;
using Microsoft.Extensions.Options;
using Savvyio.Domain;
using Savvyio.Extensions.EFCore;
using Savvyio.Extensions.EFCore.Domain;

namespace Savvyio.Extensions.DependencyInjection.EFCore.Domain
{
    /// <summary>
    /// Provides an implementation of the <see cref="EfCoreDataSource"/> that is optimized for Domain Driven Design and the concept of Aggregate Root.
    /// </summary>
    /// <seealso cref="EfCoreDataSource" />
    public class EfCoreAggregateDataSource<TMarker> : EfCoreAggregateDataSource, IEfCoreDataSource<TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreAggregateDataSource"/> class.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher" /> that are responsible for raising domain events.</param>
        /// <param name="setup">The <see cref="EfCoreDataSourceOptions" /> which need to be configured.</param>
        public EfCoreAggregateDataSource(IDomainEventDispatcher dispatcher, IOptions<EfCoreDataSourceOptions<TMarker>> setup) : base(dispatcher, new EfCoreDbContext<TMarker>(setup))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreAggregateDataSource"/> class.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher" /> that are responsible for raising domain events.</param>
        /// <param name="setup">The <see cref="EfCoreDataSourceOptions" /> which need to be configured.</param>
        public EfCoreAggregateDataSource(IDomainEventDispatcher dispatcher, Action<EfCoreDataSourceOptions<TMarker>> setup) : this(dispatcher, Options.Create(setup.Configure()))
        {
        }
    }
}
