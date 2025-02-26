using a2p.Shared.Application.Services.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IOrderWriteProcessor
    {
        Task WriteAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}
