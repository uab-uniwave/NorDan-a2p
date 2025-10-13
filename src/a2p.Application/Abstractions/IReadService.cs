using a2p.Shared.Application.Domain.Entities;

namespace a2p.Application.Abstractions
{
    public interface IReadService
    {
        Task<List<A2POrder>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);


    }
}
