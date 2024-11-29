using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostgreSQL.Repositories.Interfaces;
using PostgreSQL.Repositories;

namespace PostgreSQL.Data.Extensions
{
    public static class ServiceDataLayerExtension
    {
        public static IServiceCollection AddServiceDataLayer(this IServiceCollection services,
            IConfiguration configuration, string connectionString)
        {
            services.AddDbContext<ApiContext>(options =>
            {

                if (!string.IsNullOrEmpty(connectionString))
                {
                    options.UseNpgsql(connectionString);
                }
                else
                {
                    options.UseInMemoryDatabase("Games");
                }
            });
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IGameRepository, GameRepository>();

            return services;

        }
    }
}

        
    


