using a2p.Shared.Application.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IOrderWriteProcessor
    {
        Task WriteAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}
