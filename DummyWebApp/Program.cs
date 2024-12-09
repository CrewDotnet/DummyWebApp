using DummyWebApp.Mappings;
using DummyWebApp.Services;
using DummyWebApp.Services.Interfaces;
using DummyWebApp.Validators;
using PostgreSQL.Data.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;

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

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            
            var connectionString = builder.Configuration.GetConnectionString("GameDatabase");
            builder.Services.AddServiceDataLayer(builder.Configuration, connectionString);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


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
