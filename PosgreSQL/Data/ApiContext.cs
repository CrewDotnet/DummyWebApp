using Microsoft.EntityFrameworkCore;
using PostgreSQL.DataModels;

namespace PostgreSQL.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Company)
                .WithMany(c => c.Games)
                .HasForeignKey(g => g.CompanyId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Company> Companies { get; set; }
    }

}
