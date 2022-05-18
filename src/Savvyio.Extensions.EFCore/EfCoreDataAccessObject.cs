﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;
using Savvyio.Data;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Represents the base class from which all implementations of <see cref="EfCoreDataAccessObject{T}"/> should derive. This is an abstract class to serve as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <seealso cref="Disposable"/>
    /// <seealso cref="IPersistentDataAccessObject{T,TOptions}" />
    public abstract class EfCoreDataAccessObject<T> : IPersistentDataAccessObject<T, EfCoreOptions<T>> where T : class
    {
        /// <summary>
        /// Creates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to create in the associated <seealso cref="IEfCoreDataStore"/>.</param>
        /// <param name="setup">The <see cref="EfCoreOptions{T}"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public abstract Task CreateAsync(T dto, Action<EfCoreOptions<T>> setup = null);

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to update in the associated <seealso cref="IEfCoreDataStore"/>.</param>
        /// <param name="setup">The <see cref="EfCoreOptions{T}"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public abstract Task UpdateAsync(T dto, Action<EfCoreOptions<T>> setup = null);
        
        /// <summary>
        /// Finds an object from the specified <paramref name="setup"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="setup">The <see cref="EfCoreOptions{T}"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching object of the operation or <c>null</c> if no match was found.</returns>
        public abstract Task<T> ReadAsync(Action<EfCoreOptions<T>> setup = null);
        
        /// <summary>
        /// Finds all objects matching the specified <paramref name="setup"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="setup">The <see cref="EfCoreOptions{T}"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        public abstract Task<IEnumerable<T>> ReadAllAsync(Action<EfCoreOptions<T>> setup = null);
        
        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to delete in the associated <seealso cref="IEfCoreDataStore"/>.</param>
        /// <param name="setup">The <see cref="EfCoreOptions{T}"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public abstract Task DeleteAsync(T dto, Action<EfCoreOptions<T>> setup = null);
    }
}
