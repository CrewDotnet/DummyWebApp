using PostgreSQL.DataModels;
using PostgreSQL.Data;

namespace DummyWebApp;

public static class DummyData
{
    public static void InitializeDummyData(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

            if (!context.Games.Any() && !context.Companies.Any())
            {
                var companies = new List<Company>
                {
                    new Company { Id = 1, Name = "Nintendo" },
                    new Company { Id = 2, Name = "3DO" },
                    new Company { Id = 3, Name = "Sega" },
                    new Company { Id = 4, Name = "Activision" },
                    new Company { Id = 5, Name = "Owlcat Games" },
                    new Company { Id = 6, Name = "KONAMI" },
                    new Company { Id = 7, Name = "FromSoftware" }

                };
                
                context.Companies.AddRange(companies);
                context.SaveChanges();

                var nintendo = context.Companies.First(c => c.Id == 1);
                var threeDO = context.Companies.First(c => c.Id == 2);
                var sega = context.Companies.First(c => c.Id == 3);
                var activision = context.Companies.First(c => c.Id == 4);
                var owlcatGames = context.Companies.First(c => c.Id == 5);
                var konami = context.Companies.First(c => c.Id == 6);
                var fromSoftware = context.Companies.First(c => c.Id == 7);

                var games = new List<Game>
                {
                    new Game { Id = 1, Name = "Elden Ring", Price = 50m, Company = fromSoftware },
                    new Game { Id = 2, Name = "eFootball", Price = 60m, Company = konami },
                    new Game { Id = 3, Name = "Pathfinder", Price = 40m, Company = owlcatGames },
                    new Game { Id = 4, Name = "Call Of duty", Price = 54m, Company = activision },
                    new Game { Id = 5, Name = "Super Mario", Price = 35m, Company = nintendo },
                    new Game { Id = 6, Name = "Sonic", Price = 67m, Company = sega },
                    new Game { Id = 7, Name = "Heroes III", Price = 15m, Company = threeDO }
                };

                context.Games.AddRange(games);
                context.SaveChanges();
            }
        }
    }
}