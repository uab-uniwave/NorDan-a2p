using a2p.Application.Models;
using a2p.Application.Services;
using a2p.Domain.Entities;
using a2p.Domain.Exception;
using a2p.Domain.Interfaces;
using Microsoft.Extensions.Logging;

public class OrderQueueService : IOrderQueueService
{
    private readonly IOrderQueueRepository _repo;
    private readonly ILogger<OrderQueueService> _logger;

    public OrderQueueService(IOrderQueueRepository repo, ILogger<OrderQueueService> logger)
    {
        _repo = repo;
        _logger = logger;

    }

    public async Task<Result<OrderQueueEntity>> InsertOrderQueueAsync(OrderQueueEntity order)
    {
        try
        {

            order.CreatedUTCDateTime = DateTime.UtcNow;
            var result = await _repo.InsertOrderAsync(order);
            if (!result.IsSuccess)
            {
                return Result.Failure<OrderQueueEntity>($"Failed to insert order '{order.OrderNumber}'!");
            }
            return Result.Success(result.Value!);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<OrderQueueEntity>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inserting order '{order.OrderNumber}'!");
            return Result.Failure<OrderQueueEntity>($"Error inserting order '{order.OrderNumber}': {ex.Message}");
        }

    }

    public async Task<Result<OrderQueueEntity>> GetOrderAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.GetOrderAsync(rowId);
            if (!result.IsSuccess)
            {
                return Result.Failure<OrderQueueEntity>($"Failed to get order. Order with rowId '{rowId}' not found.");
            }
            return Result.Success(result.Value!);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<OrderQueueEntity>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting order rowId '{rowId}'!");
            return Result.Failure<OrderQueueEntity>($"Error getting order rowId '{rowId}': {ex.Message}");
        }

    }


    public async Task<Result<IEnumerable<OrderQueueEntity>>> GetOrdersAsync()
    {
        try
        {

            var result = await _repo.GetOrdersAsync();
            if (!result.IsSuccess)
            {
                return Result.Failure<IEnumerable<OrderQueueEntity>>($"Failed to get orders. No orders found.");
            }
            return Result.Success(result.Value!);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<IEnumerable<OrderQueueEntity>>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders!");
            return Result.Failure<IEnumerable<OrderQueueEntity>>($"Error getting orders: {ex.Message}");
        }

    }

    public async Task<Result<OrderQueueEntity>> UpdateOrderAsync(OrderQueueEntity order)
    {
        try
        {

            order.ModifiedUTCDateTime = DateTime.UtcNow;
            var result = await _repo.UpdateOrderAsync(order);
            if (!result.IsSuccess)
            {
                return Result.Failure<OrderQueueEntity>($"Failed to update order '{order.OrderNumber}'.");
            }
            return Result.Success(result.Value!);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<OrderQueueEntity>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating order '{order.OrderNumber}'!");
            return Result.Failure<OrderQueueEntity>($"Error updating order '{order.OrderNumber}': {ex.Message}");
        }

    }

    public async Task<Result<Guid>> DeleteOrderAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.DeleteOrderAsync(rowId);
            if (!result.IsSuccess || result.Value == Guid.Empty)
            {

                return Result.Failure<Guid>($"Failed to delete order. Order rowId '{rowId}' not found!");
            }

            return Result.Success(result.Value);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result.Failure<Guid>(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting order. Order rowId '{rowId}'!");
            return Result.Failure<Guid>($"Error deleting order. Order rowId '{rowId}': {ex.Message}");
        }

    }
}
