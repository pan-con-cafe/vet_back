using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.Models
{
    public class Mascota
    {
        public int IdMascota { get; set; }
        public string Name { get; set; } = null!;
        public string? Race { get; set; }
        public string? Color { get; set; }
        public DateOnly? birth_date { get; set; }
        public bool Gender { get; set; }
        public bool Species { get; set; }
        public string? Feature { get; set; }
        public string? Image { get; set; }
        public ICollection<ClienteMascota> ClienteMascotas { get; set; } = new List<ClienteMascota>();
        public ICollection<Vacuna> Vacunas { get; set; } = new List<Vacuna>();
        public ICollection<Desparacitacion> Desparacitaciones { get; set; } = new List<Desparacitacion>();
        public ICollection<Cirugia> Cirugias { get; set; } = new List<Cirugia>();
        public ICollection<Grooming> Groomings { get; set; } = new List<Grooming>();
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
