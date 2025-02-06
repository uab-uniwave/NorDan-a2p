using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Services
{
    public interface IMappingService
    {
        Task MapDataAsync(IEnumerable<A2POrder> orderList, IProgress<ProgressValue>? progress = null);

    }
}
