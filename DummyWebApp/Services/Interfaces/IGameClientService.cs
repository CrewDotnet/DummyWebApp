using FluentResults;

namespace DummyWebApp.Services.Interfaces
{
    public interface IGameClientService
    {
        Task<Result<bool>> AddOrUpdateClientGamesToExistingCompany();
    }
}
