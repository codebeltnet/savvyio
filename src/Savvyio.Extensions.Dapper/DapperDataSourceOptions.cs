using System;
using System.Data;
using Cuemon;
using Cuemon.Configuration;

namespace Savvyio.Extensions.Dapper
{
    /// <summary>
    /// Configuration options for <see cref="IDapperDataSource"/>.
    /// </summary>
    public class DapperDataSourceOptions : IValidatableParameterObject
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

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="ConnectionFactory"/> cannot be null.
        /// </exception>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        public void ValidateOptions()
        {
            Validator.ThrowIfObjectInDistress(ConnectionFactory == null);
        }
    }
}
