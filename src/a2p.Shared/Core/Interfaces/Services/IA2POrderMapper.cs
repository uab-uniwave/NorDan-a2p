using a2p.Shared.Core.DTO.a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Services
{

    public interface IA2POrderMapper
    {
        Task<OrderDTO> MapToOrderDTOAsync(A2POrder a2pOrder);


    }
}
