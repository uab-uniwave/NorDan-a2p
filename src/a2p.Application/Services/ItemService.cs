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

                item.ModifiedUTCDateTime = DateTime.UtcNow;

                // ItemRepository.UpdateItemAsync returns updated item (nullable)
                var updated = await _repository.UpdateItemAsync(item);
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
                _logger.LogError(ex, "ErrorDto retrieving item {Id}", id);
                return Result<ItemEntity>.Failure("ErrorDto retrieving item.");
            }
        }

        // GET ORDER ITEMS
        public async Task<PagedResult<IEnumerable<ItemEntity>?>> GetOrderItemsAsync(Guid id, int page, int size)
        {
            try
            {
                (IEnumerable<ItemEntity> Ir, int TotalCount) items = await _repository.GetOrderItems(id, page, size);
                return PagedResult<IEnumerable<ItemEntity>?>.Failure($"Items for order '{id}' not found.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ErrorDto retrieving items for order {id}", id);
                return PagedResult<IEnumerable<ItemEntity>?>.Failure("ErrorDto retrieving order items.");
            }
        }

        // PAGED (repository doesn't expose paged; do simple in-memory paging)
        public async Task<PagedResult<ItemEntity>> GetItemsAsync(int page, int size)
        {
            try
            {
                (IEnumerable<ItemEntity>? items, int total) = await _repository.GetItemsAsync(page, size);

                return PagedResult<ItemEntity>.Success(items, total, page, size);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ErrorDto retrieving paged items.");
                return PagedResult<ItemEntity>.Failure("ErrorDto retrieving paged items.");
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
                    return Result<bool>.Failure($"Order {id} not found");
                }
                var rows = await _repository.DeleteItemAsync(id);
                return rows == 0
                    ? Result<bool>.Failure("Failed to delete order.")
                    : Result<bool>.Success(true, "Order deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ErrorDto deleting order {Id}", id);
                return Result<bool>.Failure("ErrorDto deleting order.");
            }
        }

    }
}