using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Xml.Linq;

namespace AccountingSoftware.Models
{
    public class LicenceDetails 
    {
        public int Id { get; set; }
        [Display(Name = "Ключ")]
        [Required]
        public string Key { get; set; }
        [Required]
        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        
        public DateTime DateStart { get; set; }
        [Required]
        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        
        
        public DateTime DateEnd { get; set; }
        [Display(Name = "Цена")]
        public float Price { get; set; }
        [Display(Name = "Количество"), Required]
        public int Count { get; set; }

        public ICollection<Licence> Licences { get; set; }
        public LicenceDetails() { Licences = new List<Licence>(); }       
    }
}
