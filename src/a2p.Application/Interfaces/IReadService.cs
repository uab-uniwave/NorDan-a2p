using a2p.Application.DTO;
using a2p.Domain.Entities;

namespace a2p.Application.Interfaces
{
    public interface IReadService
    {
        Task<List<A2POrderDto>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);


    }
}
