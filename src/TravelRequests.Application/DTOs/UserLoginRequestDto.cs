namespace TravelRequests.Application.DTOs;

public class UserLoginRequestDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
