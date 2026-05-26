using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string NameLastname { get; set; } = null!;
        public string? Address { get; set; }
        public ICollection<ClienteTelefono> Telefonos { get; set; } = new List<ClienteTelefono>();
        public ICollection<ClienteMascota> ClienteMascotas { get; set; } = new List<ClienteMascota>();
    }
}
