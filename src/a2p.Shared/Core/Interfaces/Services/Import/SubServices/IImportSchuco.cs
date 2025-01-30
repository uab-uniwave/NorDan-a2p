using a2p.Shared.Core.DTO;

namespace a2p.Shared.Core.Interfaces.Services.Import.SubServices
{
 public interface IImportSchuco
 {
  Task<int> ImportPositionsAsync(List<ItemDTO> positions);
  Task<int> ImportMaterialsAsync(List<MaterialDTO> materials);
  Task<int> ImportGlassesAsync(List<GlassDTO> glasses);


 }


}