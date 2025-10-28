using Domain.Entities;

namespace Application.Interfaces.Repositories
{

    public interface IItemRepository
    {
        Task<ItemEntity?> CreateItemAsync(ItemEntity item);

        // READ BY ID
        Task<ItemEntity?> GetItemAsync(Guid id);

        //READ BY ORDER ID
        Task<IEnumerable<ItemEntity>> GetOrderItemsAsync(Guid id);


        // UPDATE 
        Task<int> UpdateItemAsync(ItemEntity item);

        // DELETE BY ID
        Task<int> DeleteItemAsync(Guid id);

        // DELETE BY ORDER ID
        Task<int> DeleteOrderItemsAsync(Guid id);
    }

}

