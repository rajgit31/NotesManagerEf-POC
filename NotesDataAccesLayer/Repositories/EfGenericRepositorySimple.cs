using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using NotesDomainInterfaces;

namespace NotesDataAccesLayer.Repositories
{
   
    public class EfGenericRepositorySimple<TEntity> : IRepositorySimple<TEntity> where TEntity : class 
    {
        private readonly IDbContextSimple _context;
        private readonly IDbSet<TEntity> _dbSet;
        

        public EfGenericRepositorySimple(IDbContextSimple context)
        {
            this._context = context;
            this._dbSet = context.Set<TEntity>();
        }

        internal IDbSet<TEntity> InternalSet
        {
            get
            {
                return _dbSet;
            }
        }


        public IList<TEntity> All()
        {
            return _dbSet.ToList();
        }

        public IList<TEntity> All(Expression<Func<TEntity, bool>> predicate)
        {
           
            return _dbSet.Where(predicate).ToList();
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            var entity = _dbSet.Find(keyValues);
            return entity;
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Attach(TEntity entity)
        {
            _dbSet.Attach(entity);
        }

        
        public void Add(IList<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

      
        public void Update(TEntity entity)
        {
            SetContext(entity);
        }

      
        public void Delete(TEntity entity)
        {
            SetContext(entity);
        }

     
        public void DeleteWithChildren(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public TEntity TestEagerLoad()
        {
            var entity = _dbSet.Include("Passport");
            var firstOrDefault = entity.FirstOrDefault();
            return firstOrDefault;
        }

        private void SetContext(TEntity entity)
        {
            var entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
            {
                TEntity attachedEntity = _dbSet.Local.FirstOrDefault();

                if (attachedEntity != null)
                {
                    // If the identity is already attached, rather set the state values
                    var attachedEntry = _context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
            }
        }
    }
}