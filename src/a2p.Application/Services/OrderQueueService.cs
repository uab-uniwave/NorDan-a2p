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

    public async Task<OrderQueueEntity?> InsertOrderQueueAsync(OrderQueueEntity order)
    {
        try
        {


            var result = await _repo.InsertOrderAsync(order);
            return result;


        }

        catch (DomainException dex)
        {
            _logger.LogError(dex, "Domain error inserting order!");
            return null;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting order!");
            return null;

        }

    }

    public async Task<OrderQueueEntity?> GetOrderAsync(Guid rowId)
    {
        try
        {

            var result = await _repo.GetOrderAsync(rowId);
            return result;
        }

        catch (DomainException dex)
        {
            _logger.LogError(dex, "Domain error getting order!");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order!");
            return null;
        }

    }


    public async Task<IEnumerable<OrderQueueEntity>?> GetOrdersAsync()
    {
        try
        {

            var result = await _repo.GetOrdersAsync();
            return result;


        }

        catch (DomainException dex)
        {
            _logger.LogError(dex, "Domain error getting orders!");

            return null;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders!");
            return null;

        }

    }

    public async Task<OrderQueueEntity?> UpdateOrderAsync(OrderQueueEntity order)
    {
        try
        {

            order.ModifiedUTCDateTime = DateTime.UtcNow;
            var result = await _repo.InsertOrderAsync(order);
            return result;

        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return null;

        }
        catch (Exception ex)
        {
            return null;
        }

    }

    public async Task<Guid> DeleteOrderAsync(Guid id)
    {
        try
        {

            var result = await _repo.DeleteOrderAsync(id);
            return result;
        }

        catch (DomainException dex)
        {
            // domain rule violation - handled gracefully
            return Guid.Empty;
        }
        catch (Exception ex)
        {
            return Guid.Empty;

        }

    }
}
