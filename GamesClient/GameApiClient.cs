using FluentResults;
using Newtonsoft.Json;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace GamesClient
{
    public class GameApiClient : IGameApiClient
    {
        private readonly HttpClient _httpClient;

        public GameApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

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