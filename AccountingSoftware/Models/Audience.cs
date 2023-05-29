using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSoftware.Models
{
    public class Audience
    {
        public int Id { get; set; }
        [Display(Name = "Наименование"), Required]
        public string Name { get; set; }
        public ICollection<Computer> Computers { get; set; }
        public Audience() { Computers = new List<Computer>(); }
        [NotMapped]
        public bool? fromComputers { get; set; }
    }
}
