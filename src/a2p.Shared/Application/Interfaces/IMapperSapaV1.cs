using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{
    public interface IMapperSapaV1
    {


        Task<A2POrder> MapItemsAsync(A2POrder order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<A2POrder> MapMaterialAsync(A2POrder order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}