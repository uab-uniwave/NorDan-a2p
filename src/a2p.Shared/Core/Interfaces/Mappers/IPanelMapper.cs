using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Mappers
{
 public interface IPanelMapper
 {
  Task<List<PanelDTO>> MapToPanelDTOAsync(A2PWorksheet wr);
 }
}