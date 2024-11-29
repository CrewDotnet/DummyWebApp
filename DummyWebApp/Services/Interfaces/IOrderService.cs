using DummyWebApp.ResponseModels.Order;

namespace DummyWebApp.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        public Task<OrderResponse?> GetByIdAsync(int id);
        public Task<OrderResponse> PlaceOrderAsync(int customerId, IEnumerable<int> gameIds);
        public Task<IEnumerable<OrderResponse>> GetByCustomerIdAsync(int customerId);
    }
}
