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
    public class VacunaController : ControllerBase
    {
        private readonly VetDbContext _context;

        public VacunaController(VetDbContext context)
        {
            _context = context;
        }

        // GET: api/vacuna/mascota/5
        [HttpGet("mascota/{mascotaId}")]
        public async Task<IActionResult> GetByMascota(int mascotaId)
        {
            var vacunas = await _context.Vacunas
                .Where(v => v.Mascota_FK == mascotaId)
                .Include(v => v.TypeVacuna)
                .Select(v => new
                {
                    v.IdVacuna,
                    v.Date,
                    v.Weight,
                    v.Temperature,
                    Tipo = v.TypeVacuna.Type
                })
                .ToListAsync();

            return Ok(vacunas);
        }

        // GET: api/vacuna/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vacuna = await _context.Vacunas
                .Where(v => v.IdVacuna == id)
                .Include(v => v.TypeVacuna)
                .Include(v => v.Mascota)
                .Select(v => new
                {
                    v.IdVacuna,
                    v.Date,
                    v.Weight,
                    v.Temperature,
                    Tipo = v.TypeVacuna.Type,
                    Mascota = new
                    {
                        v.Mascota.IdMascota,
                        v.Mascota.Name
                    }
                })
                .FirstOrDefaultAsync();

            if (vacuna == null) return NotFound("Vacuna no encontrada");

            return Ok(vacuna);
        }

        // POST: api/vacuna
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VacunaDTO dto)
        {
            var mascotaExiste = await _context.Mascotas
                .AnyAsync(m => m.IdMascota == dto.Mascota_FK);
            if (!mascotaExiste)
                return BadRequest("La mascota no existe");

            var tipoExiste = await _context.TypeVacunas
                .AnyAsync(t => t.IdTypeVacuna == dto.TypeVacuna_FK);
            if (!tipoExiste)
                return BadRequest("El tipo de vacuna no existe");

            var vacuna = new Vacuna
            {
                Mascota_FK = dto.Mascota_FK,
                TypeVacuna_FK = dto.TypeVacuna_FK,
                Date = dto.Date,
                Weight = dto.Weight,
                Temperature = dto.Temperature
            };

            _context.Vacunas.Add(vacuna);
            await _context.SaveChangesAsync();

            return Ok(new { vacuna.IdVacuna, mensaje = "Vacuna registrada correctamente" });
        }

        // PUT: api/vacuna/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VacunaDTO dto)
        {
            var vacuna = await _context.Vacunas
                .FirstOrDefaultAsync(v => v.IdVacuna == id);

            if (vacuna == null) return NotFound("Vacuna no encontrada");

            vacuna.Mascota_FK = dto.Mascota_FK;
            vacuna.TypeVacuna_FK = dto.TypeVacuna_FK;
            vacuna.Date = dto.Date;
            vacuna.Weight = dto.Weight;
            vacuna.Temperature = dto.Temperature;

            await _context.SaveChangesAsync();
            return Ok("Vacuna actualizada correctamente");
        }

        // DELETE: api/vacuna/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vacuna = await _context.Vacunas
                .FirstOrDefaultAsync(v => v.IdVacuna == id);

            if (vacuna == null) return NotFound("Vacuna no encontrada");

            _context.Vacunas.Remove(vacuna);
            await _context.SaveChangesAsync();

            return Ok("Vacuna eliminada correctamente");
        }
    }
}