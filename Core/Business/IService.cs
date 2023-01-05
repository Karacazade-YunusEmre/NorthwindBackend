using Core.Entities;
using Core.Utilities.Results.Abstract;

namespace Core.Business;

public interface IService<TEntity> where TEntity : class, IEntity, new()
{
    public IDataResult<List<TEntity>> GetAll();
    public IDataResult<List<TEntity>> GetByName(string name);
    public IDataResult<TEntity?> GetById(int id);
    public IResult Add(TEntity entity);
    public IResult Update(TEntity entity);
    public IResult Delete(int id);
}