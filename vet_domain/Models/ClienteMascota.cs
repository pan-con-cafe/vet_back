using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.Models
{
    public class ClienteMascota
    {
        public int IdClienteMascota { get; set; }
        public int Mascota_FK { get; set; }
        public int Cliente_FK { get; set; }
        public Mascota Mascota { get; set; } = null!;
        public Cliente Cliente { get; set; } = null!;
    }
}
