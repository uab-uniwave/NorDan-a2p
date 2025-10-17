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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IValidator<OrderDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository repository,
            IValidator<OrderDto> validator,
            IMapper mapper,
            ILogger<OrderService> logger)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        // CREATE
        public async Task<ValidationResult<OrderEntity>> CreateOrderAsync(OrderDto dto)
        {
            // Step 1. Validate input
            ValidationResult validation = await _validator.ValidateAsync(dto);
            ValidationResult<OrderEntity> validationResult = validation.ToValidationResult<OrderEntity>();
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                OrderEntity entity = _mapper.Map<OrderEntity>(dto)
                ?? throw new InvalidOperationException("Mapping resulted in null OrderEntity.");
                OrderEntity? created = await _repository.CreateOrderAsync(entity);
                if (created == null)
                {
                    return ValidationResult<OrderEntity>.Failure(
                        new[] { new ValidationError("Repository", "Failed to create order.") });
                }

                _logger.LogInformation("Order {OrderNumber} created.", created.OrderNumber);
                return ValidationResult<OrderEntity>.Success(created, "Order created successfully.");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error creating order.");
                return ValidationResult<OrderEntity>.Failure(
                    new[] { new ValidationError("Database", "Database error occurred.") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating order.");
                return ValidationResult<OrderEntity>.Failure(
                    new[] { new ValidationError("System", ex.Message) });
            }
        }

        // UPDATE
        public async Task<ValidationResult<OrderEntity>> UpdateOrderAsync(OrderDto dto)
        {
            // Step 1. Validate
            ValidationResult validation = await _validator.ValidateAsync(dto);
            ValidationResult<OrderEntity> validationResult = validation.ToValidationResult<OrderEntity>();
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                OrderEntity entity = _mapper.Map<OrderEntity>(dto);

                OrderEntity? existing = await _repository.GetOrderAsync(entity.Id);
                if (existing == null)
                {
                    return ValidationResult<OrderEntity>.Failure(
                        new[] { new ValidationError(nameof(dto.Id), "Order not found.") });
                }

                entity.ModifiedUTCDateTime = DateTime.UtcNow;

                int rows = await _repository.UpdateOrderAsync(entity);
                if (rows == 0)
                {
                    return ValidationResult<OrderEntity>.Failure(
                        new[] { new ValidationError("Repository", "Failed to update order.") });
                }

                _logger.LogInformation("Order {OrderNumber} updated successfully.", entity.OrderNumber);
                return ValidationResult<OrderEntity>.Success(entity, "Order updated successfully.");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error updating order.");
                return ValidationResult<OrderEntity>.Failure(
                    new[] { new ValidationError("Database", "Database error.") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating order.");
                return ValidationResult<OrderEntity>.Failure(
                    new[] { new ValidationError("System", ex.Message) });
            }
        }

        // GET BY ID
        public async Task<Result<OrderEntity>> GetOrderByIdAsync(Guid id)
        {
            try
            {
                OrderEntity? order = await _repository.GetOrderAsync(id);
                return order == null
                    ? Result<OrderEntity>.Failure($"Order {id} not found.")
                    : Result<OrderEntity>.Success(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order {Id}", id);
                return Result<OrderEntity>.Failure("Error retrieving order.");
            }
        }

        // PAGED
        public async Task<PagedResult<OrderEntity>> GetOrdersAsync(int page, int size)
        {
            try
            {
                (IEnumerable<OrderEntity>? orders, int total) = await _repository.GetOrdersAsync(page, size);
                return PagedResult<OrderEntity>.Success(orders, total, page, size);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged orders.");
                return PagedResult<OrderEntity>.Failure("Error retrieving paged orders.");
            }
        }

        // UPDATE DELIVERY ADDRESS
        public async Task<Result<bool>> UpdateOrderDeliveryAddressAsync(Guid id, string deliveryAddress)
        {
            try
            {
                OrderEntity? existing = await _repository.GetOrderAsync(id);
                if (existing == null)
                {
                    return Result<bool>.Failure($"Order {id} not found.");
                }

                int rows = await _repository.UpdateOrderDeliveryAddressAsync(id, deliveryAddress);
                return rows == 0
                    ? Result<bool>.Failure("Failed to update delivery address.")
                    : Result<bool>.Success(true, "Delivery address updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating delivery address for order {Id}", id);
                return Result<bool>.Failure("Error updating delivery address.");
            }
        }

        // DELETE
        public async Task<Result<bool>> DeleteOrderByIdAsync(Guid id)
        {
            try
            {
                OrderEntity? existing = await _repository.GetOrderAsync(id);
                if (existing == null)
                {
                    return Result<bool>.Failure($"Order {id} not found.");
                }
                int rows = await _repository.DeleteOrderAsync(id);
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