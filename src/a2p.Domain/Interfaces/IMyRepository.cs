using a2p.Domain.Entities;

namespace a2p.Domain.Interfaces
{
    public interface IMyRepository
    {


        Task<OrderEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderEntity>?> GetAllAsync();
        Task<OrderEntity> InsertAsync(OrderEntity order);
        Task<OrderEntity?> UpdateAsync(OrderEntity order);
        Task<Guid> DeleteAsync(Guid id);
    }

}


