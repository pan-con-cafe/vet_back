using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.DTOs
{
    public class MascotaDTO
    {
        public string Name { get; set; } = null!;
        public string? Race { get; set; }
        public string? Color { get; set; }
        public DateOnly? birth_date { get; set; }
        public bool Gender { get; set; }
        public bool Species { get; set; }
        public string? Feature { get; set; }
        public string? Image { get; set; }
        public List<int> ClienteIds { get; set; } = new();
    }
}
