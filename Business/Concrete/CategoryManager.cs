using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class CategoryManager : ICategoryManager
{
    private readonly ICategoryRepository _repository;

    public CategoryManager(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public List<Category> GetAll()
    {
        return _repository.GetAll();
    }

    public List<Category> GetByName(string name)
    {
        return _repository.GetAll(c => c.Name.ToLower().Contains(name.ToLower()));
    }

    public Category? GetById(int id)
    {
        return _repository.Get(c => c.Id == id);
    }

    public void Add(Category entity)
    {
        _repository.Add(entity);
    }

    public void Update(Category entity)
    {
        _repository.Update(entity);
    }

    public void Delete(Category entity)
    {
        _repository.Delete(entity);
    }
}