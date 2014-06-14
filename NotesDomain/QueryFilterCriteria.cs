using System;
using System.Linq.Expressions;

namespace NotesDomain
{
    public class QueryFilterCriteria<TEntity> : IFilterCriteria<TEntity> where TEntity : BaseEntity
    {
        public QueryFilterCriteria()
        {
            
        }

        public Expression<Func<TEntity, bool>> FilterExprssion
        {
            get
            {
                return (entity => (entity).IsActive && !(entity).MarkAsDeleted);
            }
        }
    }
}