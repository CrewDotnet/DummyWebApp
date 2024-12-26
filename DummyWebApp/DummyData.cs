using PostgreSQL.Data;
using PostgreSQL.DataModels;

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
                    new Company { Id = 1, Name = "Activision Blizzard" },
                    new Company { Id = 2, Name = "KRAFTON, Inc." },
                    new Company { Id = 3, Name = "Gaijin Entertainment" },
                    new Company { Id = 4, Name = "InnoGames" },
                    new Company { Id = 5, Name = "Amazon Games" },
                    new Company { Id = 6, Name = "miHoYo" },

                };
                
                context.Companies.AddRange(companies);
                context.SaveChanges();

                var activisionBlizzard = context.Companies.First(c => c.Id == 1);
                var kraftonInc = context.Companies.First(c => c.Id == 2);
                var gaijin = context.Companies.First(c => c.Id == 3);
                var innoGames = context.Companies.First(c => c.Id == 4);
                var amazonGames = context.Companies.First(c => c.Id == 5);
                var miHoyo = context.Companies.First(c => c.Id == 6);

                var games = new List<Game>
                {

                };

                context.Games.AddRange(games);
                context.SaveChanges();
            }
        }
    }
}