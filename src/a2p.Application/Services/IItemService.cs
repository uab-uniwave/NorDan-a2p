using a2p.Application.Models;
using a2p.Domain.Entities;

namespace a2p.Application.Services
{

    public interface IItemService
    {
        Task<Result<ItemEntity>> InsertItemAsync(ItemEntity item);
        Task<Result<ItemEntity?>> GetItemAsync(Guid rowId);
        Task<Result<IEnumerable<ItemEntity>?>> GetOrderItemsAsync(Guid rowId);
        Task<Result<IEnumerable<ItemEntity>?>> GetItemsAsync();
        Task<Result<ItemEntity?>> UpdateItemAsync(ItemEntity item);
        Task<Result<Guid>> DeleteItemAsync(Guid rowId);
    }
}
