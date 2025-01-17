using DummyWebApp.MediatR.Queries;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Services.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DummyWebApp.MediatR.Handlers
{
    public class GetGameByIdHandler : IRequestHandler<GetGameByIdQuery, Result<GameDTO>>
    {
        private readonly IGameService _gameService;

        public GetGameByIdHandler(IGameService gameService)
        {
            _gameService = gameService;
        }
        public async Task<Result<GameDTO>> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _gameService.GetGameById(request.Id);
            }
            catch (Exception ex)
            {
                return Result.Fail<GameDTO>(new Error(ex.Message));
            }
        }
    }
}
