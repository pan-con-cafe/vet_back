using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.Models
{
    public class Grooming
    {
        public int IdGrooming { get; set; }
        public int Mascota_FK { get; set; }
        public TimeOnly? Entry { get; set; }
        public TimeOnly? Exit { get; set; }
        public DateOnly Date { get; set; }
        public decimal? Amount { get; set; }
        public decimal? OnCredit { get; set; }
        public decimal? Residue { get; set; }
        public string? Haircut { get; set; }
        public Mascota Mascota { get; set; } = null!;
    }
}
