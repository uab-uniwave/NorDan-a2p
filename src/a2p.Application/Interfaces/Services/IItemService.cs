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
        Task<Result<IEnumerable<ItemEntity>?>> GetOrderItemsAsync(Guid id);

        // DELETE
        Task<Result<bool>> DeleteItemAsync(Guid id);

        // DELETE BY ORDER ID
        Task<Result<bool>> DeleteOrderItemsAsync(Guid id);
    }
}