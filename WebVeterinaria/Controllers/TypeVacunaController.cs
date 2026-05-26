using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vet_data.Context;


namespace WebVeterinaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TypeVacunaController : ControllerBase
    {
        private readonly VetDbContext _context;

        public TypeVacunaController(VetDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tipos = await _context.TypeVacunas
                .Select(t => new { t.IdTypeVacuna, t.Type })
                .ToListAsync();

            return Ok(tipos);
        }
    }
}
