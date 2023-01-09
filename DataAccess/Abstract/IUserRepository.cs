using Core.Utilities.Results.Abstract;
using Entities.Concrete.Authentication;
using Entities.Concrete.ViewModel;

namespace DataAccess.Abstract;

public interface IUserRepository
{
    public Task<bool> Register(RegisterViewModel model);
    public Task<UserToken?> Login(LoginViewModel model);
}