using System;
using System.Linq.Expressions;

namespace NotesDomain
{
    public interface IFilterCriteria<TEntity> where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>> FilterExprssion{ get;  }
    }
}