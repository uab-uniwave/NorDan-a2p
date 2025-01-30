using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Mappers
{
 public interface IGlassMapper
 {
  Task<List<GlassDTO>> MapToGlassDTOAsync(A2PWorksheet wr);
 }
}