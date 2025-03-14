using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.DTO;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IWriteMaterialService
    {

       Task InsertListAsync( ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

         Task DeleteAsync(string order);

    }
}
