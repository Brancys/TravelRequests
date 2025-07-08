using TravelRequests.Application.DTOs;

namespace TravelRequests.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegisterRequestDto dto);
    }
}
