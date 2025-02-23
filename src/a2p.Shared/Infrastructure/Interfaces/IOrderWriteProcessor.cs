using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IOrderWriteProcessor
    {
        Task WriteAsync(List<A2POrder> orders, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}
