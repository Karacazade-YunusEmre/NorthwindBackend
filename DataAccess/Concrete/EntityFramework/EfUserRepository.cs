using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Library;
using Entities.Concrete.Authentication;
using Entities.Concrete.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Concrete.EntityFramework;

public class EfUserRepository : IUserRepository
{
    private readonly NorthwindContext _context;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public EfUserRepository(RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
        SignInManager<User> signInManager, IConfiguration configuration, NorthwindContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

    public async Task<IResult> Register(RegisterViewModel model)
    {
        try
        {
            // Kulllanıcı bulunur.
            var existsUser = await _userManager.FindByEmailAsync(model.Email);

            // Kullanıcı yoksa;
            if (existsUser != null)
            {
                return new ErrorResult(message: "Kullanıcı zaten mevcut!");
            }

            var newUser = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email.Trim(),
                UserName = model.Email.Trim()
            };

            // Kullanıcı oluşturulur.
            var result = await _userManager.CreateAsync(newUser, model.Password.Trim());

            // Kullanıcı oluşturulamadı ise
            if (!result.Succeeded)
                return new ErrorResult(message: "Kullanıcı oluşturulma işlemi sırasında hata oluştu" +
                                                $" {result.Errors.FirstOrDefault()?.Description}");

            var roleExists = await _roleManager.RoleExistsAsync(_configuration["Roles:User"]!);

            // Rol mevcut değilse
            if (!roleExists)
            {
                var newRole = new IdentityRole(_configuration["Roles:User"]!)
                {
                    NormalizedName = _configuration["Roles:User"],
                };
                _roleManager.CreateAsync(newRole).Wait();
            }

            // Kullanıcıya ilgili rol ataması yapılır.
            _userManager.AddToRoleAsync(newUser, _configuration["Roles:User"]!).Wait();

            return new SuccessResult(message: "Kullanıcı başarıyla oluşturuldu");
        }
        catch (Exception exception)
        {
            return new ErrorResult(message: "Kullanıcı oluşturulma işlemi sırasında" +
                                            $"hata oluştu. {exception.Message}");
        }
    }

    public async Task<IDataResult<UserToken?>> Login(LoginViewModel model)
    {
        try
        {
            // Mevcut kullanıcı bulunur
            var existsUser = await _userManager.FindByEmailAsync(model.Email);

            // Eğer kullanıcı bulunamadıysa
            if (existsUser == null)
                return new ErrorDataResult<UserToken?>(data: null, message: "Kullanıcı bulunamadı");

            var signInResult = await _signInManager.PasswordSignInAsync(
                user: existsUser,
                password: model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            // Kullanıcı giriş yapamadıysa
            if (!signInResult.Succeeded)
                return new ErrorDataResult<UserToken?>(data: null, message: "Kullanıcı bilgileri yanlış");

            var signedUser = _context.Users.FirstOrDefault(u => u.Id == existsUser.Id);

            var accessTokenGenerator = new AccessTokenGenerator(_context, _configuration, signedUser!);
            // Token oluşturulur
            var userToken = accessTokenGenerator.GetToken();

            return new SuccessDataResult<UserToken?>(data: userToken, message: "Kullanıcı başarıyla " +
                                                                               "giriş yaptı");
        }
        catch (Exception exception)
        {
            return new ErrorDataResult<UserToken?>(data: null, message: "Kullanıcı giriş işlemi " +
                                                                        "sırasında hata oluştu!" +
                                                                        $" {exception.Message}");
        }
    }
}