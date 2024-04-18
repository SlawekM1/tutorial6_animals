namespace WebApplication2.Data;


using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

{
    public class ClinicContext : DbContext
    {
        public ClinicContext(DbContextOptions<ClinicContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }
    }
}
