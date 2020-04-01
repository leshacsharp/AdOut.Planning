namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IBaseManager<TEntity> where TEntity : class
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
