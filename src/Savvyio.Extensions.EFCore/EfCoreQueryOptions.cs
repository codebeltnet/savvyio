using System;
using System.Linq.Expressions;
using Cuemon.Threading;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Specifies options that is related to <see cref="DefaultEfCoreDataStore{T}"/>.
    /// </summary>
    /// <seealso cref="AsyncOptions" />
    public class EfCoreQueryOptions<T> : AsyncOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreQueryOptions{T}"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="EfCoreQueryOptions{T}"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Predicate"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AsyncOptions.CancellationToken"/></term>
        ///         <description><c>default</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public EfCoreQueryOptions()
        {
        }

        /// <summary>
        /// Gets or sets the predicate that matches one or more objects to retrieve in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <value>The predicate that matches one or more objects to retrieve in the associated <seealso cref="IEfCoreDataSource"/>.</value>
        public Expression<Func<T, bool>> Predicate { get; set; }
    }
}
