using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.Models
{
    public class Vacuna
    {
        public int IdVacuna { get; set; }
        public int Mascota_FK { get; set; }
        public int TypeVacuna_FK { get; set; }
        public DateOnly Date { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Temperature { get; set; }
        public Mascota Mascota { get; set; } = null!;
        public TypeVacuna TypeVacuna { get; set; } = null!;
    }
}
