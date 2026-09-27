
using a2p.Shared.Application.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{
    public interface IPrefSuiteService
    {
        Task<(A2POrder, ProgressValue)> InsertItemsAsync(A2POrder a2pOrder, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}

