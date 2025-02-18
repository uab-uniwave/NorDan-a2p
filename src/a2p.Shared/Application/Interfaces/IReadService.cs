using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{
    public interface IReadService
    {
        Task<IEnumerable<A2POrder>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}
