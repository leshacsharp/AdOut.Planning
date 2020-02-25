using System;
using System.Linq;
using System.Linq.Expressions;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> predicate);
    }
}
