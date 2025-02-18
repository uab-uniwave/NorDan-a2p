using a2p.Shared.Core.DTO.a2p.Shared.Core.DTO;
using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Application.Services
{

    public interface IA2POrderMapper
    {
        Task<OrderDTO> MapToOrderDTOAsync(A2POrder a2pOrder);


    }
}
