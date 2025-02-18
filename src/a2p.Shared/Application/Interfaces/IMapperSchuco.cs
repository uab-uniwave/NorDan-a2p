using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{
    public interface IMapperSchuco
    {




        Task<A2POrder> MapItemsAsync(A2POrder order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<A2POrder> MapMaterialsAsync(A2POrder order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}