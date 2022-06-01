using System;
using System.Data;

namespace Savvyio.Extensions.Dapper
{
    /// <summary>
    /// Configuration options for <see cref="IDapperDataSource"/>.
    /// </summary>
    public class DapperDataSourceOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperDataSourceOptions"/> class.
        /// </summary>
        public DapperDataSourceOptions()
        {
        }

        /// <summary>
        /// Gets or sets the function delegate that provides the <see cref="IDbConnection"/>.
        /// </summary>
        /// <value>The function delegate that provides the <see cref="IDbConnection"/>.</value>
        public Func<IDbConnection> ConnectionFactory { get; set; }
    }
}
