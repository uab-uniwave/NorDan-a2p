using a2p.Domain.Entities;
using a2p.Domain.Models;

namespace a2p.Application.Services
{
    public interface IOrderQueueService
    {
        Task<Result<OrderQueueEntity>> InsertOrderQueueAsync(OrderQueueEntity order);
        Task<Result<OrderQueueEntity>> GetOrderAsync(Guid id);
        Task<Result<IEnumerable<OrderQueueEntity>>> GetOrdersAsync();
        Task<Result<OrderQueueEntity>> UpdateOrderAsync(OrderQueueEntity order);
        Task<Result<Guid>> DeleteOrderAsync(Guid id);
    }
}
