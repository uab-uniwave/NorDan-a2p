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
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repository;
        private readonly IValidator<ItemDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemService> _logger;

        public ItemService(
         IItemRepository repository,
         IValidator<ItemDto> validator,
         IMapper mapper,
         ILogger<ItemService> logger)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        // CREATE
        public async Task<ValidationResult<ItemEntity>> CreateItemAsync(ItemDto dto)
        {

            // Step 1. Validate input
            ValidationResult validation = await _validator.ValidateAsync(dto);
            ValidationResult<ItemEntity> validationResult = validation.ToValidationResult<ItemEntity>();
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }
            try
            {

                ItemEntity entity = _mapper.Map<ItemEntity>(dto)
                 ?? throw new InvalidOperationException("Mapping resulted in null ItemEntity.");

                ItemEntity? created = await _repository.CreateItemAsync(entity);
                if (created == null)
                {
                    return ValidationResult<ItemEntity>.Failure(
                     new[] { new ValidationError("Repository", "Failed to create item.") });
                }

                _logger.LogInformation("Item {ItemName} created.", created.ItemName);
                return ValidationResult<ItemEntity>.Success(created, "Item created successfully.");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error creating item.");
                return ValidationResult<ItemEntity>.Failure(
                 new[] { new ValidationError("Database", "Database error occurred.") });
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating item.");
                return ValidationResult<ItemEntity>.Failure(
                 new[] { new ValidationError("System", ex.Message) });
            }
        }

        // UPDATE
        public async Task<ValidationResult<ItemEntity>> UpdateItemAsync(ItemDto dto)
        {
            // Step 1. Validate
            ValidationResult validation = await _validator.ValidateAsync(dto);
            ValidationResult<ItemEntity> validationResult = validation.ToValidationResult<ItemEntity>();
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                ItemEntity item = _mapper.Map<ItemEntity>(dto)
                 ?? throw new InvalidOperationException("Mapping resulted in null ItemEntity.");

                // Repository provides GetItemAsync by id
                ItemEntity? existing = await _repository.GetItemAsync(item.Id);
                if (existing == null)
                {
                    return ValidationResult<ItemEntity>.Failure(
                     new[] { new ValidationError(nameof(dto.Id), "Item not found.") });
                }

                item.ModifiedDateTime = DateTime.Now;

                // ItemRepository.UpdateItemAsync returns updated item (nullable)
                int updated = await _repository.UpdateItemAsync(item);
                if (updated == 0)
                {
                    return ValidationResult<ItemEntity>.Failure(
                     new[] { new ValidationError("Repository", "Failed to update item.") });
                }
                _logger.LogInformation("Item {ItemName} updated successfully.", item.ItemName);
                return ValidationResult<ItemEntity>.Success(item, "Item updated successfully.");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error updating item.");
                return ValidationResult<ItemEntity>.Failure(
                 new[] { new ValidationError("Database", "Database error.") });
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating item.");
                return ValidationResult<ItemEntity>.Failure(
                 new[] { new ValidationError("System", ex.Message) });
            }
        }

        // GET BY ID
        public async Task<Result<ItemEntity>> GetItemAsync(Guid id)
        {
            try
            {
                ItemEntity? item = await _repository.GetItemAsync(id);
                return item == null
                 ? Result<ItemEntity>.Failure($"Item {id} not found.")
                 : Result<ItemEntity>.Success(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving item {Id}", id);
                return Result<ItemEntity>.Failure("Error retrieving item.");
            }
        }

        // GET ORDER BY ORDER ID 
        public async Task<Result<IEnumerable<ItemEntity>?>> GetOrderItemsAsync(Guid id)
        {
            try
            {
                IEnumerable<ItemEntity> items = await _repository.GetOrderItemsAsync(id);
                return items == null || !items.Any()
                 ? Result<IEnumerable<ItemEntity>?>.Failure($"Order '{id}' items not found.")
                 : Result<IEnumerable<ItemEntity>?>.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving items for order {id}", id);
                return Result<IEnumerable<ItemEntity>?>.Failure("Error retrieving order items.");
            }
        }

        // DELETE
        public async Task<Result<bool>> DeleteItemAsync(Guid id)
        {
            try
            {
                ItemEntity? existing = await _repository.GetItemAsync(id);
                if (existing == null)
                {
                    return Result<bool>.Failure($"Item '{id}' not found");
                }
                int rows = await _repository.DeleteItemAsync(id);
                return rows == 0
                 ? Result<bool>.Failure("Failed to delete item.")
                 : Result<bool>.Success(true, "Item deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order {Id}", id);
                return Result<bool>.Failure("Error deleting order.");
            }
        }

        // DELETE BY ORDER ID
        public async Task<Result<bool>> DeleteOrderItemsAsync(Guid id)
        {
            try
            {
                IEnumerable<ItemEntity>? existing = await _repository.GetOrderItemsAsync(id);
                if (existing == null)
                {
                    return Result<bool>.Failure($"Order '{id}' items not found");
                }
                int rows = await _repository.DeleteOrderItemsAsync(id);
                return rows == 0
                 ? Result<bool>.Failure("Failed to delete items.")
                 : Result<bool>.Success(true, "Items deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order {Id} items", id);
                return Result<bool>.Failure("Error deleting order items.");
            }
        }

    }
}