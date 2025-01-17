using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Models.ResponseModels.Game;
using FluentResults;
using MediatR;

namespace DummyWebApp.MediatR.Commands
{
    public record AddGameCommand(string Title, decimal Price, int CompanyId) : IRequest<Result<GameBaseResponse>>;
}
