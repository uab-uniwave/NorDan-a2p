using Application.DTOs;

using Domain.Entities;
using Domain.Shared;

namespace Application.Interfaces.Services
{

    public interface IItemService
    {

        // CREATE
        Task<ValidationResult<ItemEntity>> CreateItemAsync(ItemDto dto);
        // UPDATE
        Task<ValidationResult<ItemEntity>> UpdateItemAsync(ItemDto dto);

        // GET BY ID
        Task<Result<ItemEntity>> GetItemAsync(Guid id);

        // GET ORDER ITEMS
        Task<PagedResult<IEnumerable<ItemEntity>?>> GetOrderItemsAsync(Guid id, int page, int size);

        // PAGED (repository doesn't expose paged; do simple in-memory paging)
        Task<PagedResult<ItemEntity>> GetItemsAsync(int page, int size);
        // DELETE
        Task<Result<bool>> DeleteItemAsync(Guid id);
    }
}