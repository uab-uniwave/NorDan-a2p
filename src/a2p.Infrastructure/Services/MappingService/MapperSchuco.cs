using a2p.Application.DTO;
using a2p.Application.Services.MappingService;
using a2p.Domain.Models;
namespace a2p.Infrastructure.Services.MappingService
{
    public class MapperSchuco : IMapperSchuco
    {

        public Task<List<ItemDTO>> MapItemsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<MaterialDTO>> MapMaterialsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }

    }
}