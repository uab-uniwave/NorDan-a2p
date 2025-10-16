using a2p.Domain.Entities;

namespace a2p.Domain.Interfaces
{
    public interface IOrderQueueRepository
    {
        Task<OrderQueueEntity?> GetOrderAsync(Guid id);
        Task<OrderQueueEntity?> GetOrderByNumberAsync(string orderNumber);
        Task<IEnumerable<OrderQueueEntity>?> GetOrdersAsync();
        Task<OrderQueueEntity> InsertOrderAsync(OrderQueueEntity order);
        Task<OrderQueueEntity?> UpdateOrderAsync(OrderQueueEntity order);
        Task<Guid> DeleteOrderAsync(Guid id);
    }
}

