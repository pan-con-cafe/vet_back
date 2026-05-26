using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vet_domain.DTOs
{
    public class GroomingDTO
    {
        public int Mascota_FK { get; set; }
        public TimeOnly? Entry { get; set; }
        public TimeOnly? Exit { get; set; }
        public DateOnly Date { get; set; }
        public decimal? Amount { get; set; }
        public decimal? OnCredit { get; set; }
        public decimal? Residue { get; set; }
        public string? Haircut { get; set; }
    }
}
