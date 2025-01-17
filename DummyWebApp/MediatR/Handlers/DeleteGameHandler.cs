using DummyWebApp.MediatR.Commands;
using DummyWebApp.Services.Interfaces;
using FluentResults;
using MediatR;

namespace DummyWebApp.MediatR.Handlers
{
    public class DeleteGameHandler : IRequestHandler<DeleteGameCommand, Result<bool>>
    {
        private readonly IGameService _gameService;

        public DeleteGameHandler(IGameService gameService)
        {
            _gameService = gameService;
        }
        public Task<Result<bool>> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
        {
            var result = _gameService.DeleteGame(request.Id);
            return result;
        }
    }
}
