using Shared.Application.Domain.Entities;

namespace Shared.Application.Interfaces
{
    public interface IReadService
    {
        Task<List<A2POrder>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);


    }
}
