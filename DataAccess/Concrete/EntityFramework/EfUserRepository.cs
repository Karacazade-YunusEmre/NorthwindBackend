using Core.Entities.Concrete.Authentication;
using Core.Entities.Concrete.Dtos;
using Core.Library.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
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
    private readonly ITokenGenerator _tokenGenerator;

    public EfUserRepository(RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
        SignInManager<User> signInManager, IConfiguration configuration, NorthwindContext context,
        ITokenGenerator tokenGenerator)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<IResult> Register(RegisterDto model)
    {
        // Kulllanıcı bulunur.
        var existsUser = await _userManager.FindByEmailAsync(model.Email);

        // Kullanıcı kayıtlıysa;
        if (existsUser != null)
        {
            return new ErrorResult(message: "Kullanıcı zaten mevcut");
        }

        var newUser = new User()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email.Trim(),
            UserName = model.Email.Trim(),
        };

        // Kullanıcı oluşturulur.
        var result = await _userManager.CreateAsync(newUser, model.Password.Trim());

        // Kullanıcı oluşturulamadı ise
        if (!result.Succeeded)
            return new ErrorResult(message: $"Kullanıcı oluşturulamadı. {result}");

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

        return new SuccessResult("Kullanıcı başarıyla oluşturuldu");
    }

    public async Task<IDataResult<UserToken?>> Login(LoginDto model)
    {
        // Mevcut kullanıcı bulunur
        var existsUser = await _userManager.FindByEmailAsync(model.Email);

        // Eğer kullanıcı bulunamadıysa
        if (existsUser == null)
            return new ErrorDataResult<UserToken?>(message: "Kullanıcı bulunamadı", data: null);

        var signInResult = await _signInManager.PasswordSignInAsync(
            user: existsUser,
            password: model.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        // Kullanıcı giriş yapamadıysa
        if (!signInResult.Succeeded)
            return new ErrorDataResult<UserToken?>(message: "Kullanıcı giriş işlemi başarısız. " +
                                                            $"{signInResult}", data: null);

        var signedUser = _context.Users.FirstOrDefault(u => u.Id == existsUser.Id);

        if (signedUser == null)
            return new ErrorDataResult<UserToken?>(message: "Kullanıcı giriş işlemi başarısız. " +
                                                            $"{signedUser}", data: null);

        // Token oluşturulur
        var userToken = await _tokenGenerator.GetToken<NorthwindContext>(signedUser);

        return new SuccessDataResult<UserToken?>(message: "Kullanıcı giriş işlemi başarılı",
            data: userToken);
    }

    public async Task<IResult> Logout(LogoutDto model)
    {
        // Mevcut kullanıcı bulunur
        var existsUser = await _userManager.FindByEmailAsync(model.Email);

        // Eğer kullanıcı bulunamadıysa
        if (existsUser == null)
            return new ErrorResult(message: "Kullanıcı bulunamadı!");

        await _signInManager.SignOutAsync();
        return new SuccessResult(message: "Çıkış işlemi başarılı");
    }
}