using Domain.Entities;

namespace Application.Interfaces.Repositories
{

    public interface IItemRepository
    {
        Task<ItemEntity?> CreateItemAsync(ItemEntity item);

        // READ BY ID
        Task<ItemEntity?> GetItemAsync(Guid id);

        // PAGED READ BY ORDER NUMBER
        Task<(IEnumerable<ItemEntity> Ir, int TotalCount)> GetOrderItems(Guid id, int page, int size);

        // PAGED READ
        Task<(IEnumerable<ItemEntity>? Items, int TotalCount)> GetItemsAsync(int page, int size);

        // UPDATE ALL ORDER DETAILS
        Task<int> UpdateItemAsync(ItemEntity item);

        // DELETE BY ID
        Task<int> DeleteItemAsync(Guid id);

        // DELETE BY ORDER ID
        Task<int> DeleteItemByOrderIdAsync(Guid id);
    }

}

