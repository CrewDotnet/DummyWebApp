using DummyWebApp.Models.ResponseModels.Game;
using FluentResults;
using MediatR;

namespace DummyWebApp.MediatR.Queries
{
    public record GetGameByIdQuery(int Id) : IRequest<Result<GameDTO>>;
}
