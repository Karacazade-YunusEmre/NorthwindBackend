using Core.Entities.Concrete.Authentication;
using Core.Entities.Concrete.Dtos;
using Core.Utilities.Results.Abstract;

namespace DataAccess.Abstract;

public interface IUserRepository
{
    public Task<IResult> Register(RegisterDto model);
    public Task<IDataResult<UserToken?>> Login(LoginDto model);
    public Task<IResult> Logout(LogoutDto model);
}