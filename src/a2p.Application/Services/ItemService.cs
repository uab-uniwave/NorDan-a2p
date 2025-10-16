using a2p.Application.Interfaces;
using a2p.Application.Services;
using a2p.Domain.Entities;
using a2p.Domain.Interfaces;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

public class ItemService : IItemService
{
    private readonly IItemRepository _repo;
    private readonly ILogger<ItemService> _logger;

    public ItemService(IItemRepository repo, ILogger<ItemService> logger, ISettingsService settingsService)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<ItemEntity?> CreateItemAsync(ItemEntity item)
    {
        try

        {


            var inserted = await _repo.CreateItemAsync(item);
            if (inserted == null || inserted.Id == Guid.Empty)
            {
                _logger.LogWarning("Failed to create item '{ItemName}' for order '{OrderNumber}'", item.ItemName, item.OrderNumber);
                return null;
            }

            return inserted;


        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error creating item '{ItemName}' for order '{OrderNumber}'", item.ItemName, item.OrderNumber);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating item '{ItemName}' for order '{OrderNumber}'", item.ItemName, item.OrderNumber);
            return null;

        }
    }
}
