using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class ProductManager : IProductService
{
    private readonly IProductRepository _repository;

    public ProductManager(IProductRepository repository)
    {
        _repository = repository;
    }

    public IDataResult<List<Product>> GetAll()
    {
        try
        {
            return new SuccessDataResult<List<Product>>(_repository.GetAll());
        }
        catch (Exception exception)
        {
            return new ErrorDataResult<List<Product>>(data: new List<Product>(), message: exception.Message);
        }
    }

    public IDataResult<List<Product>> GetByName(string name)
    {
        return new SuccessDataResult<List<Product>>(_repository.GetAll(p => p.Name.ToLower().Contains(name.ToLower())));
    }

    public IDataResult<Product?> GetById(int id)
    {
        return new SuccessDataResult<Product?>(_repository.Get(p => p.Id == id));
    }

    public IDataResult<List<Product>> GetProductsByCategoryId(int id)
    {
        return new SuccessDataResult<List<Product>>(_repository.GetAll(p => p.CategoryId == id).ToList());
    }

    public IResult Add(Product entity)
    {
        return new SuccessResult(Messages.ProductAdded);
    }

    public IResult Update(Product entity)
    {
        return new SuccessResult(Messages.ProductUpdated);
    }

    public IResult Delete(int id)
    {
        return new SuccessResult(Messages.ProductDeleted);
    }
}