using Business.Abstract;
using Entities.Concrete.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _user;

    public UsersController(IUserService user)
    {
        _user = user;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        var result = await _user.Register(model);

        if (result.Success)
        {
            return Ok(result.Message);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        var result = await _user.Login(model);
        if (result.Success)
        {
            return Ok(result.Data!);
        }

        return Unauthorized(result.Message);
    }
}