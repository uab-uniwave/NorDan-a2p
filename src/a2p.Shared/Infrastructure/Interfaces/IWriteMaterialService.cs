using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Services.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IWriteMaterialService
    {

        Task<int> InsertListAsync(List<MaterialDTO> materialDTOs, int salesDocumentNumber, int salesDocumentVersion, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

        Task<int> DeleteAsync(string order);

    }
}
