
using a2p.Domain.Entities;

namespace a2p.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {

        // CREATE
        Task<OrderEntity?> CreateOrderAsync(OrderEntity order);

        // READ BY ID
        Task<OrderEntity?> GetOrderAsync(Guid id);

        // PAGED READ
        Task<(IEnumerable<OrderEntity> Orders, int TotalCount)> GetOrdersAsync(int page, int size);

        // UPDATE ALL ORDER DETAILS
        Task<int> UpdateOrderAsync(OrderEntity order);

        // UPDATE ALL ORDER DETAILS
        Task<int> UpdateOrderDeliveryAddressAsync(Guid id, string deliveryAddress);

        // DELETE BY ID
        Task<int> DeleteOrderAsync(Guid id);

    }
}
