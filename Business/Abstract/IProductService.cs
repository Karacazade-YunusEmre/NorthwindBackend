using Core.Business.Abstract;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;

namespace Business.Abstract;

public interface IProductService : IService<Product>
{
    public IDataResult<List<Product>> GetProductsByCategoryId(int id);
}