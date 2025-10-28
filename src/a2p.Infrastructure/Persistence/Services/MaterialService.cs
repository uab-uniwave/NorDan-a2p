using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Validations;

using AutoMapper;

using Domain.Entities;
using Domain.Shared;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Services
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

        // GET ORDER MATERIALS
        public async Task<Result<IEnumerable<MaterialEntity>>> GetOrderMaterialsAsync(Guid id)
        {
            try
            {
                IEnumerable<MaterialEntity>? materials = await _repository.GetOrderMaterialsAsync(id);
                return materials == null
                ? Result<IEnumerable<MaterialEntity>>.Failure($"No materials found for order {id}.")
                : Result<IEnumerable<MaterialEntity>>.Success(materials);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving materials for order {id}", id);
                return Result<IEnumerable<MaterialEntity>>.Failure("Error retrieving order materials.");
            }
        }

        // DELETE MATERIAL
        public async Task<Result<bool>> DeleteMaterialAsync(Guid id)
        {
            try
            {
                MaterialEntity? existing = await _repository.GetMaterialAsync(id);
                if (existing == null)
                {
                    return Result<bool>.Failure($"Material {id} not found.");
                }
                int rows = await _repository.DeleteMaterialsdAsync(id);
                return rows == 0
                ? Result<bool>.Failure("Failed to delete material.")
                : Result<bool>.Success(true, $"Material {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting material {Id}", id);
                return Result<bool>.Failure("Error deleting order.");
            }
        }

        // DELETE ORDER MATERIALS
        public async Task<Result<bool>> DeleteOrderMaterialAsync(Guid id)
        {
            try
            {
                IEnumerable<MaterialEntity>? existing = await _repository.GetOrderMaterialsAsync(id);
                if (existing == null)
                {
                    return Result<bool>.Failure($"Materials for order {id} not found.");
                }
                int rows = await _repository.DeleteOrderMaterialsAsync(id);
                return rows == 0
                ? Result<bool>.Failure($"Failed to delete order {id} materials.")
                : Result<bool>.Success(true, $"Materials of order {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order {Id} materials.", id);
                return Result<bool>.Failure("Error deleting order.");
            }
        }

    }
}