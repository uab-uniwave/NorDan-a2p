using Shared.Application.Domain.Entities;
using Shared.Application.DTO;

namespace Shared.Application.Interfaces
{
    public interface IMapperSapa

    {

        Task<List<ItemDTO>> MapItemsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDTO>> MapMaterialsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}