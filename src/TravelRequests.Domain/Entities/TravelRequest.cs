namespace TravelRequests.Domain.Entities;

public class TravelRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string OriginCity { get; set; } = null!;
    public string DestinationCity { get; set; } = null!;
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public string Justification { get; set; } = null!;
    public string Status { get; set; } = "Pendiente"; // "Pendiente", "Aprobada", "Rechazada"

    // Relaciones
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
