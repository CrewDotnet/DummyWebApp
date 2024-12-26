using FluentResults;
using Newtonsoft.Json;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories;
using PostgreSQL.Repositories.Interfaces;

namespace GamesClient
{
    public class GameApiClient : IGameApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ICompanyRepository _companyRepository;
        private readonly IGameRepository _gameRepository;

        public GameApiClient(HttpClient httpClient, ICompanyRepository companyRepository, IGameRepository gameRepository)
        {
            _httpClient = httpClient;
            _companyRepository = companyRepository;
            _gameRepository = gameRepository;
        }

        public async Task<Result<bool>> AddClientGamesToExistingCompany()
        {
            var allClientGamesResult = await GetAllClientGames();
            if (!allClientGamesResult.IsSuccess)
            {
                return Result.Fail<bool>("Failed to fetch client games");
            }
            var allClientGames = allClientGamesResult.Value;

            var existingCompanies = await _companyRepository.GetAllAsync();

            // Find the games that have a publisher matching an existing company
            var gamesThatHavePublisherCompanyMatch = allClientGames
                .Where(g => existingCompanies.Value.Any(c => c.Name == g.Publisher))
                .ToList();

            foreach (var game in gamesThatHavePublisherCompanyMatch)
            {
                var matchingCompany = existingCompanies.Value.FirstOrDefault(c => c.Name == game.Publisher);

                if (matchingCompany != null)
                {
                    var newGame = new Game
                    {
                        Title = game.Title,
                        Price = 0,
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

            return Result.Ok(true);
        }

        public async Task<Result<GameClientModel>> GetClientGameByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/game?id={id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<GameClientModel>(content);

            Console.WriteLine($"Deserialized Title: {result.Title}");
            return result;
        }

        public async Task<Result<IEnumerable<GameClientModel>?>> GetAllClientGames()
        {
            var response = await _httpClient.GetAsync($"api/games");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<GameClientModel>>(content);
            return Result.Ok(result);
        }
        
    }
}