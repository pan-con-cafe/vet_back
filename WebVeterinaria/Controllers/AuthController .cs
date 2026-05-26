using Microsoft.AspNetCore.Mvc;
using vet_data.Context;
using vet_domain.DTOs;
using vet_domain.Helpers;
using vet_domain.Services;
using Microsoft.EntityFrameworkCore;

namespace WebVeterinaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly VetDbContext _context;
        private readonly AuthService _authService;

        public AuthController(VetDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !PasswordHelper.Verify(dto.Password, user.Password))
                return Unauthorized("Credenciales incorrectas");

            var token = _authService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}