using a2p.Shared.Application.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{
    public interface IReadService
    {
        Task<List<A2POrder>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);


    }
}
