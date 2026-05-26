using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.DTOs
{
    public class CitaDTO
    {
        public int Mascota_FK { get; set; }
        public int TypeCita_FK { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; } = "pendiente";
    }
}
