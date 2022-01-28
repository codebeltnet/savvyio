using System;
using Cuemon.Extensions;
using Microsoft.Extensions.Options;
using Savvyio.Domain;
using Savvyio.Extensions.Microsoft.EntityFrameworkCore;

namespace Savvyio.Extensions.Microsoft.DependencyInjection.EntityFrameworkCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IEfCoreDataStore{TMarker}"/> interface to support multiple implementations that does the actual I/O communication towards a data store using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="EfCoreDataStore" />
    /// <seealso cref="IEfCoreDataStore{TMarker}" />
    public class EfCoreDataStore<TMarker> : EfCoreDataStore, IEfCoreDataStore<TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataStore{TMarker}"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions{TMarker}" /> which need to be configured.</param>
        public EfCoreDataStore(IOptions<EfCoreDataStoreOptions<TMarker>> setup) : this(null, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataStore{TMarker}"/> class.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher"/> that are responsible for raising domain events.</param>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions{TMarker}" /> which need to be configured.</param>
        public EfCoreDataStore(IDomainEventDispatcher dispatcher, IOptions<EfCoreDataStoreOptions<TMarker>> setup) : base(dispatcher, new SavvyioDbContext<TMarker>(setup))
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataStore{TMarker}"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions{TMarker}" /> which need to be configured.</param>
        public EfCoreDataStore(Action<EfCoreDataStoreOptions<TMarker>> setup) : this(null, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataStore{TMarker}"/> class.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher"/> that are responsible for raising domain events.</param>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions{TMarker}" /> which need to be configured.</param>
        public EfCoreDataStore(IDomainEventDispatcher dispatcher, Action<EfCoreDataStoreOptions<TMarker>> setup) : this(dispatcher, Options.Create(setup.Configure()))
        {
        }
    }
}
