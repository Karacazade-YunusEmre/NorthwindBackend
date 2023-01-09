using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
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

    public async Task<IResult> Register(RegisterViewModel model)
    {
        try
        {
            var result = await _repository.Register(model);
            if (result)
                return new SuccessResult("Kullanıcı başarıyla oluşturuldu");

            return new ErrorResult("Kullanıcı oluşturulamadı");
        }
        catch (Exception exception)
        {
            return new ErrorResult("Kullanıcı oluşturma işlemi sırasında hata oluştu" +
                                   $" {exception.Message}");
        }
    }

    public async Task<IDataResult<UserToken?>> Login(LoginViewModel model)
    {
        try
        {
            var result = await _repository.Login(model);

            if (result == null)
                return new ErrorDataResult<UserToken?>(message: "Kullanıcı giriş işlemi başarısız oldu!", data: null);

            return new SuccessDataResult<UserToken?>(message: "Kullanıcı giriş işlemi başarılı.", data: result);
        }
        catch (Exception exception)
        {
            return new ErrorDataResult<UserToken?>(data: null, message: "Kullanıcı giriş işlemi sırasında" +
                                                                        " hata oluştu." +
                                                                        $" {exception.Message}");
        }
    }
}