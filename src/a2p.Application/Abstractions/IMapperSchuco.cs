using a2p.Application.Models.DTO;
using a2p.Shared.Application.Domain.Entities;

namespace a2p.Application.Abstractions
{
    public interface IMapperSchuco
    {

        Task<List<ItemDTO>> MapItemsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDTO>> MapMaterialsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}