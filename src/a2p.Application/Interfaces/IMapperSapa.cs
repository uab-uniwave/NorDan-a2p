using a2p.Application.DTO;
using a2p.Domain.Entities;
using a2p.Shared.Application.DTO;

namespace a2p.Application.Interfaces
{
    public interface IMapperSapa

    {

        Task<List<ItemDTO>> MapItemsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDTO>> MapMaterialsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}