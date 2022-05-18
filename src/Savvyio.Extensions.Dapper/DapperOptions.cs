using System;
using System.Data;
using Cuemon.Threading;
using Dapper;

namespace Savvyio.Extensions.Dapper
{
    /// <summary>
    /// Specifies options that is related to <see cref="DapperDataAccessObject{T}"/>.
    /// </summary>
    /// <seealso cref="AsyncOptions" />
    public class DapperOptions : AsyncOptions
    {
        /// <summary>
        /// Performs an implicit conversion from <see cref="DapperOptions"/> to <see cref="CommandDefinition"/>.
        /// </summary>
        /// <param name="value">The <see cref="DapperOptions"/> to convert.</param>
        /// <returns>A <see cref="CommandDefinition"/> that is equivalent to <paramref name="value"/>.</returns>
        public static implicit operator CommandDefinition(DapperOptions value)
        {
            return new CommandDefinition(value.CommandText, commandTimeout: (int)value.CommandTimeout.TotalSeconds, cancellationToken: value.CancellationToken, flags: value.CommandFlags, commandType: value.CommandType, transaction: value.Transaction, parameters: value.Parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AsyncOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="AsyncOptions.CancellationToken"/></term>
        ///         <description><c>default</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="CommandTimeout"/></term>
        ///         <description><c>TimeSpan.FromSeconds(30)</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="CommandFlags"/></term>
        ///         <description><c>CommandFlags.Buffered</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="CommandType"/></term>
        ///         <description><c>CommandType.Text</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public DapperOptions()
        {
            CommandTimeout = TimeSpan.FromSeconds(30);
            CommandFlags = CommandFlags.Buffered;
            CommandType = CommandType.Text;
        }

        /// <summary>
        /// Gets or sets the parameters associated with the command.
        /// </summary>
        /// <value>The parameters associated with the command.</value>
        /// <remarks>This is typically the DTO.</remarks>
        public object Parameters { get; set; }

        /// <summary>
        /// Gets or sets the command (sql or a stored-procedure name) to execute.
        /// </summary>
        /// <value>The command (sql or a stored-procedure name) to execute.</value>
        public string CommandText { get; set; }

        /// <summary>
        /// Gets or sets the timeout for the command.
        /// </summary>
        /// <value>The timeout for the command.</value>
        public TimeSpan CommandTimeout { get; set; }

        /// <summary>
        /// Gets or sets the state flags against the command.
        /// </summary>
        /// <value>The state flags against the command.</value>
        public CommandFlags CommandFlags { get; set; }

        /// <summary>
        /// Gets or sets the type of command that the command-text represents.
        /// </summary>
        /// <value>The type of command that the command-text represents.</value>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the active transaction for the command.
        /// </summary>
        /// <value>The active transaction for the command.</value>
        public IDbTransaction Transaction { get; set; }
    }
}
