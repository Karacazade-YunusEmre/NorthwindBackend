using Business.Abstract;
using Core.Entities.Concrete.Authentication;
using Core.Entities.Concrete.Dtos;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;

namespace Business.Concrete;

public class UserManager : IUserService
{
    private readonly IUserRepository _repository;

    public UserManager(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IResult> Register(RegisterDto model)
    {
        try
        {
            var result = await _repository.Register(model);
            return result;
        }
        catch (Exception exception)
        {
            return new ErrorResult("Kullanıcı oluşturma işlemi sırasında hata oluştu" +
                                   $" {exception.Message}");
        }
    }

    public async Task<IDataResult<UserToken?>> Login(LoginDto model)
    {
        try
        {
            var result = await _repository.Login(model);

            return result;
        }
        catch (Exception exception)
        {
            return new ErrorDataResult<UserToken?>(data: null, message: "Kullanıcı giriş işlemi sırasında" +
                                                                        " hata oluştu." +
                                                                        $" {exception.Message}");
        }
    }

    public async Task<IResult> Logout(LogoutDto model)
    {
        try
        {
            var result = await _repository.Logout(model);

            return result;
        }
        catch (Exception exception)
        {
            return new ErrorResult($"Çıkış işlemi sırasında hata oluştu. {exception.Message}");
        }
    }
}