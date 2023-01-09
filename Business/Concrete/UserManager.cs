using Business.Abstract;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using Entities.Concrete.Authentication;
using Entities.Concrete.ViewModel;

namespace Business.Concrete;

public class UserManager : IUserService
{
    private readonly IUserRepository _repository;

    public UserManager(IUserRepository repository)
    {
        _repository = repository;
    }

    public Task<IResult> Register(RegisterViewModel model)
    {
        return _repository.Register(model);
    }

    public Task<IDataResult<UserToken?>> Login(LoginViewModel model)
    {
        return _repository.Login(model);
    }
}