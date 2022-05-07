using System;
using System.Data;

namespace Savvyio.Extensions.Dapper
{
    /// <summary>
    /// Configuration options for <see cref="IDapperDataStore"/>.
    /// </summary>
    public class DapperDataStoreOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperDataStoreOptions"/> class.
        /// </summary>
        public DapperDataStoreOptions()
        {
        }

        /// <summary>
        /// Gets or sets the function delegate that provides the <see cref="IDbConnection"/>.
        /// </summary>
        /// <value>The function delegate that provides the <see cref="IDbConnection"/>.</value>
        public Func<IDbConnection> ConnectionFactory { get; set; }
    }
}
