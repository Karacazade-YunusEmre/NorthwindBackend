using Core.Entities;

namespace Core.Business;

public interface IManager<TEntity> where TEntity : class, IEntity, new()
{
    public List<TEntity> GetAll();
    public List<TEntity> GetByName(string name);
    public TEntity? GetById(int id);
    public void Add(TEntity entity);
    public void Update(TEntity entity);
    public void Delete(TEntity entity);
}