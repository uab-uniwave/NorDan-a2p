using a2p.Shared.Core.DTO;

namespace a2p.Shared.Core.Interfaces.Services.Import.SubServices
{
 public interface IImportSapa_v2
 {
  Task<int> ImportItemsAsync(List<ItemDTO> positions);
  Task<int> ImportMaterialsAsync(List<MaterialDTO> materials);
  Task<int> ImportGlassesAsync(List<GlassDTO> glasses);
  Task<int> ImportPanelsAsync(List<PanelDTO> panels);

 }
}
