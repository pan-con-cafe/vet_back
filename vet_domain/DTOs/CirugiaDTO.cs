using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.DTOs
{
    public class CirugiaDTO
    {
        public int Mascota_FK { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
    }
}
