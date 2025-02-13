using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Services
{
    public interface IMappingHandlerService
    {
        Task<IEnumerable<A2POrder>> MapDataAsync(IEnumerable<A2POrder> a2pOrderList, Progress<ProgressValue>? progress = null);

    }
}
