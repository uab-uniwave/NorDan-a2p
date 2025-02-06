using a2p.Shared.Core.DTO;

namespace a2p.Shared.Core.Interfaces.Services
{
    public interface IWriteService
    {
        Task<int> InsertItemAsync(ItemDTO itemsDTO);
        Task<int> InsertMaterialAsync(MaterialDTO materialsDTO);
        Task<int> InsertGlassAsync(GlassDTO glassesDTO);
        Task<int> InsertPanelAsync(PanelDTO panelsDTO);

    }
}
