using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Potresi.Models
{
    public class Nudim
    {
        public int Id { get; set; }
        public string Opis { get; set; }
        public string Lokacija { get; set; }
        public bool Aktivno { get; set; }
        public DateTime Vrijeme { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
    }
}
