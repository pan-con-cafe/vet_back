using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.Models
{
    public class ClienteTelefono
    {
        public int IdClienteTelefono { get; set; }
        public int Cliente_FK { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public Cliente Cliente { get; set; } = null!;
    }
}
