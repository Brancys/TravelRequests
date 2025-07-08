using TravelRequests.Application.DTOs;
using TravelRequests.Application.Interfaces;
using TravelRequests.Domain.Entities;
using TravelRequests.Infrastructure.Persistence;
using BCrypt.Net;
using TravelRequests.Domain.Enums;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
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
