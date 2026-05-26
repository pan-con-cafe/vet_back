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
    public class CitaController : ControllerBase
    {
        private readonly VetDbContext _context;

        public CitaController(VetDbContext context)
        {
            _context = context;
        }

        // GET: api/cita
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var citas = await _context.Citas
                .Where(c => c.deleted_at == null)
                .Include(c => c.Mascota)
                .Include(c => c.TypeCita)
                .Select(c => new
                {
                    c.IdCita,
                    Date = c.Date.ToString("yyyy-MM-dd"),
                    c.Status,
                    c.TypeCita_FK,
                    Tipo = c.TypeCita.Type,
                    Mascota = new
                    {
                        c.Mascota.IdMascota,
                        c.Mascota.Name,
                        c.Mascota.Species
                    }
                })
                .ToListAsync();

            return Ok(citas);
        }

        // GET: api/cita/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cita = await _context.Citas
                .Where(c => c.IdCita == id && c.deleted_at == null)
                .Include(c => c.Mascota)
                .Include(c => c.TypeCita)
                .Select(c => new
                {
                    c.IdCita,
                    c.Date,
                    c.Status,
                    Tipo = c.TypeCita.Type,
                    Mascota = new
                    {
                        c.Mascota.IdMascota,
                        c.Mascota.Name,
                        c.Mascota.Species,
                        c.Mascota.Race
                    }
                })
                .FirstOrDefaultAsync();

            if (cita == null) return NotFound("Cita no encontrada");

            return Ok(cita);
        }

        // POST: api/cita
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CitaDTO dto)
        {
            var mascotaExiste = await _context.Mascotas
                .AnyAsync(m => m.IdMascota == dto.Mascota_FK);
            if (!mascotaExiste)
                return BadRequest("La mascota no existe");

            var tipoExiste = await _context.TypeCitas
                .AnyAsync(t => t.IdTypeCita == dto.TypeCita_FK);
            if (!tipoExiste)
                return BadRequest("El tipo de cita no existe");

            var cita = new Cita
            {
                Mascota_FK = dto.Mascota_FK,
                TypeCita_FK = dto.TypeCita_FK,
                Date = dto.Date,
                Status = "pendiente"
            };

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return Ok(new { cita.IdCita, mensaje = "Cita creada correctamente" });
        }

        // PUT: api/cita/5 — editar o reprogramar
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CitaDTO dto)
        {
            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == id && c.deleted_at == null);

            if (cita == null) return NotFound("Cita no encontrada");

            cita.Mascota_FK = dto.Mascota_FK;
            cita.TypeCita_FK = dto.TypeCita_FK;
            cita.Date = dto.Date;
            cita.Status = dto.Status;

            await _context.SaveChangesAsync();
            return Ok("Cita actualizada correctamente");
        }

        // PATCH: api/cita/5/status — cambiar solo el status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == id && c.deleted_at == null);

            if (cita == null) return NotFound("Cita no encontrada");

            var statusValidos = new[] { "pendiente", "atendida", "cancelada" };
            if (!statusValidos.Contains(status))
                return BadRequest("Status inválido. Use: pendiente, atendida o cancelada");

            cita.Status = status;
            await _context.SaveChangesAsync();

            return Ok("Status actualizado correctamente");
        }

        // DELETE: api/cita/5 — soft delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == id && c.deleted_at == null);

            if (cita == null) return NotFound("Cita no encontrada");

            cita.deleted_at = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok("Cita eliminada correctamente");
        }
    }
}
