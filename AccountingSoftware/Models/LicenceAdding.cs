using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
namespace AccountingSoftware.Models
{
    [NotMapped]
    public class LicenceAdding
    {
        public int? softwareTechnicalDetailsId { get; set; }
        [Display(Name = "Программное обеспечение")]
        public SoftwareTechnicalDetails? softwareTechnicalDetails { get; set; }
        public int? licenceTypeId { get; set; }
        [Display(Name = "Тип лицензии")]
        public LicenceType? licenceType { get; set; }
        public int? employeeId { get; set; }
        [Display(Name = "Сотрудник")]
        public Employee? employee { get; set; }
        public int Id { get; set; }
        [Display(Name = "Ключ")]
        public string? Key { get; set; }
        [Display(Name = "Дата начала")]
        public DateTime DateStart { get; set; }
        [Display(Name = "Дата окончания")]
        public DateTime DateEnd { get; set; }
        [Display(Name = "Цена")]
        public float Price { get; set; }
        [Display(Name = "Количество")]
        public int Count { get; set; }
    }
}