using Core.Entities.Concrete.Authentication;
using Core.Entities.Concrete.Dtos;

namespace DataAccess.Abstract;

public interface IUserRepository
{
    public Task<bool> Register(RegisterDto model);
    public Task<UserToken?> Login(LoginDto model);
}