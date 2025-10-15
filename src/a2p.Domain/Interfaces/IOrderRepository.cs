using a2p.Domain.Entities;

namespace a2p.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderEntity?> GetOrderAsync(Guid id);
        Task<OrderEntity?> GetOrderByNumberAsync(string orderNumber);
        Task<IEnumerable<OrderEntity>?> GetOrdersAsync();
        Task<OrderEntity> InsertOrderAsync(OrderEntity order);
        Task<OrderEntity?> UpdateOrderAsync(OrderEntity order);
        Task<Guid> DeleteOrderAsync(Guid id);
    }
}

