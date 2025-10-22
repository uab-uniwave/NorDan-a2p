using Application.DTOs;

using Domain.Entities;
using Domain.Shared;

namespace Application.Interfaces.Services
{
    public interface IOrderService
    {
        // CREATE

        Task<ValidationResult<OrderEntity>> CreateOrderAsync(OrderDto dto);

        // UPDATE
        Task<ValidationResult<OrderEntity>> UpdateOrderAsync(OrderDto dto);

        // GET BY ID
        Task<Result<OrderEntity>> GetOrderByIdAsync(Guid id);

        // PAGED
        Task<PagedResult<OrderEntity>> GetOrdersAsync(int page, int size);

        // UPDATE DELIVERY ADDRESS
        Task<Result<bool>> UpdateOrderDeliveryAddressAsync(Guid id, string deliveryAddress);

        Task<Result<bool>> DeleteOrderByIdAsync(Guid id);
    }
}

