using a2p.Shared.Core.DTO.a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Mappers
{

 public interface IOrderMapper
 {
  Task<OrderDTO> MapToOrderDTOAsync(A2POrder a2pOrder);


 }
}
