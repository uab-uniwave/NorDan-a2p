using a2p.Application.Models;
using a2p.Domain.Entities;

namespace a2p.Application.Services
{
    public interface IOrderQueueService
    {
        Task<OrderQueueEntity?> InsertOrderQueueAsync(OrderQueueEntity order);
        Task<OrderQueueEntity?> GetOrderAsync(Guid id);
        Task<IEnumerable<OrderQueueEntity>?> GetOrdersAsync();
        Task<OrderQueueEntity?> UpdateOrderAsync(OrderQueueEntity order);
        Task<Guid> DeleteOrderAsync(Guid id);
    }
}   
