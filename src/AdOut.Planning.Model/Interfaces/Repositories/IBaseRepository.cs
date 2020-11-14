using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        ValueTask<TEntity> GetByIdAsync(params object[] id);

        //todo: delete this method!
        IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> predicate);
    }
}
