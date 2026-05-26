using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.Models
{
    public class Cirugia
    {
        public int IdCirugia { get; set; }
        public int Mascota_FK { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public Mascota Mascota { get; set; } = null!;
    }
}
