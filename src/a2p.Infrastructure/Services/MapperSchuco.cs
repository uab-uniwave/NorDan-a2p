using a2p.Application.DTO;
using a2p.Application.Interfaces;
using a2p.Domain.Entities;
using a2p.Shared.Application.DTO;

namespace a2p.Infrastructure.Services
{
    public class MapperSchuco : IMapperSchuco
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