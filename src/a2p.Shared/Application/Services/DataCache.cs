using a2p.Shared.Domain.Entities;

public class DataCache
{
    private static readonly Lazy<DataCache> _instance = new(() => new DataCache());
    private readonly Dictionary<string, A2POrder> _orderCache = []; // Key: Order ID

    private DataCache() { }

    public static DataCache Instance => _instance.Value;

    public void AddOrder(A2POrder order)
    {
        lock (_orderCache)
        {
            _orderCache[order.Order] = order;
        }
    }

    public List<A2POrder> GetAllOrders()
    {
        lock (_orderCache)
        {
            return _orderCache.Values.ToList();
        }
    }

    public void ClearCache()
    {
        lock (_orderCache)
        {
            _orderCache.Clear();
        }
    }
}