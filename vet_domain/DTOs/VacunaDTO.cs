using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.DTOs
{
    public class VacunaDTO
    {
        public int Mascota_FK { get; set; }
        public string? TypeName { get; set; }
        public DateOnly Date { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Temperature { get; set; }
    }
}
