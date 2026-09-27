using Shared.Application.Domain.Entities;
using Shared.Application.DTO;
using Shared.Application.Interfaces;

namespace Shared.Application.Services
{
    public class MapperSapa : IMapperSapa
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
