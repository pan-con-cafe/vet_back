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
    public class DesparacitacionController : ControllerBase
    {
        private readonly VetDbContext _context;

        public DesparacitacionController(VetDbContext context)
        {
            _context = context;
        }

        // GET: api/desparacitacion/mascota/5
        [HttpGet("mascota/{mascotaId}")]
        public async Task<IActionResult> GetByMascota(int mascotaId)
        {
            var desparacitaciones = await _context.Desparacitaciones
                .Where(d => d.Mascota_FK == mascotaId)
                .Select(d => new
                {
                    d.IdDesparacitacion,
                    d.Date,
                    d.Product,
                    d.Weight,
                    d.Dose
                })
                .ToListAsync();

            return Ok(desparacitaciones);
        }

        // GET: api/desparacitacion/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var desparacitacion = await _context.Desparacitaciones
                .Where(d => d.IdDesparacitacion == id)
                .Include(d => d.Mascota)
                .Select(d => new
                {
                    d.IdDesparacitacion,
                    d.Date,
                    d.Product,
                    d.Weight,
                    d.Dose,
                    Mascota = new
                    {
                        d.Mascota.IdMascota,
                        d.Mascota.Name
                    }
                })
                .FirstOrDefaultAsync();

            if (desparacitacion == null) return NotFound("Desparacitacion no encontrada");

            return Ok(desparacitacion);
        }

        // POST: api/desparacitacion
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DesparacitacionDTO dto)
        {
            var mascotaExiste = await _context.Mascotas
                .AnyAsync(m => m.IdMascota == dto.Mascota_FK);
            if (!mascotaExiste)
                return BadRequest("La mascota no existe");

            var desparacitacion = new Desparacitacion
            {
                Mascota_FK = dto.Mascota_FK,
                Product = dto.Product,
                Date = dto.Date,
                Weight = dto.Weight,
                Dose = dto.Dose
            };

            _context.Desparacitaciones.Add(desparacitacion);
            await _context.SaveChangesAsync();

            return Ok(new { desparacitacion.IdDesparacitacion, mensaje = "Desparacitacion registrada correctamente" });
        }

        // PUT: api/desparacitacion/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DesparacitacionDTO dto)
        {
            var desparacitacion = await _context.Desparacitaciones
                .FirstOrDefaultAsync(d => d.IdDesparacitacion == id);

            if (desparacitacion == null) return NotFound("Desparacitacion no encontrada");

            desparacitacion.Mascota_FK = dto.Mascota_FK;
            desparacitacion.Product = dto.Product;
            desparacitacion.Date = dto.Date;
            desparacitacion.Weight = dto.Weight;
            desparacitacion.Dose = dto.Dose;

            await _context.SaveChangesAsync();
            return Ok("Desparacitacion actualizada correctamente");
        }

        // DELETE: api/desparacitacion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var desparacitacion = await _context.Desparacitaciones
                .FirstOrDefaultAsync(d => d.IdDesparacitacion == id);

            if (desparacitacion == null) return NotFound("Desparacitacion no encontrada");

            _context.Desparacitaciones.Remove(desparacitacion);
            await _context.SaveChangesAsync();

            return Ok("Desparacitacion eliminada correctamente");
        }
    }
}
