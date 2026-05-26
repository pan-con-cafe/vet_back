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
    public class ClienteController : ControllerBase
    {
        private readonly VetDbContext _context;

        public ClienteController(VetDbContext context)
        {
            _context = context;
        }

        // GET: api/cliente
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Telefonos)
                .Include(c => c.ClienteMascotas)
                    .ThenInclude(cm => cm.Mascota)
                .Select(c => new
                {
                    c.IdCliente,
                    c.NameLastname,
                    c.Address,
                    Telefonos = c.Telefonos.Select(t => t.PhoneNumber).ToList(),
                    Mascotas = c.ClienteMascotas.Select(cm => new
                    {
                        cm.Mascota.IdMascota,
                        cm.Mascota.Name
                    }).ToList()
                })
                .ToListAsync();

            return Ok(clientes);
        }

        // GET: api/cliente/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Telefonos)
                .Include(c => c.ClienteMascotas)
                    .ThenInclude(cm => cm.Mascota)
                .Where(c => c.IdCliente == id)
                .Select(c => new
                {
                    c.IdCliente,
                    c.NameLastname,
                    c.Address,
                    Telefonos = c.Telefonos.Select(t => t.PhoneNumber).ToList(),
                    Mascotas = c.ClienteMascotas.Select(cm => new
                    {
                        cm.Mascota.IdMascota,
                        cm.Mascota.Name,
                        cm.Mascota.Species,
                        cm.Mascota.Race
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (cliente == null) return NotFound("Cliente no encontrado");

            return Ok(cliente);
        }

        // POST: api/cliente
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClienteDTO dto)
        {
            var cliente = new Cliente
            {
                NameLastname = dto.NameLastname,
                Address = dto.Address
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            // Agregar teléfonos
            foreach (var numero in dto.Telefonos)
            {
                _context.ClienteTelefonos.Add(new ClienteTelefono
                {
                    Cliente_FK = cliente.IdCliente,
                    PhoneNumber = numero
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { cliente.IdCliente, mensaje = "Cliente creado correctamente" });
        }

        // PUT: api/cliente/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteDTO dto)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Telefonos)
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null) return NotFound("Cliente no encontrado");

            cliente.NameLastname = dto.NameLastname;
            cliente.Address = dto.Address;

            // Reemplazar teléfonos
            _context.ClienteTelefonos.RemoveRange(cliente.Telefonos);
            foreach (var numero in dto.Telefonos)
            {
                _context.ClienteTelefonos.Add(new ClienteTelefono
                {
                    Cliente_FK = cliente.IdCliente,
                    PhoneNumber = numero
                });
            }

            await _context.SaveChangesAsync();
            return Ok("Cliente actualizado correctamente");
        }

        // DELETE: api/cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Telefonos)
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null) return NotFound("Cliente no encontrado");

            _context.ClienteTelefonos.RemoveRange(cliente.Telefonos);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok("Cliente eliminado correctamente");
        }
    }
}
