using System.Linq;
using a2p.Application.Models;
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
            return Result.Success(result);
        }
        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<OrderEntity>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting order '{OrderNumber}'!", order.OrderNumber);
            return Result.Failure<OrderEntity>($"Error inserting order '{order.OrderNumber}': {ex.Message}");
        }
    }

    public async Task<Result<OrderEntity?>> GetOrderAsync(Guid id)
    {
        try
        {
            var result = await _repo.GetOrderAsync(id);
            if (result == null || result.Id == Guid.Empty)
            {
                return Result.Failure<OrderEntity?>($"Failed to get order. Order with rowId '{id}' not found.");
            }
            return Result.Success<OrderEntity?>(result);
        }
        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<OrderEntity?>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order rowId '{RowId}'!", id);
            return Result.Failure<OrderEntity?>($"Error getting order rowId '{id}': {ex.Message}");
        }
    }


    public async Task<Result<IEnumerable<OrderEntity>>?> GetOrdersAsync()
    {
        try
        {
            var result = await _repo.GetOrdersAsync();
            if (result == null || !result.Any())
            {
                return Result.Failure<IEnumerable<OrderEntity>>($"Failed to get orders. Orders not found.");
            }
            return Result.Success(result);
        }
        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<IEnumerable<OrderEntity>>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders!");
            return Result.Failure<IEnumerable<OrderEntity>>($"Error getting orders: {ex.Message}");
        }
    }

    public async Task<Result<OrderEntity?>> UpdateOrderAsync(OrderEntity order)
    {
        try
        {
            order.ModifiedUTCDateTime = DateTime.UtcNow;
            var result = await _repo.UpdateOrdrAsync(order);
            if (result == null || result.Id == Guid.Empty)
            {
                return Result.Failure<OrderEntity?>($"Failed to update order. OrderNumber '{order.OrderNumber}'");
            }
            return Result.Success<OrderEntity?>(result);
        }
        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<OrderEntity?>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order '{OrderNumber}'!", order.OrderNumber);
            return Result.Failure<OrderEntity?>($"Error updating order '{order.OrderNumber}': {ex.Message}");
        }
    }

    public async Task<Result<Guid>> DeleteOrderAsync(Guid id)
    {
        try
        {
            var result = await _repo.DeleteOrderAsync(id);
            if (result == Guid.Empty)
            {
                return Result.Failure<Guid>($"Failed to delete order. Order rowId '{id}' not found!");
            }

            return Result.Success(result);
        }
        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<Guid>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting order. Order rowId '{RowId}'!", id);
            return Result.Failure<Guid>($"Error deleting order. Order rowId '{id}': {ex.Message}");
        }
    }
}
