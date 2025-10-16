using a2p.Domain.Entities;

namespace a2p.Domain.Interfaces
{

    public interface IItemRepository
    {
        //{
        //    Task<ItemEntity?> GetItemAsync(Guid id);
        //    Task<IEnumerable<ItemEntity>?> GetOrderItemsAsync(Guid orderId);

        //    Task<IEnumerable<ItemEntity>?> GetItemsAsync();
        Task<ItemEntity?> CreateItemAsync(ItemEntity item);
        //Task<ItemEntity?> UpdateItemAsync(ItemEntity item);
        //Task<Guid> DeleteItemAsync(Guid id);
    }

}

