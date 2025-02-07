using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Services.Other.Mappers
{
    public interface IMaterialMapper
    {

        Task<List<MaterialDTO>> GetSapa_v1Async(A2PWorksheet wr);
        Task<List<MaterialDTO>> GetSapa_v2Async(A2PWorksheet wr);
        Task<List<MaterialDTO>> GetSchucoAsync(A2PWorksheet wr);

    }
}