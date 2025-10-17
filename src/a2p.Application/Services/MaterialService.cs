using a2p.Application.DTOs;
using a2p.Application.Interfaces.Repositories;
using a2p.Application.Interfaces.Services;
using a2p.Application.Validations;
using a2p.Domain.Entities;
using a2p.Domain.Shared;

using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace a2p.Application.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _repository;
        private readonly IValidator<MaterialDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialService> _logger;

        public MaterialService(
            IMaterialRepository repository,
            IValidator<MaterialDto> validator,
            IMapper mapper,
            ILogger<MaterialService> logger)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        // CREATE
        public async Task<ValidationResult<MaterialEntity>> CreateMaterialAsync(MaterialDto dto)
        {
            // Step 1. Validate input
            ValidationResult validation = await _validator.ValidateAsync(dto);
            ValidationResult<MaterialEntity> validationResult = validation.ToValidationResult<MaterialEntity>();
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                MaterialEntity entity = _mapper.Map<MaterialEntity>(dto)
                    ?? throw new InvalidOperationException("Mapping resulted in null MaterialEntity.");

                MaterialEntity? created = await _repository.CreateMaterialAsync(entity);
                if (created == null)
                {
                    return ValidationResult<MaterialEntity>.Failure(
                        new[] { new ValidationError("Repository", "Failed to create material.") });
                }

                _logger.LogInformation("Material {Reference} created.", created.Reference);
                return ValidationResult<MaterialEntity>.Success(created, "Material created successfully.");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error creating material.");
                return ValidationResult<MaterialEntity>.Failure(
                    new[] { new ValidationError("Database", "Database error occurred.") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating material.");
                return ValidationResult<MaterialEntity>.Failure(
                    new[] { new ValidationError("System", ex.Message) });
            }
        }

        // UPDATE
        public async Task<ValidationResult<MaterialEntity>> UpdateMaterialAsync(MaterialDto dto)
        {
            // Step 1. Validate
            ValidationResult validation = await _validator.ValidateAsync(dto);
            ValidationResult<MaterialEntity> validationResult = validation.ToValidationResult<MaterialEntity>();
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                MaterialEntity material = _mapper.Map<MaterialEntity>(dto)
                    ?? throw new InvalidOperationException("Mapping resulted in null MaterialEntity.");

                // Repository provides GetMaterialAsync by id
                MaterialEntity? existing = await _repository.GetMaterialAsync(material.Id);
                if (existing == null)
                {
                    return ValidationResult<MaterialEntity>.Failure(
                        new[] { new ValidationError(nameof(dto.Id), "Material not found.") });
                }

                material.ModifiedUTCDateTime = DateTime.UtcNow;

                // MaterialRepository.UpdateMaterialAsync returns updated material (nullable)
                int updated = await _repository.UpdateMaterialAsync(material);
                if (updated == 0)
                {
                    return ValidationResult<MaterialEntity>.Failure(
                        new[] { new ValidationError("Repository", "Failed to update material.") });
                }
                _logger.LogInformation("Material {Reference} updated successfully.", material.Reference);
                return ValidationResult<MaterialEntity>.Success(material, "Material updated successfully.");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error updating material.");
                return ValidationResult<MaterialEntity>.Failure(
                    new[] { new ValidationError("Database", "Database error.") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating material.");
                return ValidationResult<MaterialEntity>.Failure(
                    new[] { new ValidationError("System", ex.Message) });
            }
        }

        // GET BY ID
        public async Task<Result<MaterialEntity>> GetMaterialByIdAsync(Guid id)
        {
            try
            {
                MaterialEntity? material = await _repository.GetMaterialAsync(id);
                return material == null
                    ? Result<MaterialEntity>.Failure($"Material {id} not found.")
                    : Result<MaterialEntity>.Success(material);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving material {Id}", id);
                return Result<MaterialEntity>.Failure("Error retrieving material.");
            }
        }

        // GET ORDER ITEMS
        public async Task<PagedResult<IEnumerable<MaterialEntity>?>> GetOrderMaterialsAsync(Guid id, int page, int size)
        {
            try
            {
                (IEnumerable<MaterialEntity> Materials, int TotalCount) materials = await _repository.GetOrderMaterialsAsync(id, page, size);
                return PagedResult<IEnumerable<MaterialEntity>?>.Failure($"Materials for order '{id}' not found.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving materials for order {id}", id);
                return PagedResult<IEnumerable<MaterialEntity>?>.Failure("Error retrieving order materials.");
            }
        }

        // PAGED (repository doesn't expose paged; do simple in-memory paging)
        public async Task<PagedResult<MaterialEntity>> GetMaterialsAsync(int page, int size)
        {
            try
            {
                (IEnumerable<MaterialEntity>? materials, int total) = await _repository.GetMaterialsAsync(page, size);

                return PagedResult<MaterialEntity>.Success(materials, total, page, size);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged materials.");
                return PagedResult<MaterialEntity>.Failure("Error retrieving paged materials.");
            }
        }

        // DELETE
        public async Task<Result<bool>> DeleteMaterialAsync(Guid id)
        {
            try
            {
                MaterialEntity? existing = await _repository.GetMaterialAsync(id);
                if (existing == null)
                {
                    return Result<bool>.Failure($"Order {id} not found");
                }
                int rows = await _repository.DeleteMaterialsdAsync(id);
                return rows == 0
                    ? Result<bool>.Failure("Failed to delete order.")
                    : Result<bool>.Success(true, "Order deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order {Id}", id);
                return Result<bool>.Failure("Error deleting order.");
            }
        }

    }
}