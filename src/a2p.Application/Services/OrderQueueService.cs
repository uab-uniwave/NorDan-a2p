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
            if (result.IsFailure)
            {
                return Result<OrderQueueEntity>.Failure($"Failed to insert order '{order.OrderNumber}'!");
            }
            return Result<OrderQueueEntity>.Success(result.Value);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<OrderQueueEntity>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inserting order '{order.OrderNumber}'!");
            return Result<OrderQueueEntity>.Failure($"Error inserting order '{order.OrderNumber}': {ex.Message}");
        }

    }

    public async Task<Result<OrderQueueEntity>> GetOrderAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.GetOrderAsync(rowId);
            if (result.IsFailure)
            {
                return Result<OrderQueueEntity>.Failure($"Failed to get order. Order with rowId '{rowId}' not found.");
            }
            return Result<OrderQueueEntity>.Success(result.Value);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<OrderQueueEntity>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting order rowId '{rowId}'!", ex.Message);
            return Result<OrderQueueEntity>.Failure($"Error getting order rowId '{rowId}': {ex.Message}");
        }

    }


    public async Task<Result<IEnumerable<OrderQueueEntity>>> GetOrdersAsync()
    {
        try
        {

            var result = await _repo.GetOrdersAsync();
            if (result.IsFailure)
            {
                return Result<IEnumerable<OrderQueueEntity>>.Failure($"Failed to get orders. No orders found.");
            }
            return Result<IEnumerable<OrderQueueEntity>>.Success(result.Value);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<IEnumerable<OrderQueueEntity>>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders!");
            return Result<IEnumerable<OrderQueueEntity>>.Failure($"Error getting orders: {ex.Message}");
        }

    }

    public async Task<Result<OrderQueueEntity>> UpdateOrderAsync(OrderQueueEntity order)
    {
        try
        {

            order.ModifiedUTCDateTime = DateTime.UtcNow;
            var result = await _repo.UpdateOrderAsync(order);
            if (result.IsFailure)
            {
                return Result<OrderQueueEntity>.Failure($"Failed to update order '{order.OrderNumber}'.");
            }
            return Result<OrderQueueEntity>.Success(result.Value);
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Result<OrderQueueEntity>.Failure(dex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating order '{order.OrderNumber}'!");
            return Result<OrderQueueEntity>.Failure($"Error updating order '{order.OrderNumber}': {ex.Message}");
        }

    }

    public async Task<Result<Guid>> DeleteOrderAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.DeleteOrderAsync(rowId);
            if (result.IsFailure)
            {

                return Result<Guid>.Failure($"Failed to delete order. Order rowId '{rowId}' not found!");
            }

            return Result<Guid>.Success(result.Value);
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
