using a2p.Application.DTO;
using a2p.Domain.Models;
namespace a2p.Application.Services.MappingService
{
    public interface IMapperSchuco
    {

        Task<List<ItemDTO>> MapItemsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDTO>> MapMaterialsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}