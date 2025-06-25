using Microsoft.EntityFrameworkCore;
using SotoGeneratorAPI.Data.Entities;

namespace SotoGeneratorAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<SotoEntity> Sotos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SotoEntity>()
                        .HasKey(s => s.Reference);
        }
    }
}
