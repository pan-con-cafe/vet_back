using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.DTOs
{
    public class DesparacitacionDTO
    {
        public int Mascota_FK { get; set; }
        public string? Product { get; set; }
        public DateOnly Date { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Dose { get; set; }
    }
}
