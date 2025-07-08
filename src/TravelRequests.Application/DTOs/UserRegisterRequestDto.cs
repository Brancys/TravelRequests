namespace TravelRequests.Application.DTOs;

public class UserRegisterRequestDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = "Solicitante";
}
