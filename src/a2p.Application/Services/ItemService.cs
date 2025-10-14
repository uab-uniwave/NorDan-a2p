using a2p.Application.Models;
using a2p.Application.Services;
using a2p.Domain.Entities;
using a2p.Domain.Exception;
using a2p.Domain.Interfaces;

using Microsoft.Extensions.Logging;

public class ItemService : IItemService
{
    private readonly IItemRepository _repo;
    private readonly ILogger<ItemService> _logger;

    public ItemService(IItemRepository repo, ILogger<ItemService> logger)
    {
        _repo = repo;
        _logger = logger;

    }

    public async Task<Result<ItemEntity>> InsertItemAsync(ItemEntity item)
    {
        try
        {

            item.CreatedUTCDateTime = DateTime.UtcNow;
            var responseItem = await _repo.InsertItemAsync(item);
            if (responseItem == null || responseItem.RowId == Guid.Empty)
            {
                return Result<ItemEntity>.Failure($"Failed inserting item. Order '{item.OrderNumber}' , item '{item.ItemName}'!");
            }
            return Result<ItemEntity>.Success(responseItem);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<ItemEntity>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inserting item '{item.ItemName}' of order '{item.OrderNumber}'!");
            return Result<ItemEntity>.Failure($"Error inserting item '{item.ItemName}' of order '{item.OrderNumber}': {ex.Message}");
        }

    }

    public async Task<Result<ItemEntity?>> GetItemAsync(Guid rowId)
    {
        try
        {

            var response = await _repo.GetItemAsync(rowId);
            if (response == null || response.RowId == Guid.Empty)
            {
                return Result<ItemEntity?>.Failure($"Failed to get item. Item with rowId '{rowId}' not found.");
            }
            return Result<ItemEntity>.Success(response)!;
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<ItemEntity?>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting item by rowId '{rowId}'!", ex.Message);
            return Result<ItemEntity?>.Failure($"Error getting item by rowId '{rowId}!");
        }

    }
    public async Task<Result<IEnumerable<ItemEntity>?>> GetOrderItemsAsync(Guid rowId)
    {
        try
        {

            var response = await _repo.GetOrderItemsAsync(rowId);
            if (response == null || !response.Any())
            {
                return Result<IEnumerable<ItemEntity>?>.Failure($"Failed to get items. Items for order rowId '{rowId}' not found.");
            }
            return Result<IEnumerable<ItemEntity>>.Success(response)!;
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<IEnumerable<ItemEntity>?>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting items by order rowId '{rowId}'!", ex.Message);
            return Result<IEnumerable<ItemEntity>?>.Failure($"Error getting item by rowId '{rowId}!");
        }

    }


    public async Task<Result<IEnumerable<ItemEntity>?>> GetItemsAsync()
    {
        try
        {

            var response = await _repo.GetItemsAsync();
            if (response == null || !response.Any())
            {
                return Result<IEnumerable<ItemEntity>?>.Failure($"Failed to get items.Items  not found.");
            }
            return Result<IEnumerable<ItemEntity>?>.Success(response)!;
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<IEnumerable<ItemEntity>?>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error geting Items{}!", ex.Message);
            return Result<IEnumerable<ItemEntity>?>.Failure($"Error getting items !");
        }

    }

    public async Task<Result<ItemEntity?>> UpdateItemAsync(ItemEntity item)
    {
        try
        {

            item.ModifiedUTCDateTime = DateTime.UtcNow;
            var response = await _repo.UpdateItemAsync(item);
            if (response == null || response.RowId == Guid.Empty)
            {
                return Result<ItemEntity?>.Failure($"Failed to updating item. Order '{item.OrderNumber}', item '{item.ItemName}'!");
            }
            return Result<ItemEntity>.Success(response)!;
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<ItemEntity?>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inserting order item. Order '{item.OrderNumber}' , item '{item.ItemName}'!");
            return Result<ItemEntity?>.Failure($"Error inserting item '{item.ItemName}' for order '{item.OrderNumber}': {ex.Message}");
        }

    }

    public async Task<Result<Guid>> DeleteItemAsync(Guid rowId)
    {
        try
        {

            var response = await _repo.DeleteItemAsync(rowId);
            if (string.IsNullOrEmpty(response.ToString()))
            {

                return Result<Guid>.Failure($"Failed to delete Item by with RowId '{rowId}'. Item not found!");
            }
            if (response == Guid.Empty)
            {

                return Result<Guid>.Failure($"Failed to delete Item by with RowId '{rowId}'. Item not found!");
            }

            return Result<Guid>.Success(response)!;
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<Guid>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error delteing item. Item rowId '{rowId}'!", ex.Message);
            return Result<Guid>.Failure($"Error delteing item. Item rowId '{rowId}!");
        }

    }
}
