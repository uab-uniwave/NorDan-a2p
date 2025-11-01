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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IValidator<OrderDto> _orderValidator;
        private readonly IValidator<ItemDto> _itemValidator;
        private readonly IValidator<MaterialDto> _materialValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
        IOrderRepository orderRepository,
        IValidator<OrderDto> orderValidator,
        IValidator<ItemDto> itemValidator,
        IValidator<MaterialDto> materialValidator,
        IMapper mapper,
        ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _orderValidator = orderValidator;
            _itemValidator = itemValidator;
            _materialValidator = materialValidator;
            _mapper = mapper;
            _logger = logger;
        }

        // CREATE
        public async Task<ValidationResult<OrderEntity>> CreateOrderAsync(OrderDto orderDto)
        {

            try
            {
                // Step 1. Validate order DTO 
                //===========================================================================================
                ValidationResult orderValidation = await _orderValidator.ValidateAsync(orderDto);
                ValidationResult<OrderEntity> orderValidationResult = orderValidation.ToValidationResult<OrderEntity>();
                if (!orderValidationResult.IsSuccess)
                {
                    return orderValidationResult;
                }
                // Step 2. mapp order DTO to order entity
                //===========================================================================================
                OrderEntity orderEntity = _mapper.Map<OrderEntity>(orderDto);
                if (orderEntity == null)
                {
                    throw new InvalidOperationException("Mapping resulted in null OrderEntity.");
                }
                orderEntity.CreatedDateTime = DateTime.Now;
                orderEntity.ModifiedDateTime = orderEntity.CreatedDateTime;

                // Step 2.1 Validate and mapp items DTOs to item entities 
                //===========================================================================================
                for (int i = 0; i < orderDto.ItemsDto.Count; i++)
                {
                    // Step 2.1.1 mapp item DTO to item entity
                    //===========================================================================================
                    ItemEntity itemEntity = _mapper.Map<ItemEntity>(orderDto.ItemsDto[i]);
                    itemEntity.CreatedDateTime = orderEntity.CreatedDateTime;
                    itemEntity.ModifiedDateTime = orderEntity.CreatedDateTime;
                    if (itemEntity == null)
                    {
                        _logger.LogError("Mapping resulted in null ItemEntity for ItemDto at index {Index}.", i);
                        continue;
                    }

                    // Step 2.1.2 add item entity to order entity
                    //===========================================================================================
                    orderEntity.Items.Add(itemEntity);
                }

                // Step 2.2 validate and mapp materials DTOs to material entities 
                //===========================================================================================
                for (int i = 0; i < orderDto.MaterialsDto.Count; i++)
                {

                    // Step 2.2.1 mapp material DTO to material entity
                    //===========================================================================================
                    MaterialEntity materialEntity = _mapper.Map<MaterialEntity>(orderDto.MaterialsDto[i]);
                    if (materialEntity == null)
                    {
                        _logger.LogError("Mapping resulted in null MaterialEntity for MaterialDto at index {Index}.", i);
                        continue;
                    }
                    materialEntity.CreatedDateTime = orderEntity.CreatedDateTime;
                    materialEntity.ModifiedDateTime = orderEntity.CreatedDateTime;

                    // Step 2.2.2 add material entity to order entity
                    //===========================================================================================
                    orderEntity.Materials.Add(materialEntity);

                }

                // Step 3. Create order entity in repository
                //===========================================================================================
                OrderEntity? created = await _orderRepository.CreateOrderAsync(orderEntity);
                if (created == null)
                {
                    return ValidationResult<OrderEntity>.Failure(
                    new[] { new ValidationError("Repository", "Failed to create order.") });
                }

                _logger.LogInformation("OrderNumber {OrderNumber} created.", created.OrderNumber);
                return ValidationResult<OrderEntity>.Success(created, "OrderNumber created successfully.");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error creating order.");
                return ValidationResult<OrderEntity>.Failure(
                 new[] { new ValidationError("Database", "Database error occurred.") });
            }
        }

        // UPDATE
        public async Task<ValidationResult<OrderEntity>> UpdateOrderAsync(OrderDto orderDto)
        {
            // Step 1. Validate
            ValidationResult orderValidation = await _orderValidator.ValidateAsync(orderDto);
            ValidationResult<OrderEntity> validationResult = orderValidation.ToValidationResult<OrderEntity>();
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                OrderEntity orderEntity = _mapper.Map<OrderEntity>(orderDto);

                OrderEntity? existing = await _orderRepository.GetOrderAsync(orderEntity.Id);
                if (existing == null)
                {
                    return ValidationResult<OrderEntity>.Failure(
                    new[] { new ValidationError(nameof(orderDto.Id), "OrderNumber not found.") });
                }

                orderEntity.ModifiedDateTime = DateTime.Now;

                int rows = await _orderRepository.UpdateOrderAsync(orderEntity);
                if (rows == 0)
                {
                    return ValidationResult<OrderEntity>.Failure(
                    new[] { new ValidationError("Repository", "Failed to update order.") });
                }

                _logger.LogInformation("OrderNumber {OrderNumber} updated successfully.", orderEntity.OrderNumber);
                return ValidationResult<OrderEntity>.Success(orderEntity, "OrderNumber updated successfully.");
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
                OrderEntity? order = await _orderRepository.GetOrderAsync(id);
                return order == null
                 ? Result<OrderEntity>.Failure($"OrderNumber {id} not found.")
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
                (IEnumerable<OrderEntity>? orders, int total) = await _orderRepository.GetOrdersAsync(page, size);
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
                OrderEntity? existing = await _orderRepository.GetOrderAsync(id);
                if (existing == null)
                {
                    return Result<bool>.Failure($"OrderNumber {id} not found.");
                }

                int rows = await _orderRepository.UpdateOrderDeliveryAddressAsync(id, deliveryAddress);
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
                OrderEntity? existing = await _orderRepository.GetOrderAsync(id);
                if (existing == null)
                {
                    return Result<bool>.Failure($"OrderNumber {id} not found.");
                }
                int rows = await _orderRepository.DeleteOrderAsync(id);
                return rows == 0
                 ? Result<bool>.Failure("Failed to delete order.")
                 : Result<bool>.Success(true, "OrderNumber deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order {Id}", id);
                return Result<bool>.Failure("Error deleting order.");
            }
        }
    }
}