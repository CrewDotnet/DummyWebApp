using DummyWebApp.Models;

namespace DummyWebApp.Data;

public static class DummyData
{
    public static void InitializeDummyData(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

            // Check if the database is empty before seeding
            if (!context.Games.Any())
            {
                var games = new List<Game>
                {
                    new Game { Id = 1, Name = "Elden Ring", Price = 50, Platform = "PS5" },
                    new Game { Id = 2, Name = "eFootball", Price = 60, Platform = "PS5" },
                    new Game { Id = 3, Name = "Pathfinder", Price = 40, Platform = "PC" },
                    new Game { Id = 4, Name = "Call Of duty", Price = 54, Platform = "Xbox" },
                    new Game { Id = 5, Name = "Super Mario", Price = 35, Platform = "Nintendo" },
                    new Game { Id = 6, Name = "Sonic", Price = 67, Platform = "Sega" },
                    new Game { Id = 7, Name = "Heroes III", Price = 15, Platform = "PC" }
                };
                context.Games.AddRange(games);
                context.SaveChanges();
            }
        }
    }
}