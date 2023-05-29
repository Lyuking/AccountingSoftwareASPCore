using Microsoft.EntityFrameworkCore;
using AccountingSoftware.Models;

namespace AccountingSoftware.Models
{
      public class AppDBContext:DbContext
      { 
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { Database.EnsureCreated(); }
        public DbSet<Audience> Audiences { get;set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Licence> Licences { get; set; }
        public DbSet<LicenceDetails> LicenceDetailses { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<SoftwareTechnicalDetails> SoftwareTechnicalDetailses { get; set; }
        public DbSet<SubjectArea> SubjectAreas { get; set; }
        public DbSet<LicenceType> LicenceType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Audience>()
                .HasMany<Computer>(c => c.Computers)
                .WithOne(s => s.Audience)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<SubjectArea>()
                .HasMany<SoftwareTechnicalDetails>(c => c.SoftwareTechnicalDetailses)
                .WithOne(s => s.SubjectArea)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Employee>()
                .HasMany<Licence>(c => c.Licences)
                .WithOne(s => s.Employee)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<LicenceType>()
                .HasMany<Licence>(c => c.Licences)
                .WithOne(s => s.LicenceType)
                .OnDelete(DeleteBehavior.SetNull);
           
             
        }
    }
}
