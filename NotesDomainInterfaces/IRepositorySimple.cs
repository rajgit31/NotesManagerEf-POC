using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NotesDomainInterfaces
{
    /// <summary>
    /// Summary description for IRepository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public interface IRepositorySimple<TEntity> where TEntity : class 
    {
        IList<TEntity> All();

        IList<TEntity> All(Expression<Func<TEntity, bool>> predicate);

        TEntity Find(params object[] keyValues);

        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void Attach(TEntity entity);

        void Add(IList<TEntity> entities);

        void Update(TEntity entity);

        void Delete(TEntity entity);
        
        void DeleteWithChildren(TEntity entity);

        TEntity TestEagerLoad();
    }
}