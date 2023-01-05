using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class CategoryManager : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryManager(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public IDataResult<List<Category>> GetAll()
    {
        return new SuccessDataResult<List<Category>>(_repository.GetAll());
    }

    public IDataResult<List<Category>> GetByName(string name)
    {
        return new SuccessDataResult<List<Category>>(_repository.GetAll(c =>
            c.Name.ToLower().Contains(name.ToLower())));
    }

    public IDataResult<Category?> GetById(int id)
    {
        return new SuccessDataResult<Category?>(_repository.Get(c => c.Id == id));
    }

    public IResult Add(Category entity)
    {
        return new SuccessResult("Ürün başarıyla eklendi");
    }

    public IResult Update(Category entity)
    {
        return new SuccessResult("Ürün başarıyla güncellendi");
    }

    public IResult Delete(int id)
    {
        return new SuccessResult("Ürün başarıyla silindi");
    }
}