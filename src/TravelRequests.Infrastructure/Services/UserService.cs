using TravelRequests.Application.DTOs;
using TravelRequests.Application.Interfaces;
using TravelRequests.Domain.Entities;
using TravelRequests.Infrastructure.Persistence;
using BCrypt.Net;
using TravelRequests.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TravelRequests.Infrastructure.Services;


public class UserService : IUserService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;


    public UserService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<AuthResponseDto?> LoginAsync(UserLoginRequestDto dto)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == dto.Email);
        if (user == null) return null;

        bool validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!validPassword) return null;

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Role = user.Role.ToString(),
            Email = user.Email
        };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _config.GetSection("JwtSettings");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> RegisterUserAsync(UserRegisterRequestDto dto)
    {
        if (_db.Users.Any(u => u.Email == dto.Email)) return false;

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        if (!Enum.TryParse<UserRole>(dto.Role, true, out var parsedRole))
            throw new ArgumentException("Rol inválido");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = hashedPassword,
            Role = parsedRole
        };


        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return true;
    }

}
