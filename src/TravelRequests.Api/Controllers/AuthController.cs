using Microsoft.AspNetCore.Mvc;
using TravelRequests.Application.DTOs;
using TravelRequests.Application.Interfaces;
using System.Security.Claims;


namespace TravelRequests.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto dto)
        {
            var success = await _userService.RegisterUserAsync(dto);
            if (!success)
                return BadRequest("El correo ya está registrado.");

            return Ok("Usuario registrado correctamente.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto dto)
        {
            var result = await _userService.LoginAsync(dto);
            if (result == null)
                return Unauthorized("Credenciales inválidas");

            return Ok(result);
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Email = email,
                Role = role
            });
        }


    }
}
