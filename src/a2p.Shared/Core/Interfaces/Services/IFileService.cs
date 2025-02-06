using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Services
{




    public interface IFileService
    {
        //Task<List<OrderEntry>> GetSingleOrderFilesAsync(IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<A2POrder>> GetOrdersAsync(List<A2POrder> a2pOrderList, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

        void MoveFilesAsync(string order, string fileName);

        Task<bool> IsLockedAsync(string filePath);

    }

}