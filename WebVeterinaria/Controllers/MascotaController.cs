using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using vet_data.Context;
using vet_domain.DTOs;
using vet_domain.Models;


namespace WebVeterinaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MascotaController : ControllerBase
    {
        private readonly VetDbContext _context;
        private readonly ILogger<MascotaController> _logger;

        public MascotaController(VetDbContext context, ILogger<MascotaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/mascota
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mascotas = await _context.Mascotas
                .Include(m => m.ClienteMascotas)
                    .ThenInclude(cm => cm.Cliente)
                .Select(m => new
                {
                    m.IdMascota,
                    m.Name,
                    m.Race,
                    m.Color,
                    m.birth_date,
                    m.Gender,
                    m.Species,
                    m.Feature,
                    m.Image,
                    Propietarios = m.ClienteMascotas.Select(cm => new
                    {
                        cm.Cliente.IdCliente,
                        cm.Cliente.NameLastname
                    }).ToList()
                })
                .ToListAsync();

            return Ok(mascotas);
        }

        // GET: api/mascota/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var mascota = await _context.Mascotas
                .Include(m => m.ClienteMascotas)
                    .ThenInclude(cm => cm.Cliente)
                        .ThenInclude(c => c.Telefonos)
                .Include(m => m.Vacunas)
                    .ThenInclude(v => v.TypeVacuna)
                .Include(m => m.Desparacitaciones)
                .Include(m => m.Cirugias)
                .Include(m => m.Groomings)
                .Include(m => m.Citas)
                    .ThenInclude(c => c.TypeCita)
                .Where(m => m.IdMascota == id)
                .Select(m => new
                {
                    m.IdMascota,
                    m.Name,
                    m.Race,
                    m.Color,
                    m.birth_date,
                    m.Gender,
                    m.Species,
                    m.Feature,
                    m.Image,
                    Propietarios = m.ClienteMascotas.Select(cm => new
                    {
                        cm.Cliente.IdCliente,
                        cm.Cliente.NameLastname,
                        cm.Cliente.Address,
                        Telefonos = cm.Cliente.Telefonos
                            .Select(t => t.PhoneNumber).ToList()
                    }).ToList(),
                    Vacunas = m.Vacunas.Select(v => new
                    {
                        v.IdVacuna,
                        v.Date,
                        v.Weight,
                        v.Temperature,
                        Tipo = v.TypeVacuna.Type
                    }).ToList(),
                    Desparacitaciones = m.Desparacitaciones.Select(d => new
                    {
                        d.IdDesparacitacion,
                        d.Date,
                        d.Product,
                        d.Weight,
                        d.Dose
                    }).ToList(),
                    Cirugias = m.Cirugias.Select(c => new
                    {
                        c.IdCirugia,
                        c.Description,
                        Date = c.Date.ToString("yyyy-MM-dd")
                    }).ToList(),
                    Groomings = m.Groomings.Select(g => new
                    {
                        g.IdGrooming,
                        g.Date,
                        g.Entry,
                        g.Exit,
                        g.Amount,
                        g.OnCredit,
                        g.Residue,
                        g.Haircut
                    }).ToList(),
                    Citas = m.Citas
                        .Where(c => c.deleted_at == null)
                        .Select(c => new
                        {
                            c.IdCita,
                            c.Date,
                            c.Status,
                            Tipo = c.TypeCita.Type
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (mascota == null) return NotFound("Mascota no encontrada");

            return Ok(mascota);
        }

        // POST: api/mascota
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MascotaDTO dto)
        {
            _logger.LogInformation("Payload recibido: {@dto}", dto);

            var mascota = new Mascota
            {
                Name = dto.Name,
                Race = dto.Race,
                Color = dto.Color,
                birth_date = dto.birth_date,
                Gender = dto.Gender,
                Species = dto.Species,
                Feature = dto.Feature,
                Image = dto.Image
            };

            _context.Mascotas.Add(mascota);
            await _context.SaveChangesAsync();

            foreach (var clienteId in dto.ClienteIds)
            {
                var clienteExiste = await _context.Clientes
                    .AnyAsync(c => c.IdCliente == clienteId);

                if (!clienteExiste)
                    return BadRequest($"Cliente con id {clienteId} no existe");

                _context.ClienteMascotas.Add(new ClienteMascota
                {
                    Mascota_FK = mascota.IdMascota,
                    Cliente_FK = clienteId
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { mascota.IdMascota, mensaje = "Mascota creada correctamente" });
        }

        // PUT: api/mascota/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MascotaDTO dto)
        {
            var mascota = await _context.Mascotas
                .Include(m => m.ClienteMascotas)
                .FirstOrDefaultAsync(m => m.IdMascota == id);

            if (mascota == null) return NotFound("Mascota no encontrada");

            mascota.Name = dto.Name;
            mascota.Race = dto.Race;
            mascota.Color = dto.Color;
            mascota.birth_date = dto.birth_date;
            mascota.Gender = dto.Gender;
            mascota.Species = dto.Species;
            mascota.Feature = dto.Feature;
            mascota.Image = dto.Image;

            // Reemplazar propietarios
            _context.ClienteMascotas.RemoveRange(mascota.ClienteMascotas);
            foreach (var clienteId in dto.ClienteIds)
            {
                _context.ClienteMascotas.Add(new ClienteMascota
                {
                    Mascota_FK = mascota.IdMascota,
                    Cliente_FK = clienteId
                });
            }

            await _context.SaveChangesAsync();
            return Ok("Mascota actualizada correctamente");
        }

        // DELETE: api/mascota/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var mascota = await _context.Mascotas
                .Include(m => m.ClienteMascotas)
                .FirstOrDefaultAsync(m => m.IdMascota == id);

            if (mascota == null) return NotFound("Mascota no encontrada");

            _context.ClienteMascotas.RemoveRange(mascota.ClienteMascotas);
            _context.Mascotas.Remove(mascota);
            await _context.SaveChangesAsync();

            return Ok("Mascota eliminada correctamente");
        }
    }
}
