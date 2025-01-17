using DummyWebApp.MediatR.Commands;
using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Services.Interfaces;
using FluentResults;
using MediatR;

namespace DummyWebApp.MediatR.Handlers
{
    public class UpdateGameHandler : IRequestHandler<UpdateGameCommand, Result<GameDTO>>
    {
        private readonly IGameService _gameService;

        public UpdateGameHandler(IGameService gameService)
        {
            _gameService = gameService;
        }
        public async Task<Result<GameDTO>> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            var result = await _gameService.UpdateGame(request.Id, new UpdateGameRequest
            {
                Genre = request.Genre,
                ShortDescription = request.ShortDescription,
                Platform = request.Platform
            });
            if (result.IsFailed)
            {
                return Result.Fail<GameDTO>(result.Errors);
            }
            return Result.Ok(result.Value);
        }
    }
}
