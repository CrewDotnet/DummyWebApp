using DummyWebApp.Services.Interfaces;
using FluentResults;
using GamesClient;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace DummyWebApp.Services
{
    public class GameClientService : IGameClientService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IGameApiClient _gameApiClient;

        public GameClientService(ICompanyRepository companyRepository, IGameRepository gameRepository,
            IGameApiClient gameApiClient)
        {
            _companyRepository = companyRepository;
            _gameRepository = gameRepository;
            _gameApiClient = gameApiClient;
        }

        public async Task<Result<bool>> AddOrUpdateClientGamesToExistingCompany()
        {
            var allClientGamesResult = await _gameApiClient.GetAllClientGames();
            if (!allClientGamesResult.IsSuccess)
            {
                return Result.Fail<bool>("Failed to fetch client games");
            }

            var allClientGames = allClientGamesResult.Value;

            var existingCompanies = await _companyRepository.GetAllAsync();
            if (!existingCompanies.IsSuccess)
            {
                return Result.Fail<bool>("Failed to fetch existing companies");
            }

            var existingGamesResult = await _gameRepository.GetAllAsync();
            var existingGames = existingGamesResult.IsSuccess ? existingGamesResult.Value : new List<Game>();

            var gamesThatHavePublisherCompanyMatch = allClientGames
                .Where(g => existingCompanies.Value.Any(c => c.Name == g.Publisher))
                .ToList();

            foreach (var game in gamesThatHavePublisherCompanyMatch)
            {
                var matchingCompany = existingCompanies.Value.FirstOrDefault(c => c.Name == game.Publisher);

                if (matchingCompany != null)
                {
                    var existingGame = existingGames
                        .FirstOrDefault(eg => eg.Title == game.Title && eg.CompanyId == matchingCompany.Id);

                    if (existingGame != null)
                    {
                        // Update existing game
                        existingGame.ShortDescription = game.ShortDescription;
                        existingGame.Genre = game.Genre;
                        existingGame.Platform = game.Platform;
                        existingGame.ReleaseDate = game.ReleaseDate;

                        var updateGameResult = await _gameRepository.UpdateAsync(existingGame);
                        if (!updateGameResult.IsSuccess)
                        {
                            return Result.Fail<bool>($"Failed to update game: {game.Title}");
                        }
                    }
                    else
                    {
                        // Add new game
                        var newGame = new Game
                        {
                            Title = game.Title,
                            Price = 1,
                            ShortDescription = game.ShortDescription,
                            Genre = game.Genre,
                            Platform = game.Platform,
                            ReleaseDate = game.ReleaseDate,
                            CompanyId = matchingCompany.Id
                        };

                        var addGameResult = await _gameRepository.AddAsync(newGame);
                        if (!addGameResult.IsSuccess)
                        {
                            return Result.Fail<bool>($"Failed to add game: {game.Title}");
                        }
                    }
                }
            }

            return Result.Ok(true);
        }
    }
}
