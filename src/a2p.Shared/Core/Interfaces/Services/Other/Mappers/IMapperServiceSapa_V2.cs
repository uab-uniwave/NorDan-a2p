using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Services.Other.Mappers
{
    public interface IMapperServiceSapa_V2
    {


        Task<List<MaterialDTO>> MapMaterialAsync(A2PWorksheet wr);
        Task<List<ItemDTO>> MapItemsAsync(A2PWorksheet wr);
    }
}