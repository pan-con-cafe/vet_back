using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.Models
{
    public class TypeCita
    {
        public int IdTypeCita { get; set; }
        public string Type { get; set; } = null!;
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
