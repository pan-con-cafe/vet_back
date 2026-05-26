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
    public class CirugiaController : ControllerBase
    {
        private readonly VetDbContext _context;

        public CirugiaController(VetDbContext context)
        {
            _context = context;
        }

        // GET: api/cirugia/mascota/5
        [HttpGet("mascota/{mascotaId}")]
        public async Task<IActionResult> GetByMascota(int mascotaId)
        {
            var cirugias = await _context.Cirugias
                .Where(c => c.Mascota_FK == mascotaId)
                .Select(c => new
                {
                    c.IdCirugia,
                    c.Description,
                    Date = c.Date.ToString("yyyy-MM-dd")
                })
                .ToListAsync();

            return Ok(cirugias);
        }

        // GET: api/cirugia/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cirugia = await _context.Cirugias
                .Where(c => c.IdCirugia == id)
                .Include(c => c.Mascota)
                .Select(c => new
                {
                    c.IdCirugia,
                    c.Description,
                    Date = c.Date.ToString("yyyy-MM-dd"),
                    Mascota = new
                    {
                        c.Mascota.IdMascota,
                        c.Mascota.Name
                    }
                })
                .FirstOrDefaultAsync();

            if (cirugia == null) return NotFound("Cirugia no encontrada");

            return Ok(cirugia);
        }

        // POST: api/cirugia
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CirugiaDTO dto)
        {
            var mascotaExiste = await _context.Mascotas
                .AnyAsync(m => m.IdMascota == dto.Mascota_FK);
            if (!mascotaExiste)
                return BadRequest("La mascota no existe");

            var cirugia = new Cirugia
            {
                Mascota_FK = dto.Mascota_FK,
                Description = dto.Description,
                Date = dto.Date
            };

            _context.Cirugias.Add(cirugia);
            await _context.SaveChangesAsync();

            return Ok(new { cirugia.IdCirugia, mensaje = "Cirugia registrada correctamente" });
        }

        // PUT: api/cirugia/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CirugiaDTO dto)
        {
            var cirugia = await _context.Cirugias
                .FirstOrDefaultAsync(c => c.IdCirugia == id);

            if (cirugia == null) return NotFound("Cirugia no encontrada");

            cirugia.Mascota_FK = dto.Mascota_FK;
            cirugia.Description = dto.Description;
            cirugia.Date = dto.Date;

            await _context.SaveChangesAsync();
            return Ok("Cirugia actualizada correctamente");
        }

        // DELETE: api/cirugia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cirugia = await _context.Cirugias
                .FirstOrDefaultAsync(c => c.IdCirugia == id);

            if (cirugia == null) return NotFound("Cirugia no encontrada");

            _context.Cirugias.Remove(cirugia);
            await _context.SaveChangesAsync();

            return Ok("Cirugia eliminada correctamente");
        }
    }
}
