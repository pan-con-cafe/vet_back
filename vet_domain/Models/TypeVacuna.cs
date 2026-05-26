using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.Models
{
    public class TypeVacuna
    {
        public int IdTypeVacuna { get; set; }
        public string Type { get; set; } = null!;
        public ICollection<Vacuna> Vacunas { get; set; } = new List<Vacuna>();
    }
}
