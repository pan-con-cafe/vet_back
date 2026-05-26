using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.Models
{
    public class Cita
    {
        public int IdCita { get; set; }
        public int Mascota_FK { get; set; }
        public int TypeCita_FK { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; } = "pendiente";
        public DateTime? deleted_at { get; set; }
        public Mascota Mascota { get; set; } = null!;
        public TypeCita TypeCita { get; set; } = null!;
    }
}
