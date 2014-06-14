using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using NotesDomain;
using NotesDomainInterfaces;
using EntityState = System.Data.Entity.EntityState;

namespace NotesDataAccesLayer.Repositories
{
    
    /// <summary>
    /// The implementation of an IRepository, which is an Entity framework repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public class EfGenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IDbContext _context;
        private readonly IDbSet<TEntity> _dbSet;
        private readonly IFilterCriteria<TEntity> _filterCriteria;
        private bool _withCriteria = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfGenericRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filterCriteria">The filter criteria.</param>
        public EfGenericRepository(IDbContext context, IFilterCriteria<TEntity> filterCriteria)
        {
            this._filterCriteria = filterCriteria;
            this._context = context;
            this._dbSet = context.Set<TEntity>();
        }

        public bool WithCriteria
        {
            get
            {
                return _withCriteria;
            }
            set
            {
                _withCriteria = value;
            }
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IList&lt;TEntity&gt;.</returns>
        public IList<TEntity> All()
        {
            if (_withCriteria)
            {
                return _dbSet.Where(_filterCriteria.FilterExprssion).ToList();
            }

            return _dbSet.ToList();
        }

        /// <summary>
        /// Alls the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IList&lt;TEntity&gt;.</returns>
        public IList<TEntity> All(Expression<Func<TEntity, bool>> predicate)
        {
            if (_withCriteria)
            {
                return _dbSet.Where(_filterCriteria.FilterExprssion).Where(predicate).ToList();
            }

            return _dbSet.Where(predicate).ToList(); 
        }

        /// <summary>
        /// Finds the specified key values.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Find(params object[] keyValues)
        {
            var entity =  _dbSet.Find(keyValues);
            return entity;
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>TEntity.</returns>
        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            if (_withCriteria)
            {
                return _dbSet.Where(_filterCriteria.FilterExprssion).FirstOrDefault<TEntity>(predicate);
            }

            return _dbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public int Add(TEntity entity)
        {
            TEntity add = _dbSet.Add(entity);
            return add.Id;
        }

        /// <summary>
        /// Adds the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Add(IList<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(TEntity entity) 
        {
            SetContext(entity);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(TEntity entity)
        {
            SetContext(entity);
        }

        /// <summary>
        /// Deletes the with children.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void DeleteWithChildren(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        private void SetContext(TEntity entity)
        {
            var entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                TEntity attachedEntity = _dbSet.Local.SingleOrDefault(e => e.Id.Equals(entity.Id));

                if (attachedEntity != null)
                {
                    // If the identity is already attached, rather set the state values
                    var attachedEntry = _context.Entry(attachedEntity);
                    attachedEntry.Entity.EntityState = entity.EntityState;
                    attachedEntry.CurrentValues.SetValues(entity);
                }
            }
        }
    }
}