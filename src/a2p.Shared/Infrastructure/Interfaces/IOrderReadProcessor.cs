using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IOrderReadProcessor
    {
        Task<List<A2POrder>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}
