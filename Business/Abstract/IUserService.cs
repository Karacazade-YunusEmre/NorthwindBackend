using Core.Entities.Concrete.Authentication;
using Core.Entities.Concrete.Dtos;
using Core.Utilities.Results.Abstract;

namespace Business.Abstract;

public interface IUserService
{
    public Task<IResult> Register(RegisterDto model);
    public Task<IDataResult<UserToken?>> Login(LoginDto model);
    public Task<IResult> Logout(LogoutDto model);
}