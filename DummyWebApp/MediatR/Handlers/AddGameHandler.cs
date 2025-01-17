using AutoMapper;
using DummyWebApp.MediatR.Commands;
using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Services.Interfaces;
using FluentResults;
using MediatR;

namespace DummyWebApp.MediatR.Handlers
{
    public class AddGameHandler : IRequestHandler<AddGameCommand, Result<GameBaseResponse>>
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public AddGameHandler(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;
        }
        public async Task<Result<GameBaseResponse>> Handle(AddGameCommand request, CancellationToken cancellationToken)
        {
            var result = await _gameService.AddGame(new NewGameRequest
            {
                Title = request.Title,
                CompanyId = request.CompanyId,
                Price = request.Price
            });

            if (result.IsFailed)
            {
                return Result.Fail<GameBaseResponse>(result.Errors);
            }

            var mappedResponse = _mapper.Map<GameBaseResponse>(result.Value);
            return Result.Ok(mappedResponse);
        }
    }
}
