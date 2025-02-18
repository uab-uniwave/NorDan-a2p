using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{
    public interface IOrderProcessingService
    {
        Task<A2POrder> MapDataAsync(A2POrder order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}
