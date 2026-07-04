using EcommerceAPI.DTOs;
using EcommerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        try
        {
            var user = _authService.Register(dto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var token = _authService.Login(dto);

        if (token == null)
            return Unauthorized("Invalid email or password.");

        return Ok(new
        {
            Token = token
        });
    }
}