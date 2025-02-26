using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Infrastructure.Interfaces;

public class DataCache
{
    private readonly Dictionary<string, A2POrder> _orderCache = [];
    private readonly ILogService _logService;

    public DataCache(ILogService logService)
    {
        _logService = logService;
    }

    /// <summary>
    /// Adds or updates an order in the cache.
    /// </summary>
    public void AddOrder(A2POrder order)
    {
        lock (_orderCache)
        {
            if (!_orderCache.ContainsKey(order.Order))
            {
                _orderCache[order.Order] = order;
                _logService.Information($"Order {order.Order} added to cache.");
            }
            else
            {
                _orderCache[order.Order].Items.AddRange(order.Items);
                _orderCache[order.Order].Materials.AddRange(order.Materials);
                _orderCache[order.Order].Files.AddRange(order.Files);
                _logService.Information($"Order {order.Order} updated in cache.");
            }
        }
    }

    /// <summary>
    /// Retrieves an order from the cache.
    /// </summary>
    public A2POrder? GetOrder(string orderId)
    {
        lock (_orderCache)
        {
            if (_orderCache.TryGetValue(orderId, out A2POrder? order))
            {
                _logService.Information($"Retrieved order {orderId} from cache.");
                return order;
            }
            _logService.Warning($"Order {orderId} not found in cache.");
            return null;
        }
    }

    /// <summary>
    /// Updates an order in the cache using an update action.
    /// </summary>
    public void UpdateOrderInCache(string orderId, Action<A2POrder> updateAction)
    {
        lock (_orderCache)
        {
            if (_orderCache.TryGetValue(orderId, out A2POrder? order))
            {
                updateAction(order);
                _logService.Information($"Order {orderId} updated in cache.");
            }
            else
            {
                _logService.Warning($"Attempted to update non-existing order {orderId}.");
            }
        }
    }

    /// <summary>
    /// Saves an updated order back to the cache.
    /// </summary>
    public void SaveOrder(A2POrder order)
    {
        lock (_orderCache)
        {
            _orderCache[order.Order] = order;
            _logService.Information($"Order {order.Order} saved in cache.");
        }
    }

    /// <summary>
    /// Removes an order from the cache.
    /// </summary>
    public bool RemoveOrder(string orderId)
    {
        lock (_orderCache)
        {
            if (_orderCache.Remove(orderId))
            {
                _logService.Information($"Order {orderId} removed from cache.");
                return true;
            }
            else
            {
                _logService.Warning($"Attempted to remove non-existing order {orderId}.");
                return false;
            }
        }
    }

    /// <summary>
    /// Retrieves all orders from the cache.
    /// </summary>
    public List<A2POrder> GetAllOrders()
    {
        lock (_orderCache)
        {
            return _orderCache.Values.ToList();
        }
    }

    /// <summary>
    /// Clears the entire cache.
    /// </summary>
    public void ClearCache()
    {
        lock (_orderCache)
        {
            _orderCache.Clear();
            _logService.Information("Cache cleared.");
        }
    }
}