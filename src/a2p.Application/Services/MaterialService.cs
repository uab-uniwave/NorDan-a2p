using a2p.Application.Models;
using a2p.Application.Services;
using a2p.Domain.Entities;
using a2p.Domain.Exception;
using a2p.Domain.Interfaces;

using Microsoft.Extensions.Logging;

public class MaterialService : IMaterialService
{
    private readonly IMaterialRepository _repo;
    private readonly ILogger<MaterialService> _logger;

    public MaterialService(IMaterialRepository repo, ILogger<MaterialService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Result<MaterialEntity>> InsertMaterialAsync(MaterialEntity material)
    {
        try
        {

            material.CreatedUTCDateTime = DateTime.UtcNow;
            var result = await _repo.InsertMaterialAsync(material);
            if (result == null || result.Id == Guid.Empty)
            {
                return Result.Failure<MaterialEntity>($"Failed inserting material. OrderNumber '{material.OrderNumber}', material '{material.Reference}'!");
            }
            return Result.Success<MaterialEntity>(result);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<MaterialEntity>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inserting material '{material.Reference}' of order '{material.OrderNumber}'!");
            return Result.Failure<MaterialEntity>($"Error inserting material '{material.Reference}' of order '{material.OrderNumber}': {ex.Message}");
        }

    }

    public async Task<Result<MaterialEntity?>> GetMaterialAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.GetMaterialAsync(rowId);
            if (result == null || result.Id == Guid.Empty)
            {
                return Result.Failure<MaterialEntity?>($"Failed to get material. Material with rowId '{rowId}' not found.");
            }
            return Result.Success<MaterialEntity?>(result);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<MaterialEntity?>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting material by rowId '{rowId}'!");
            return Result.Failure<MaterialEntity?>($"Error getting material by rowId '{rowId}'!");
        }

    }
    public async Task<Result<IEnumerable<MaterialEntity>?>> GetOrderMaterialsAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.GetOrderMaterialsAsync(rowId);
            if (result == null || !result.Any())
            {
                return Result.Failure<IEnumerable<MaterialEntity>?>($"Failed to get materials. Materials for order rowId '{rowId}' not found.");
            }
            return Result.Success<IEnumerable<MaterialEntity>?>(result);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<IEnumerable<MaterialEntity>?>(dex.Message);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting materials by order rowId '{rowId}'!");
            return Result.Failure<IEnumerable<MaterialEntity>?>($"Error getting materials by rowId '{rowId}'!");
        }

    }


    public async Task<Result<IEnumerable<MaterialEntity>?>> GetMaterialsAsync()
    {
        try
        {

            var result = await _repo.GetMaterialsAsync();
            if (result == null || !result.Any())
            {
                return Result.Failure<IEnumerable<MaterialEntity>?>($"Failed to get materials. Materials not found.");
            }
            return Result.Success<IEnumerable<MaterialEntity>?>(result);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<IEnumerable<MaterialEntity>?>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting materials!");
            return Result.Failure<IEnumerable<MaterialEntity>?>($"Error getting materials!");
        }

    }

    public async Task<Result<MaterialEntity?>> UpdateMaterialAsync(MaterialEntity material)
    {
        try
        {

            material.ModifiedUTCDateTime = DateTime.UtcNow;
            var result = await _repo.UpdateMaterialAsync(material);
            if (result == null || result.Id == Guid.Empty)
            {
                return Result.Failure<MaterialEntity?>($"Failed to update material. OrderNumber '{material.OrderNumber}', material '{material.Reference}'!");
            }
            return Result.Success<MaterialEntity?>(result);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<MaterialEntity?>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating material '{material.Reference}' of order '{material.OrderNumber}'!");
            return Result.Failure<MaterialEntity?>($"Error updating material '{material.Reference}' for order '{material.OrderNumber}': {ex.Message}");
        }

    }

    public async Task<Result<Guid>> DeleteMaterialAsync(Guid rowId)
    {
        try
        {
            var result = await _repo.DeleteMaterialAsync(rowId);
            if (result == Guid.Empty)
            {
                return Result.Failure<Guid>($"Failed to delete Material with Id '{rowId}'. Material not found!");
            }

            return Result.Success<Guid>(result);
        }
        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<Guid>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting Material with rowId '{rowId}'!");
            return Result.Failure<Guid>($"Error deleting Material with rowId '{rowId}'!");
        }
    }
}
