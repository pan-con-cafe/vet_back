using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.Models
{
    public class Desparacitacion
    {
        public int IdDesparacitacion { get; set; }
        public int Mascota_FK { get; set; }
        public string? Product { get; set; }
        public DateOnly Date { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Dose { get; set; }
        public Mascota Mascota { get; set; } = null!;
    }
}
