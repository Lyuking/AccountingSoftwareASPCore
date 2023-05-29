using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AccountingSoftware.Models
{
    public class LicenceType
    {
        public int Id { get; set; }
        [Display(Name = "Тип лицензии"), Required]
        public string Name { get; set; }
        public ICollection<Licence> Licences { get; set; }
        public LicenceType() { Licences = new List<Licence>(); }
        [NotMapped]
        public bool? fromSelectLicenceType { get; set; }
    }
}
