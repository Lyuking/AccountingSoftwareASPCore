using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSoftware.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Display(Name = "Имя"), Required]
        public string Name { get; set; }
        [Display(Name = "Фамилия"), Required]
        public string Surname { get; set; }
        [Display(Name = "Отчество")]
        public string? Patronymic { get; set; }
        public ICollection<Licence> Licences { get; set;}
        public Employee() { Licences = new List<Licence>(); }
        [NotMapped]
        public bool? fromSelectEmployee { get; set; }
        [NotMapped]
        [Display(Name = "ФИО ответственного лица")]
        public string FullName { get { return $"{this.Name} {this.Surname} {this.Patronymic?? ""}"; } }
    }
}
