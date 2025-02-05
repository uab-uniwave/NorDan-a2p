using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Services.Read
{
    public interface IReadService
    {
        Task ReadDataAsync(IEnumerable<A2POrder> orderList, IProgress<ProgressValue>? progress = null);

    }
}
