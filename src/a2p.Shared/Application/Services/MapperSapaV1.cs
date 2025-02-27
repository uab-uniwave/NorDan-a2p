using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services.Domain.Entities;

namespace a2p.Shared.Application.Services
{
    public class MapperSapaV1 : IMapperSapaV1
    {

        public Task<List<ItemDTO>> MapItemsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<MaterialDTO>> MapMaterialsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }

    }
}
