using a2p.Domain.Entities;

namespace a2p.Domain.Interfaces
{
    public interface IOrderQueueRepository
    {
        Task<a2p.Domain.Models.Result<OrderQueueEntity>> GetOrderAsync(Guid id);
        Task<a2p.Domain.Models.Result<OrderQueueEntity>> GetOrderByNumberAsync(string orderNumber);
        Task<a2p.Domain.Models.Result<IEnumerable<OrderQueueEntity>>> GetOrdersAsync();
        Task<a2p.Domain.Models.Result<OrderQueueEntity>> InsertOrderAsync(OrderQueueEntity order);
        Task<a2p.Domain.Models.Result<OrderQueueEntity>> UpdateOrderAsync(OrderQueueEntity order);
        Task<a2p.Domain.Models.Result<Guid>> DeleteOrderAsync(Guid id);
    }
}

