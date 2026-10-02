using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(RegisterDto registerDto);

    Task<User?> LoginAsync(string email, string password);
}