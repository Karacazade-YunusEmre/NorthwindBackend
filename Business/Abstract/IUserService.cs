using Core.Utilities.Results.Abstract;
using Entities.Concrete.Authentication;
using Entities.Concrete.ViewModel;

namespace Business.Abstract;

public interface IUserService
{
    public Task<IResult> Register(RegisterViewModel model);
    public Task<IDataResult<UserToken?>> Login(LoginViewModel model);
}