using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSoftware.Models.ViewModels

{
    public class AudiencesDetailsViewModel
    {
        public Audience Audience { get; set; }
        public IEnumerable<Computer> Computers { get;set; }
    }
}
