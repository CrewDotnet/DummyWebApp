using DummyWebApp.Models.ResponseModels.Game;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostgreSQL.DataModels;

namespace DummyWebApp.MediatR.Queries
{
    public record GetGameListQuery() : IRequest<Result<List<GameDTO>>>;
}
