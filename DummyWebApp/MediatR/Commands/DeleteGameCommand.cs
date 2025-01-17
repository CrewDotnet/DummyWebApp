using FluentResults;
using MediatR;

namespace DummyWebApp.MediatR.Commands
{
    public record DeleteGameCommand(int Id) : IRequest<Result<bool>>;
}
