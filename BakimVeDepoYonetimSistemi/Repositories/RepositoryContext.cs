using BakimVeDepoYonetimSistemi.Model;
using Microsoft.EntityFrameworkCore;

namespace BakimVeDepoYonetimSistemi.Repositories
{
    public class RepositoryContext : DbContext
    {

        public DbSet<User> KullanicilarTable { get; set; }
        public DbSet<TeamMember> EkipUye { get; set; }
        public DbSet<Team> EkipTable { get; set; }

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
     .HasKey(u => u.KullaniciId);
        }

    }
}
