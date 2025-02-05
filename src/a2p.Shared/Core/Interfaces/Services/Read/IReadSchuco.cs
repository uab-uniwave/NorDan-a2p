using a2p.Shared.Core.DTO;

namespace a2p.Shared.Core.Interfaces.Services.Read
{
 public interface IReadSchuco
 {
  Task<int> ReadPositionsAsync(List<ItemDTO> positions);
  Task<int> ReadMaterialsAsync(List<MaterialDTO> materials);
  Task<int> ReadGlassesAsync(List<GlassDTO> glasses);


 }


}