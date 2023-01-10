using Core.DataAccess;
using Core.DataAccess.Abstract;
using Entities.Concrete;

namespace DataAccess.Abstract;

public interface IProductRepository : IEntityRepository<Product>
{
}