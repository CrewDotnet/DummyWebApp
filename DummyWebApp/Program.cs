using DummyWebApp.Mappings;
using DummyWebApp.Services;
using DummyWebApp.Services.Interfaces;
using PostgreSQL.Data.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using AutoMapper;
using GamesClient;

namespace DummyWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<MappingProfile>();
            //});
            //config.AssertConfigurationIsValid();

            builder.Services.AddControllers();

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            
            var connectionString = builder.Configuration.GetConnectionString("GameDatabase");
            builder.Services.AddServiceDataLayer(builder.Configuration, connectionString!);

            builder.Services.AddHttpClient<IGameApiClient, GameApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://free-to-play-games-database.p.rapidapi.com/");
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "50b6755e82mshfd7f4d5a3a5066bp15f732jsn36b5d07836b2");
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "free-to-play-games-database.p.rapidapi.com");
            });

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
