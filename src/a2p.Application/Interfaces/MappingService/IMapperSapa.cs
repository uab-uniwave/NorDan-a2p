using a2p.Application.DTO;
using a2p.Domain.Models;



namespace a2p.Application.Interfaces.MappingService
{
    public interface IMapperSapa

    {

        Task<List<ItemDto>> MapItemsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDTO>> MapMaterialsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}