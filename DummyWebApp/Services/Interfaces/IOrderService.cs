using DummyWebApp.Models.ResponseModels.Order;
using FluentResults;

namespace DummyWebApp.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<Result<List<OrderResponse>>> GetAllOrdersAsync();
        public Task<Result<OrderResponse>> GetByIdAsync(int id);
        public Task<Result<OrderResponse>> PlaceOrderAsync(int customerId, IEnumerable<int> gameIds);
        public Task<Result<List<OrderResponse>>> GetOrdersByCustomerIdAsync(int customerId);
    }
}
