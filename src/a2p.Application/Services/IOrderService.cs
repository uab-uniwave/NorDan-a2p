using a2p.Application.Models;
using a2p.Domain.Entities;

namespace a2p.Application.Services
{
    public interface IOrderService
    {
        Task<Result<OrderEntity>> InsertOrderAsync(OrderEntity order);
        Task<Result<OrderEntity?>> GetOrderAsync(Guid id);
        Task<Result<IEnumerable<OrderEntity>>?> GetOrdersAsync();
        Task<Result<OrderEntity?>> UpdateOrderAsync(OrderEntity order);
        Task<Result<Guid>> DeleteOrderAsync(Guid id);
    }
}
