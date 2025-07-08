namespace TravelRequests.Application.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string Email { get; set; } = default!;
}
