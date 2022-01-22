﻿using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way of abstracting deletable data access (cruD).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <seealso cref="IDataAccessObject{T}"/>
    public interface IDeletableDataAccessObject<in T> : IDataAccessObject<T> where T : class
    {
        /// <summary>
        /// Deletes the specified <paramref name="dto"/> from a data store asynchronous.
        /// </summary>
        /// <param name="dto">The DTO to delete from a data store.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task DeleteAsync(T dto, Action<AsyncOptions> setup = null);
    }
}
