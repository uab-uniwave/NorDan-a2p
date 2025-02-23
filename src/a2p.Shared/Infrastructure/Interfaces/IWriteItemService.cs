using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Services.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IWriteItemService
    {
        Task<int> InsertListAsync(List<ItemDTO> itemDTO, int salesDocumentNumber, int salesDocumentVersion, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

        Task<int> DeleteAsync(string order);

    }
}
