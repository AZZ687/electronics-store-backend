using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly JwtTokenService _jwtTokenService;
    private readonly IAuthService _authService;

    public AuthController(
    IAuthService authService,
    JwtTokenService jwtTokenService)
    {
        _authService = authService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        try
        {
            var user = await _authService.RegisterAsync(registerDto);

            return Ok(new
            {
                message = "User registered successfully",
                userId = user.Id,
                fullName = user.FullName,
                email = user.Email,
                role = user.Role
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _authService.LoginAsync(
            loginDto.Email,
            loginDto.Password);

        if (user == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var token = _jwtTokenService.CreateToken(user);

        return Ok(new
        {
            message = "Login successful",
            token = token,
            userId = user.Id,
            fullName = user.FullName,
            email = user.Email,
            role = user.Role
        });
    }
}