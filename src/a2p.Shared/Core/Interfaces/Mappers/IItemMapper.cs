using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Mappers
{
 public interface IItemMapper
 {
  Task<List<ItemDTO>> MapToPositionDTOAsync(A2PWorksheet wr);
 }
}
