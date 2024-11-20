using DummyWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DummyWebApp.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        public DbSet<Game> Games { get; set; }
    }

}
