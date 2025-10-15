using a2p.Domain.Entities;
using a2p.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace a2p.Domain.Interfaces
{
    public interface IOrderQueueRepository
    {
        Task<Result<OrderQueueEntity>> GetOrderAsync(Guid id);
        Task<Result<OrderQueueEntity>> GetOrderByNumberAsync(string orderNumber);
        Task<Result<IEnumerable<OrderQueueEntity>>> GetOrdersAsync();
        Task<Result<OrderQueueEntity>> InsertOrderAsync(OrderQueueEntity order);
        Task<Result<OrderQueueEntity>> UpdateOrderAsync(OrderQueueEntity order);
        Task<Result<Guid>> DeleteOrderAsync(Guid id);
    }
}

