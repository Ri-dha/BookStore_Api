using BookStoreTask.Users.Payloads;
using BookStoreTask.Users.Service;
using BookStoreTask.Utli;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTask.Users.Controller;

[Route("auth/")]
public class AuthController : BaseController
{
    private readonly IUserServices _userServices;

    public AuthController(IUserServices userServices)
    {
        _userServices = userServices;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginForm loginForm)
    {
        var (userDto, error) = await _userServices.Login(loginForm);
        if (error != null)
        {
            return BadRequest(new { error });
        }

        return Ok(userDto);
    }

    [HttpPost("register-customer")]
    [Consumes("multipart/form-data")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterCustomer([FromForm] CustomerForm customerForm)
    {
        var (user, error) = await _userServices.RegisterCustomer(customerForm, customerForm.ProfileImage);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpPost("register-admin")]
    [Consumes("multipart/form-data")]
    [Authorize(Policy = "ManagerPolicy")]
    public async Task<IActionResult> RegisterAdmin([FromForm] AdminForm adminForm)
    {
        var (user, error) = await _userServices.RegisterAdmin(adminForm, adminForm.ProfileImage);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }
    
    
}