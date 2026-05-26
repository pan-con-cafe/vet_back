using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vet_data.Context;
using vet_domain.DTOs;
using vet_domain.Models;


namespace WebVeterinaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GroomingController : ControllerBase
    {
        private readonly VetDbContext _context;

        public GroomingController(VetDbContext context)
        {
            _context = context;
        }

        // GET: api/grooming/mascota/5
        [HttpGet("mascota/{mascotaId}")]
        public async Task<IActionResult> GetByMascota(int mascotaId)
        {
            var groomings = await _context.Groomings
                .Where(g => g.Mascota_FK == mascotaId)
                .Select(g => new
                {
                    g.IdGrooming,
                    g.Date,
                    g.Entry,
                    g.Exit,
                    g.Amount,
                    g.OnCredit,
                    g.Residue,
                    g.Haircut
                })
                .ToListAsync();

            return Ok(groomings);
        }

        // GET: api/grooming/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var grooming = await _context.Groomings
                .Where(g => g.IdGrooming == id)
                .Include(g => g.Mascota)
                .Select(g => new
                {
                    g.IdGrooming,
                    g.Date,
                    g.Entry,
                    g.Exit,
                    g.Amount,
                    g.OnCredit,
                    g.Residue,
                    g.Haircut,
                    Mascota = new
                    {
                        g.Mascota.IdMascota,
                        g.Mascota.Name
                    }
                })
                .FirstOrDefaultAsync();

            if (grooming == null) return NotFound("Grooming no encontrado");

            return Ok(grooming);
        }

        // POST: api/grooming
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroomingDTO dto)
        {
            var mascotaExiste = await _context.Mascotas
                .AnyAsync(m => m.IdMascota == dto.Mascota_FK);
            if (!mascotaExiste)
                return BadRequest("La mascota no existe");

            var grooming = new Grooming
            {
                Mascota_FK = dto.Mascota_FK,
                Entry = dto.Entry,
                Exit = dto.Exit,
                Date = dto.Date,
                Amount = dto.Amount,
                OnCredit = dto.OnCredit,
                Residue = dto.Residue,
                Haircut = dto.Haircut
            };

            _context.Groomings.Add(grooming);
            await _context.SaveChangesAsync();

            return Ok(new { grooming.IdGrooming, mensaje = "Grooming registrado correctamente" });
        }

        // PUT: api/grooming/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GroomingDTO dto)
        {
            var grooming = await _context.Groomings
                .FirstOrDefaultAsync(g => g.IdGrooming == id);

            if (grooming == null) return NotFound("Grooming no encontrado");

            grooming.Mascota_FK = dto.Mascota_FK;
            grooming.Entry = dto.Entry;
            grooming.Exit = dto.Exit;
            grooming.Date = dto.Date;
            grooming.Amount = dto.Amount;
            grooming.OnCredit = dto.OnCredit;
            grooming.Residue = dto.Residue;
            grooming.Haircut = dto.Haircut;

            await _context.SaveChangesAsync();
            return Ok("Grooming actualizado correctamente");
        }

        // DELETE: api/grooming/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var grooming = await _context.Groomings
                .FirstOrDefaultAsync(g => g.IdGrooming == id);

            if (grooming == null) return NotFound("Grooming no encontrado");

            _context.Groomings.Remove(grooming);
            await _context.SaveChangesAsync();

            return Ok("Grooming eliminado correctamente");
        }
    }
}
