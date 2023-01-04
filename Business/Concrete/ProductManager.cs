using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class ProductManager : IProductManager
{
    private readonly IProductRepository _repository;

    public ProductManager(IProductRepository repository)
    {
        _repository = repository;
    }

    public List<Product> GetAll()
    {
        return _repository.GetAll();
    }

    public List<Product> GetByName(string name)
    {
        return _repository.GetAll(p => p.Name.ToLower().Contains(name.ToLower()));
    }

    public Product? GetById(int id)
    {
        return _repository.Get(p => p.Id == id);
    }

    public void Add(Product entity)
    {
        _repository.Add(entity);
    }

    public void Update(Product entity)
    {
        _repository.Update(entity);
    }

    public void Delete(int id)
    {
        _repository.Delete(new Product()
        {
            Id = id,
        });
    }
}