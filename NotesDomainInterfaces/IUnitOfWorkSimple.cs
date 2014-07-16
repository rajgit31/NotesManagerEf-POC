using System;

namespace NotesDomainInterfaces
{
    /// <summary>
    /// Interface IUnitOfWork
    /// </summary>
    public interface IUnitOfWorkSimple : IDisposable
    {
        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int Save();
        /// <summary>
        /// Repositories this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>IRepository&lt;TEntity&gt;.</returns>
        IRepositorySimple<TEntity> Repository<TEntity>() where TEntity : class ;
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// Commits this instance.
        /// </summary>
        void Commit();
        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        void Rollback();
    }
}