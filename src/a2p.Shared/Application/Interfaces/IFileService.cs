using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{




    public interface IFileService
    {
        //Task<List<OrderEntry>> GetSingleOrderFilesAsync(IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<A2POrder>> GetOrdersAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

        // Task<A2POrder> GetOrderFilesAsync(A2POrder order);

        void MoveFilesAsync(string order, string fileName);

        Task<bool> IsLockedAsync(string filePath);
    }

}