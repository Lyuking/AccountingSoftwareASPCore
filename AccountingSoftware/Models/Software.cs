namespace AccountingSoftware.Models
{
    public class Software
    {
        public int Id { get; set; }
        public int SoftwareTechnicalDetailsId { get; set; }
        public SoftwareTechnicalDetails? SoftwareTechnicalDetails { get; set; }
        public int? LicenceId { get; set; }
        public Licence? Licence { get; set; }
        public ICollection<Computer> Computers { get; set; }
        public Software() { Computers = new List<Computer>(); }
    }
}
