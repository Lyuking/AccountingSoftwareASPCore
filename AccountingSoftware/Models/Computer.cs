using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace AccountingSoftware.Models
{
    
    public class Computer
    {
        public int Id { get; set; }
        [Display(Name = "Номер компьютера"), Required]
        public string Number { get; set; }
        public int? AudienceId { get; set; }
        [Display(Name = "Аудитория")]
        public Audience? Audience { get; set; }
        [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$", ErrorMessage = "IP должен соответствовать IPv4")]
        [Display(Name = "IP-адрес")]
        public string? IpAdress { get; set; }
        [Display(Name = "Процессор"), Required]
        public string? Processor { get; set; }
        [Display(Name = "Видеокарта"), Required]
        public string? Videocard { get; set; }
        [Display(Name = "Оперативная память"), Required]
        public string? RAM { get; set; }
        [Display(Name = "Всего места на диске"), Required]
        public string? TotalSpace { get; set; }

        public ICollection<Software> Softwares { get; set; }
        public Computer() { Softwares= new List<Software>(); }
    }
}
