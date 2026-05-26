using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.DTOs
{
    public class ClienteDTO
    {
        public string NameLastname { get; set; } = null!;
        public string? Address { get; set; }
        public List<string> Telefonos { get; set; } = new();
    }
}
