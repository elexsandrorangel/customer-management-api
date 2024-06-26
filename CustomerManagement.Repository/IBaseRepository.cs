﻿using CustomerManagement.Models;
using System.Linq.Expressions;

namespace CustomerManagement.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        #region Add

        /// <summary>
        /// Inserts a single object to the database and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="entity">The object to insert</param>
        /// <returns>The resulting object including its primary key after the insert</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Inserts a collection of objects into the database and commits the changes
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="entities">An IEnumerable list of objects to insert</param>
        /// <returns>The IEnumerable resulting list of inserted objects including the primary keys</returns>
        Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities);

        #endregion Add

        #region Count

        /// <summary>
        /// Gets the count of the number of objects in the databse
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>The count of the number of objects</returns>
        Task<long> CountAsync();

        /// <summary>
        /// Gets the count of the number of objects in the databse
        /// </summary>
        /// <param name="predicate">Lambda expression</param>
        /// <returns>Total of objects</returns>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion Count

        #region Remove

        /// <summary>
        /// Deletes a single object from the database and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="id">The primary key of the object to remove</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Deletes a single object from the database and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="entity">The object to delete</param>
        Task DeleteAsync(TEntity entity);

        #endregion Remove

        bool Exists(Expression<Func<TEntity, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        #region Get

        /// <summary>
        /// Gets a collection of all objects in the database
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>An IEnumerable of every object in the database</returns>
        Task<IEnumerable<TEntity>> GetAsync(int page = 0, int qty = int.MaxValue, bool track = false);

        /// <summary>
        /// Returns a collection of objects which match the provided expression
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        /// <returns>An IEnumerable of object which match the expression filter</returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> match, int page = 0, int qty = int.MaxValue, bool track = false);

        /// <summary>
        /// Returns a single object with a primary key of the provided id
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="id">The primary key of the object to fetch</param>
        /// <returns>A single object with the provided primary key or null</returns>
        Task<TEntity?> GetAsync(Guid id, bool track = false);

        /// <summary>
        /// Returns a single object which matches the provided expression
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="match">A Linq expression filter to find a single result</param>
        /// <returns>A single object which matches the expression filter.
        /// If more than one object is found or if zero are found, null is returned</returns>
        Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> match, bool track = false);

        #endregion Get

        #region Save

        /// <summary>
        /// Save changes in the database
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>The number of state entries written to the underlying database. This can include</returns>
        Task<int> SaveAsync();


        /// <summary>
        /// Create or update a single object based on the provided primary key and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="entity">Object to create or update</param>
        /// <returns>The resulting updated object</returns>
        Task<TEntity> SaveOrUpdateAsync(TEntity entity);

        #endregion Save

        #region Update

        /// <summary>
        /// Updates a single object based on the provided primary key and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="updated">The updated object to apply to the database</param>
        /// <returns>The resulting updated object</returns>
        Task<TEntity> UpdateAsync(TEntity updated);

        /// <summary>
        /// Updates a single object based on the provided primary key and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="updated">The updated object to apply to the database</param>
        /// <param name="key">The primary key of the object to update</param>
        /// <returns>The resulting updated object</returns>
        Task<TEntity> UpdateAsync(TEntity updated, Guid key);

        #endregion Update
    }
}