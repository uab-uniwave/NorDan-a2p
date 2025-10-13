using a2p.Application.Models.DTO;


namespace a2p.Application.Abstractions
{
    public interface IMapperSapa

    {

        Task<List<ItemDTO>> MapItemsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDTO>> MapMaterialsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}