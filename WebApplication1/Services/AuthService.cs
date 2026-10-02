using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class AuthService : IAuthService
{

    private readonly JwtTokenService _jwtTokenService;
    private readonly ApplicationDbContext _context;

    public AuthService(
    ApplicationDbContext context,
    JwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<User> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var user = new User
        {
            FullName = registerDto.FullName,
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Role = "Customer"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return null;
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(
            password,
            user.PasswordHash);

        if (!isPasswordValid)
        {
            return null;
        }

        return user;
    }
}