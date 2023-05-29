using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSoftware.Models
{
    public class SoftwareTechnicalDetails
    {
        public int Id { get; set; }
        [Display(Name = "Наименование ПО"), Required]
        public string Name { get; set; }
        public int? SubjectAreaId { get; set; }
        [Display(Name = "Предметная область")]
        public SubjectArea? SubjectArea { get; set; }
        [Display(Name = "Описание ПО")]
        public string? Description { get; set; }
        [Display(Name = "Требуемое место на диске")]
        public string? RequiredSpace { get; set; }
        [Display(Name = "Логотип")]
        public string? Photo { get; set; }
        public ICollection<Software> Softwares { get; set; }
        public SoftwareTechnicalDetails() { Softwares= new List<Software>(); }

    }
}
