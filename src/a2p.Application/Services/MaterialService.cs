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
            if (result == null || result.RowId == Guid.Empty)
            {
                return Result<MaterialEntity>.Failure($"Failed inserting material. Order '{material.OrderNumber}', material '{material.Reference}'!");
            }
            return Result<MaterialEntity>.Success(result);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<MaterialEntity>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inserting material '{material.Reference}' of order '{material.OrderNumber}'!");
            return Result<MaterialEntity>.Failure($"Error inserting material '{material.Reference}' of order '{material.OrderNumber}' : {ex.Message.ToString()}");
        }

    }

    public async Task<Result<MaterialEntity?>> GetMaterialAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.GetMaterialAsync(rowId);
            if (result == null || result.RowId == Guid.Empty)
            {
                return Result<MaterialEntity?>.Failure($"Failed to get material. Material with rowId '{rowId}' not found.");
            }
            return Result<MaterialEntity>.Success(result)!;
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<MaterialEntity?>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting material by rowId '{rowId}'!", ex.Message);
            return Result<MaterialEntity?>.Failure($"Error getting  by rowId '{rowId}!");
        }

    }
    public async Task<Result<IEnumerable<MaterialEntity>?>> GetOrderMaterialsAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.GetOrderMaterialsAsync(rowId);
            if (result == null || !result.Any())
            {
                return Result<IEnumerable<MaterialEntity>?>.Failure($"Failed to get materials. Materials for order rowId '{rowId}' not found.");
            }
            return Result<IEnumerable<MaterialEntity>>.Success(result)!;
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<IEnumerable<MaterialEntity>?>.Failure(dex.Message);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting materials by order rowId '{rowId}'!", ex.Message);
            return Result<IEnumerable<MaterialEntity>?>.Failure($"Error getting  by rowId '{rowId}!");
        }

    }


    public async Task<Result<IEnumerable<MaterialEntity>?>> GetMaterialsAsync()
    {
        try
        {

            var result = await _repo.GetMaterialsAsync();
            if (result == null || !result.Any())
            {
                return Result<IEnumerable<MaterialEntity>?>.Failure($"Failed to get materials.Materials  not found.");
            }
            return Result<IEnumerable<MaterialEntity>>.Success(result)!;
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<IEnumerable<MaterialEntity>?>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting materials");
            return Result<IEnumerable<MaterialEntity>?>.Failure($"Error getting materials !");
        }

    }

    public async Task<Result<MaterialEntity?>> UpdateMaterialAsync(MaterialEntity material)
    {
        try
        {

            material.ModifiedUTCDateTime = DateTime.UtcNow;
            var result = await _repo.UpdateMaterialAsync(material);
            if (result == null || result.RowId == Guid.Empty)
            {
                return Result<MaterialEntity?>.Failure($"Failed to updating . Order '{material.OrderNumber}',  '{material.Reference}'!");
            }
            return Result<MaterialEntity>.Success(result)!;
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<MaterialEntity?>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inserting order . Order '{material.OrderNumber}' ,  '{material.Reference}'!");
            return Result<MaterialEntity?>.Failure($"Error inserting  '{material.Reference}' for order '{material.OrderNumber}': {ex.Message}");
        }

    }

    public async Task<Result<Guid?>> DeleteMaterialAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.DeleteMaterialAsync(rowId);
            if (string.IsNullOrEmpty(result.ToString()))
            {

                return Result<Guid?>.Failure($"Failed to delete Material by with RowId '{rowId}'. Material not found!");
            }
            if (result == Guid.Empty)
            {

                return Result<Guid?>.Failure($"Failed to delete Material by with RowId '{rowId}'. Material not found!");
            }

            return Result<Guid?>.Success(result)!;
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<Guid?>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error delteing . Material rowId '{rowId}'!", ex.Message);
            return Result<Guid?>.Failure($"Error delteing . Material rowId '{rowId}!");
        }

    }
}
