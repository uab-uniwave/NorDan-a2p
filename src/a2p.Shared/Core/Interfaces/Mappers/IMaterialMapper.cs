using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Mappers
{
 public interface IMaterialMapper
 {

  Task<List<MaterialDTO>> MapToMaterialDTOAsync(A2PWorksheet wr);
 }
}