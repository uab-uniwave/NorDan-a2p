using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Services.Other.Mappers
{
    public interface IGlassMapper
    {
        Task<List<GlassDTO>> GetSapa_v1Async(A2POrderFileWorksheet wr);
        Task<List<GlassDTO>> GetSapa_v2Async(A2POrderFileWorksheet wr);
        Task<List<GlassDTO>> GetSchucoAsync(A2POrderFileWorksheet wr);
    }
}