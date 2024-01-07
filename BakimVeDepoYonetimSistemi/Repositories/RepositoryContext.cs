using System.Data.SqlClient;
using BakimVeDepoYonetimSistemi.Model;
using Microsoft.EntityFrameworkCore;

namespace BakimVeDepoYonetimSistemi.Repositories
{
    public class RepositoryContext : DbContext
    {

        public DbSet<User> KullanicilarTable { get; set; }
        public DbSet<TeamMember> EkipUye { get; set; }
        public DbSet<Team> EkipTable { get; set; }
        public DbSet<Maintaince> BakimTalep { get; set; }
        public DbSet<MaintainceState> TalepDurumTable { get; set; }
        public DbSet<Asset>VarlikTable { get; set; }
        public DbSet<WorkForce> IsGucuTable { get; set; }

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
     .HasKey(u => u.KullaniciId);
      modelBuilder.Entity<Team>()
     .HasKey(u => u.EkipId);
        modelBuilder.Entity<TeamMember>()
     .HasKey(u => u.EkipUyeId);
     modelBuilder.Entity<Maintaince>()
     .HasKey(u => u.TalepId);
     modelBuilder.Entity<MaintainceState>()
     .HasKey(u => u.DurumId);
     modelBuilder.Entity<Asset>()
     .HasKey(u => u.VarlikId);
        modelBuilder.Entity<WorkForce>().HasKey(u => u.IsGucuId);
    

        }


          

    }
}
