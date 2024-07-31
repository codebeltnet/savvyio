using Cuemon;
using Cuemon.Configuration;

namespace Savvyio.Extensions.EFCore.Domain.EventSourcing
{
    /// <summary>
    /// Configuration options for <see cref="EfCoreTracedAggregateEntity{TEntity,TKey}"/>.
    /// </summary>
    public class EfCoreTracedAggregateEntityOptions : IParameterObject
    {
        private string _payloadColumnType;
        private string _payloadColumnName;
        private string _typeColumnType;
        private string _typeColumnName;
        private string _timestampColumnType;
        private string _timestampColumnName;
        private string _compositePrimaryKeyVersionColumnType;
        private string _compositePrimaryKeyVersionColumnName;
        private string _compositePrimaryKeyIdColumnType;
        private string _compositePrimaryKeyIdColumnName;
        private string _tableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreTracedAggregateEntityOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="EfCoreTracedAggregateEntityOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="TableName"/></term>
        ///         <description>DomainEvents</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="CompositePrimaryKeyIdColumnName"/></term>
        ///         <description>id</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="CompositePrimaryKeyIdColumnType"/></term>
        ///         <description>uniqueidentifier</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="CompositePrimaryKeyVersionColumnName"/></term>
        ///         <description>version</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="CompositePrimaryKeyVersionColumnType"/></term>
        ///         <description>int</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="TimestampColumnName"/></term>
        ///         <description>timestamp</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="TimestampColumnType"/></term>
        ///         <description>datetime</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="TypeColumnName"/></term>
        ///         <description>type</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="TypeColumnType"/></term>
        ///         <description>varchar(1024)</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="PayloadColumnName"/></term>
        ///         <description>payload</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="PayloadColumnType"/></term>
        ///         <description>varchar(max)</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public EfCoreTracedAggregateEntityOptions()
        {
            TableName = "DomainEvents";
            CompositePrimaryKeyIdColumnName = "id";
            CompositePrimaryKeyIdColumnType = "uniqueidentifier";
            CompositePrimaryKeyVersionColumnName = "version";
            CompositePrimaryKeyVersionColumnType = "int";
            TimestampColumnName = "timestamp";
            TimestampColumnType = "datetime";
            TypeColumnName = "clrtype";
            TypeColumnType = "varchar(1024)";
            PayloadColumnName = "payload";
            PayloadColumnType = "varchar(max)";
        }

        /// <summary>
        /// Gets or sets the table name of the Event Sourcing schema.
        /// </summary>
        /// <value>The table name of the Event Sourcing schema.</value>
        public string TableName
        {
            get => _tableName;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value);
                _tableName = value;
            }
        }

        /// <summary>
        /// Gets or sets the identifier part of the composite PK column name of the Event Sourcing schema.
        /// </summary>
        /// <value>The identifier part of the composite PK column name of the Event Sourcing schema.</value>
        public string CompositePrimaryKeyIdColumnName
        {
            get => _compositePrimaryKeyIdColumnName;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value);
                _compositePrimaryKeyIdColumnName = value;
            }
        }

        /// <summary>
        /// Gets or sets the identifier part composite PK column type of the Event Sourcing schema.
        /// </summary>
        /// <value>The identifier part of the composite PK column type of the Event Sourcing schema.</value>
        public string CompositePrimaryKeyIdColumnType
        {
            get => _compositePrimaryKeyIdColumnType;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value);
                _compositePrimaryKeyIdColumnType = value;
            }
        }

        /// <summary>
        /// Gets or sets the version part of the composite PK column name of the Event Sourcing schema.
        /// </summary>
        /// <value>The version part of the composite PK column name of the Event Sourcing schema.</value>
        public string CompositePrimaryKeyVersionColumnName
        {
            get => _compositePrimaryKeyVersionColumnName;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value);
                _compositePrimaryKeyVersionColumnName = value;
            }
        }

        /// <summary>
        /// Gets or sets the version part composite PK column type of the Event Sourcing schema.
        /// </summary>
        /// <value>The version part of the composite PK column type of the Event Sourcing schema.</value>
        public string CompositePrimaryKeyVersionColumnType
        {
            get => _compositePrimaryKeyVersionColumnType;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value);
                _compositePrimaryKeyVersionColumnType = value;
            }
        }

        /// <summary>
        /// Gets or sets the timestamp column name of the Event Sourcing schema.
        /// </summary>
        /// <value>The timestamp column name of the Event Sourcing schema.</value>
        public string TimestampColumnName
        {
            get => _timestampColumnName;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value);
                _timestampColumnName = value;
            }
        }

        /// <summary>
        /// Gets or sets the timestamp column type of the Event Sourcing schema.
        /// </summary>
        /// <value>The timestamp column type of the Event Sourcing schema.</value>
        public string TimestampColumnType
        {
            get => _timestampColumnType;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value);
                _timestampColumnType = value;
            }
        }

        /// <summary>
        /// Gets or sets the CLR type column name of the Event Sourcing schema.
        /// </summary>
        /// <value>The CLR type column name of the Event Sourcing schema.</value>
        public string TypeColumnName
        {
            get => _typeColumnName;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value);
                _typeColumnName = value;
            }
        }

        /// <summary>
        /// Gets or sets the CLR type column type of the Event Sourcing schema.
        /// </summary>
        /// <value>The CLR type column type of the Event Sourcing schema.</value>
        public string TypeColumnType
        {
            get => _typeColumnType;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value);
                _typeColumnType = value;
            }
        }

        /// <summary>
        /// Gets or sets the payload (serialized data) column name of the Event Sourcing schema.
        /// </summary>
        /// <value>The payload (serialized data) column name of the Event Sourcing schema.</value>
        public string PayloadColumnName
        {
            get => _payloadColumnName;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value);
                _payloadColumnName = value;
            }
        }

        /// <summary>
        /// Gets or sets the payload (serialized data) column type of the Event Sourcing schema.
        /// </summary>
        /// <value>The payload (serialized data) column type of the Event Sourcing schema.</value>
        public string PayloadColumnType
        {
            get => _payloadColumnType;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value);
                _payloadColumnType = value;
            }
        }
    }
}
