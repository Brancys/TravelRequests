using TravelRequests.Domain.Enums;

namespace TravelRequests.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public UserRole Role { get; set; } = UserRole.Solicitante;

    public ICollection<TravelRequest> TravelRequests { get; set; } = new List<TravelRequest>();
}
