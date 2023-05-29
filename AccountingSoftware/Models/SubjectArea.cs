using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AccountingSoftware.Models
{

    public class SubjectArea
    {
        public int Id { get; set; }
        [Display(Name = "Наименование"), Required]
        public string Name { get; set; }
        public ICollection<SoftwareTechnicalDetails> SoftwareTechnicalDetailses { get; set; }
        public SubjectArea() { SoftwareTechnicalDetailses = new List<SoftwareTechnicalDetails>(); }
        [NotMapped]
        public bool? fromSoftwareTechnicalDetails { get; set; }
    }
}
