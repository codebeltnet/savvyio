using System;
using System.Linq.Expressions;
using Cuemon.Threading;
using DapperExtensions;
using DapperExtensions.Predicate;

namespace Savvyio.Extensions.DapperExtensions
{
    /// <summary>
    /// Specifies options that is related to <see cref="DapperExtensionsDataStore{T}"/>.
    /// </summary>
    /// <seealso cref="AsyncOptions" />
    public class DapperExtensionsQueryOptions<T> : AsyncOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperExtensionsQueryOptions{T}"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="DapperExtensionsQueryOptions{T}"/>.
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
        ///         <term><see cref="Op"/></term>
        ///         <description><see cref="Operator.Eq"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Value"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Not"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="UseColumnPrefix"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Function"/></term>
        ///         <description><see cref="DatabaseFunction.None"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="FunctionParameters"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AsyncOptions.CancellationToken"/></term>
        ///         <description><c>default</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public DapperExtensionsQueryOptions()
        {
            Op = Operator.Eq;
            Not = false;
            UseColumnPrefix = true;
            Function = DatabaseFunction.None;
        }

        /// <summary>
        /// Gets or sets the predicate of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>.
        /// </summary>
        /// <value>The predicate of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>..</value>
        public Expression<Func<T, object>> Predicate { get; set; }

        /// <summary>
        /// Gets or sets the comparison operator of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>.
        /// </summary>
        /// <value>The comparison operator of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>.</value>
        public Operator Op { get; set; }

        /// <summary>
        /// Gets or sets the value of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>.
        /// </summary>
        /// <value>The value of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>.</value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets a value that inverts the comparision operator of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>.
        /// </summary>
        /// <value><c>true</c> to invert the value of <see cref="Op"/>; otherwise, <c>false</c>.</value>
        public bool Not { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether generated SQL should have column prefix of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>.
        /// </summary>
        /// <value><c>true</c> if to include column prefix on generated SQL; otherwise, <c>false</c>.</value>
        public bool UseColumnPrefix { get; set; }

        /// <summary>
        /// Gets or sets the database function of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>.
        /// </summary>
        /// <value>The database function of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>.</value>
        public DatabaseFunction Function { get; set; }

        /// <summary>
        /// Gets or sets the parameters to the <see cref="Function"/> of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>.
        /// </summary>
        /// <value>The parameters to the <see cref="Function"/> of the <see cref="Predicates.Field{T}(Expression{Func{T,object}},Operator,object,bool,bool,DatabaseFunction,string)"/>.</value>
        public string FunctionParameters { get; set; }
    }
}
