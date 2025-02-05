using a2p.Shared.Core.DTO;

namespace a2p.Shared.Core.Interfaces.Services.Read
{
 public interface IReadSapa_v1
 {

  Task<int> ImportPositionsAsync(List<ItemDTO> position);
  Task<int> ImportMaterialsAsync(List<MaterialDTO> material);
  Task<int> ImportGlassesAsync(List<GlassDTO> glass);
  Task<int> ImportPanelsAsync(List<PanelDTO> panel);

 }
}