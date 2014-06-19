using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NotesDomain;

namespace NotesDomainInterfaces
{
    /// <summary>
    /// Summary description for IRepository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        bool WithCriteria { get; set; }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        IList<TEntity> All();

        /// <summary>
        /// Find list of entities by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IList<TEntity> All(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Finds the specified key values.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns>TEntity.</returns>
        TEntity Find(params object[] keyValues);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(TEntity entity);
        /// <summary>
        /// Adds the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Add(IList<TEntity> entities);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);
        /// <summary>
        /// Deletes the with children.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void DeleteWithChildren(TEntity entity);
    }
}