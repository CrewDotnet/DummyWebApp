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
                .HasOne(g => g.CompanyService)
                .WithMany(c => c.Games)
                .HasForeignKey(g => g.CompanyId);
            
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Games)
                .WithMany()
                .UsingEntity(j => j.ToTable("CustomerGames"));

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Games)
                .WithMany(g => g.Orders)
                .UsingEntity(j => j.ToTable("OrderGames"));

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<CompanyService> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }

}
