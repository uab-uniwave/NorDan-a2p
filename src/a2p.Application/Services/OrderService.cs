using a2p.Application.DTOs;
using a2p.Domain.Entities;

using a2p.Application.Services;
using a2p.Domain.Exception;
using a2p.Domain.Interfaces;

using Microsoft.Extensions.Logging;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository repo, ILogger<OrderService> logger)
    {
        _repo = repo;
        _logger = logger;

    }

    public async Task<Result<OrderEntity>> InsertOrderAsync(OrderEntity order)
    {
        try
        {

            order.CreatedUTCDateTime = DateTime.UtcNow;
            var result = await _repo.InsertOrderAsync(order);
            if (result == null || result.Id == Guid.Empty)
            {
                return Result.Failure<OrderEntity>($"Failed to insert order '{order.OrderNumber}'!");
            }
            return Result<OrderEntity>.Success(result);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<OrderEntity>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inserting order '{order.OrderNumber}'!");
            return Result.Failure<OrderEntity>($"Error inserting order '{order.OrderNumber}': {ex.Message}");
        }

    }

    public async Task<Result<OrderEntity>> GetOrderAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.GetOrderAsync(rowId);
            if (result == null || result.Id == Guid.Empty)
            {
                return Result.Failure<OrderEntity>($"Failed to get order. Order with rowId '{rowId}' not found.");
            }
            return Result<OrderEntity>.Success(result);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<OrderEntity>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting order rowId '{rowId}'!", ex.Message);
            return Result.Failure<OrderEntity>($"Error getting order rowId '{rowId}': {ex.Message}");
        }

    }


    public async Task<Result<IEnumerable<OrderEntity>>> GetOrdersAsync()
    {
        try
        {

            var result = await _repo.GetOrdersAsync();
            if (result == null || !result.Any())
            {
                return Result.Failure<IEnumerable<OrderEntity>>($"Failed to get orders. Orders not found.");
            }
            return Result<IEnumerable<OrderEntity>>.Success(result);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<IEnumerable<OrderEntity>>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders!");
            return Result<IEnumerable<OrderEntity>>.Failure($"Error getting orders: {ex.Message}");
        }

    }

    public async Task<Result<OrderEntity>> UpdateOrderAsync(OrderEntity order)
    {
        try
        {

            order.ModifiedUTCDateTime = DateTime.UtcNow;
            var result = await _repo.UpdateOrderAsync(order);
            if (result == null || result.Id == Guid.Empty)
            {
                return Result<OrderEntity>.Failure($"Failed to update order. OrderNumber '{order.OrderNumber}'");
            }
            return Result<OrderEntity>.Success(result);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<OrderEntity>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating order '{order.OrderNumber}'!");
            return Result<OrderEntity>.Failure($"Error updating order '{order.OrderNumber}': {ex.Message}");
        }

    }

    public async Task<Result<Guid>> DeleteOrderAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.DeleteOrderAsync(rowId);
            if (result == Guid.Empty)
            {

                return Result<Guid>.Failure($"Failed to delete order. Order rowId '{rowId}' not found!");
            }

            return Result<Guid>.Success(result);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<Guid>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting order. Order rowId '{rowId}'!", ex.Message);
            return Result<Guid>.Failure($"Error deleting order. Order rowId '{rowId}': {ex.Message}");
        }

    }
}
