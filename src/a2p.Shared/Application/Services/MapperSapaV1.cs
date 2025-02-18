using a2p.Shared.Application.Interfaces;
using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Application.Services
{
    public class MapperSapaV1 : IMapperSapaV1
    {

        public Task<A2POrder> MapMaterialAsync(A2POrder order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }




        public Task<A2POrder> MapItemsAsync(A2POrder order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        { throw new NotImplementedException(); }

    }
}
