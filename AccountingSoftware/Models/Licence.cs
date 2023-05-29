using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AccountingSoftware.Models
{
    public class Licence
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        [Display(Name = "Ответственный работник")]
        public Employee? Employee { get; set; }
        public int? LicenceTypeId { get; set; }
        [Display(Name = "Тип лицензии")]
        public LicenceType? LicenceType { get; set; }
        public int LicenceDetailsId { get; set; }
        [Display(Name = "Детальная информация")]
        public LicenceDetails? LicenceDetails { get; set; }
        public ICollection<Software> Softwares { get; set; }
        public Licence() { Softwares= new List<Software>(); }
        
    }
}
