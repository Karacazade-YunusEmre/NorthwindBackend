using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Concrete.Authentication;
using Entities.Concrete.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Library;

namespace WebApi.Controllers;

public class UsersController : ControllerBase
{
    private readonly NorthwindContext _context;
    private readonly IConfiguration _config;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(NorthwindContext context, IConfiguration config, SignInManager<User> signInManager,
        UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _config = config;
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
    {
        var responseViewModel = new ResponseViewModel();

        try
        {
            #region Validate

            if (!ModelState.IsValid)
            {
                responseViewModel.IsSuccess = false;
                responseViewModel.Message = "Bilgileriniz eksik, bazı alanlar gönderilmemiş. " +
                                            "Lütfen tüm alanları doldurunuz.";

                return BadRequest(responseViewModel);
            }

            var existsUser = await _userManager.FindByNameAsync(model.Email);

            if (existsUser != null)
            {
                responseViewModel.IsSuccess = false;
                responseViewModel.Message = "Kullanıcı zaten var.";

                return BadRequest(responseViewModel);
            }

            #endregion

            // Kullanıcı bilgileri set edilir.
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email.Trim(),
                UserName = model.Email.Trim()
            };

            // Kullanıcı oluşturulur.
            var result = await _userManager.CreateAsync(user, model.Password.Trim());

            // Kullanıcı oluşturuldu ise
            if (result.Succeeded)
            {
                var roleExists = await _roleManager.RoleExistsAsync(_config["Roles:User"]!);

                if (!roleExists)
                {
                    var role = new IdentityRole(_config["Roles:User"]!)
                    {
                        NormalizedName = _config["Roles:User"],
                    };

                    _roleManager.CreateAsync(role).Wait();
                }

                // Kullanıcıya ilgili rol ataması yapılır.
                _userManager.AddToRoleAsync(user, _config["Roles:User"]!).Wait();

                responseViewModel.IsSuccess = true;
                responseViewModel.Message = "Kullanıcı başarılı şekilde oluşturuldu.";
            }
            else
            {
                responseViewModel.IsSuccess = false;
                responseViewModel.Message = $"Kullanıcı oluşturulurken bir hata oluştu: " +
                                            $"{result.Errors.FirstOrDefault()?.Description}";
            }

            return Ok(responseViewModel);
        }
        catch (Exception ex)
        {
            responseViewModel.IsSuccess = false;
            responseViewModel.Message = ex.Message;

            return BadRequest(responseViewModel);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody] LoginViewModel model)
    {
        var responseViewModel = new ResponseViewModel();

        try
        {
            #region Validate

            if (ModelState.IsValid == false)
            {
                responseViewModel.IsSuccess = false;
                responseViewModel.Message = "Bilgileriniz eksik, bazı alanlar gönderilmemiş. " +
                                            "Lütfen tüm alanları doldurunuz.";
                return BadRequest(responseViewModel);
            }

            // Kulllanıcı bulunur.
            var user = await _userManager.FindByNameAsync(model.Email);

            // Kullanıcı yoksa;
            if (user == null)
            {
                return Unauthorized();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(
                user: user,
                password: model.Password,
                isPersistent: false,
                lockoutOnFailure: false);
            
            // Kullanıcı bulunamadı ise
            if (signInResult.Succeeded == false)
            {
                responseViewModel.IsSuccess = false;
                responseViewModel.Message = "Kullanıcı adı veya şifre hatalı.";

                return Unauthorized(responseViewModel);
            }

            #endregion

            var signedUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);

            var accessTokenGenerator = new AccessTokenGenerator(_context, _config, signedUser!);
            var userTokens = accessTokenGenerator.GetToken();

            responseViewModel.IsSuccess = true;
            responseViewModel.Message = "Kullanıcı giriş yaptı.";
            responseViewModel.TokenInfo = new TokenInfo
            {
                Token = userTokens.Value!,
                ExpireDate = userTokens.ExpireDate
            };

            return Ok(responseViewModel);
        }
        catch (Exception ex)
        {
            responseViewModel.IsSuccess = false;
            responseViewModel.Message = ex.Message;

            return BadRequest(responseViewModel);
        }
    }
}