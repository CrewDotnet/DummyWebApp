using DummyWebApp;
using DummyWebApp.Mappings;
using DummyWebApp.Services.Interfaces.Game;
using Microsoft.EntityFrameworkCore;
using PostgreSQL.Data;
using PostgreSQL.Repositories;
using PostgreSQL.Repositories.Interfaces;
using DummyWebApp.Services;
using DummyWebApp.Services.Interfaces.Company;

namespace DummyWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddControllers();
            builder.Services.AddDbContext<ApiContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("GameDatabase");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    options.UseNpgsql(connectionString);
                }
                else
                {
                    options.UseInMemoryDatabase("Games");
                }
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();

            var app = builder.Build();
            DummyData.InitializeDummyData(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
