using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vet_data.Context;


namespace WebVeterinaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TypeCitaController : ControllerBase
    {
        private readonly VetDbContext _context;

        public TypeCitaController(VetDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tipos = await _context.TypeCitas
                .Select(t => new { t.IdTypeCita, t.Type })
                .ToListAsync();

            return Ok(tipos);
        }
    }
}
