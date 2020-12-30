using AdOut.Extensions.Repositories;
using AdOut.Planning.Model.Interfaces.Managers;
using System;

namespace AdOut.Planning.Core.Managers
{

    //todo: delete the basemanager because the manager is useless
    public abstract class BaseManager<TEntity> : IBaseManager<TEntity> where TEntity : PersistentEntity
    {
        private readonly IBaseRepository<TEntity> _repository;
        public BaseManager(IBaseRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public void Create(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _repository.Create(entity);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _repository.Delete(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _repository.Update(entity);
        }
    }
}
