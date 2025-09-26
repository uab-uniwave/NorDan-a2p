using a2p.Application.DTO;
using a2p.Domain.Entities;

namespace a2p.Application.Interfaces
{
    public interface IPrefSuiteService
    {
        Task<(A2POrderDto, ProgressValue)> InsertItemsAsync(A2POrderDto a2pOrder, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}

