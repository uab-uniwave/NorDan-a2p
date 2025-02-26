using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Services.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IWriteItemService
    {
        Task InsertListAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

        Task DeleteAsync(string order);

    }
}
