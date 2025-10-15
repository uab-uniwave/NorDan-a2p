using a2p.Application.Interfaces;
using a2p.Application.Services;
using a2p.Domain.Entities;
using a2p.Domain.Interfaces;

using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

public class ItemService : IItemService
{
    private readonly IItemRepository _repo;
    private readonly ILogger<ItemService> _logger;
    private readonly ISettingsService _settingsService;
    private readonly string _connectionString;

    public ItemService(IItemRepository repo, ILogger<ItemService> logger, ISettingsService settingsService)
    {
        _repo = repo;
        _logger = logger;
        _settingsService = settingsService;

        var settings = _settingsService.LoadAllSettings();
        _connectionString = settings.ConnectionStrings["DefaultConnection"] ?? string.Empty;
    }



    public async Task<Result<int>> CreateItemAsync(ItemEntity item)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(item.ItemName))
                return Result.Failure<int>("Item name is required", "VALIDATION_ERROR");

            if (item.Price <= 0)
                return Result.Failure<int>("Item price must be greater than zero", "VALIDATION_ERROR");

            using var connection = new SqlConnection(_connectionString);

            var sql = @"INSERT INTO [dbo].[Uniwave_a2p_Items]
           ([Id]
           ,[OrderId]
           ,[OrderNumber]
           ,[ProjectNumber]
           ,[SalesDocumentNumber]
           ,[SalesDocumentVersion]
           ,[ItemName]
           ,[SortOrder]
           ,[Description]
           ,[Quantity]
           ,[Width]
           ,[Height]
           ,[Weight]
           ,[WeightWithoutGlass]
           ,[WeightGlass]
           ,[TotalWeight]
           ,[TotalWeightWithoutGlass]
           ,[TotalWeightGlass]
           ,[Area]
           ,[TotalArea]
           ,[Hours]
           ,[TotalHours]
           ,[MaterialCost]
           ,[LaborCost]
           ,[Cost]
           ,[TotalMaterialCost]
           ,[TotalLaborCost]
           ,[TotalCost]
           ,[Price]
           ,[TotalPrice]
           ,[Worksheet]
           ,[Line]
           ,[Column]
           ,[CreatedUTCDateTime]
           ,[ModifiedUTCDateTime]
           ,[CreatedBy]
           ,[ModifiedBy])
     VALUES
           (@Id, 
           ,@OrderId, 
           ,@OrderNumber, 
           ,@ProjectNumber, 
           ,@SalesDocumentNumber, 
           ,@SalesDocumentVersion, 
           ,@ItemName,
           ,@SortOrder, 
           ,@Description, 
           ,@Quantity,
           ,@Width, 
           ,@Height,
           ,@Weight, 
           ,@WeightWithoutGlass, 
           ,@WeightGlass, 
           ,@TotalWeight, 
           ,@TotalWeightWithoutGlass, 
           ,@TotalWeightGlass,
           ,@Area, 
           ,@TotalArea,
           ,@Hours, 
           ,@TotalHours, 
           ,@MaterialCost, 
           ,@LaborCost,
           ,@Cost, 
           ,@TotalMaterialCost,
           ,@TotalLaborCost,
           ,@TotalCost, 
           ,@Price, 
           ,@TotalPrice, 
           ,@Worksheet,
           ,@Line, 
           ,@Column, 
           ,@CreatedUTCDateTime, 
           ,@ModifiedUTCDateTime, 
           ,@CreatedBy,
           ,@ModifiedBy,
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await connection.QuerySingleAsync<int>(sql, item);

            return Result.Success(newId);
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601) // Unique constraint violation
        {
            return Result.Failure<int>("A product with this name already exists", "DUPLICATE_ERROR", ex);
        }
        catch (SqlException ex)
        {
            return Result.Failure<int>($"Database error: {ex.Message}", "DB_ERROR", ex);
        }
        catch (Exception ex)
        {
            return Result.Failure<int>($"Unexpected error: {ex.Message}", "UNKNOWN_ERROR", ex);
        }
    }

    //public async Task<Result<ItemEntity?>> GetItemAsync(Guid rowId)
    //{
    //    try
    //    {

    //        var response = await _repo.GetItemAsync(rowId);
    //        if (response == null || response.Id == Guid.Empty)
    //        {
    //            return Result<ItemEntity?>.Failure($"Failed to get item. ItemName with rowId '{rowId}' not found.");
    //        }
    //        return Result<ItemEntity>.Success(response)!;
    //    }

    //    catch (DomainException dex)
    //    {
    //        // domain rule violation - handled gracefully
    //        return Result<ItemEntity?>.Failure(dex.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, $"Error getting item by rowId '{rowId}'!", ex.Message);
    //        return Result<ItemEntity?>.Failure($"Error getting item by rowId '{rowId}!");
    //    }

    //}
    //public async Task<Result<IEnumerable<ItemEntity>?>> GetOrderItemsAsync(Guid rowId)
    //{
    //    try
    //    {

    //        var response = await _repo.GetOrderItemsAsync(rowId);
    //        if (response == null || !response.Any())
    //        {
    //            return Result<IEnumerable<ItemEntity>?>.Failure($"Failed to get items. ItemsDto for order rowId '{rowId}' not found.");
    //        }
    //        return Result<IEnumerable<ItemEntity>>.Success(response)!;
    //    }

    //    catch (DomainException dex)
    //    {
    //        // domain rule violation - handled gracefully
    //        return Result<IEnumerable<ItemEntity>?>.Failure(dex.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, $"Error getting items by order rowId '{rowId}'!", ex.Message);
    //        return Result<IEnumerable<ItemEntity>?>.Failure($"Error getting item by rowId '{rowId}!");
    //    }

    //}


    //public async Task<Result<IEnumerable<ItemEntity>?>> GetItemsAsync()
    //{
    //    try
    //    {

    //        var response = await _repo.GetItemsAsync();
    //        if (response == null || !response.Any())
    //        {
    //            return Result<IEnumerable<ItemEntity>?>.Failure($"Failed to get items.ItemsDto  not found.");
    //        }
    //        return Result<IEnumerable<ItemEntity>?>.Success(response)!;
    //    }

    //    catch (DomainException dex)
    //    {
    //        // domain rule violation - handled gracefully
    //        return Result<IEnumerable<ItemEntity>?>.Failure(dex.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error geting ItemsDto{}!", ex.Message);
    //        return Result<IEnumerable<ItemEntity>?>.Failure($"Error getting items !");
    //    }

    //}

    //public async Task<Result<ItemEntity?>> UpdateItemAsync(ItemEntity item)
    //{
    //    try
    //    {

    //        item.ModifiedUTCDateTime = DateTime.UtcNow;
    //        var response = await _repo.UpdateItemAsync(item);
    //        if (response == null || response.Id == Guid.Empty)
    //        {
    //            return Result<ItemEntity?>.Failure($"Failed to updating item. OrderNumber '{item.OrderNumber}', item '{item.ItemName}'!");
    //        }
    //        return Result<ItemEntity>.Success(response)!;
    //    }

    //    catch (DomainException dex)
    //    {
    //        // domain rule violation - handled gracefully
    //        return Result<ItemEntity?>.Failure(dex.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, $"Error inserting order item. OrderNumber '{item.OrderNumber}' , item '{item.ItemName}'!");
    //        return Result<ItemEntity?>.Failure($"Error inserting item '{item.ItemName}' for order '{item.OrderNumber}': {ex.Message}");
    //    }

    //}

    //    public async Task<Result<Guid>> DeleteItemAsync(Guid rowId)
    //    {
    //        try
    //        {

    //            var response = await _repo.DeleteItemAsync(rowId);
    //            if (string.IsNullOrEmpty(response.ToString()))
    //            {

    //                return Result<Guid>.Failure($"Failed to delete ItemName by with Id '{rowId}'. ItemName not found!");
    //            }
    //            if (response == Guid.Empty)
    //            {

    //                return Result<Guid>.Failure($"Failed to delete ItemName by with Id '{rowId}'. ItemName not found!");
    //            }

    //            return Result<Guid>.Success(response)!;
    //        }

    //        catch (DomainException dex)
    //        {
    //            // domain rule violation - handled gracefully
    //            return Result<Guid>.Failure(dex.Message);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, $"Error delteing item. ItemName rowId '{rowId}'!", ex.Message);
    //            return Result<Guid>.Failure($"Error delteing item. ItemName rowId '{rowId}!");
    //        }

    //    }
}
