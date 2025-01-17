using DummyWebApp.MediatR.Queries;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Services.Interfaces;
using FluentResults;
using MediatR;

namespace DummyWebApp.MediatR.Handlers
{
    public class GetGameListHandler : IRequestHandler<GetGameListQuery, Result<List<GameDTO>>>
    {
        private readonly IGameService _gameService;

        public GetGameListHandler(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<Result<List<GameDTO>>> Handle(GetGameListQuery request, CancellationToken cancellationToken)
        {
            var gamesList = await _gameService.GetAllGames();
            if (gamesList.IsFailed)
            {
                return Result.Fail<List<GameDTO>>(gamesList.Errors);
            }
            return Result.Ok(gamesList.Value);
        }
    }
}
