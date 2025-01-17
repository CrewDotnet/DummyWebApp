using DummyWebApp.Models.ResponseModels.Game;
using FluentResults;
using MediatR;

namespace DummyWebApp.MediatR.Commands
{
    public record UpdateGameCommand(int Id, string Genre, string ShortDescription, string Platform) : IRequest<Result<GameDTO>>;
}
