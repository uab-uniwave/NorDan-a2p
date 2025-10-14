using a2p.Domain.Entities;
using a2p.Domain.Models;

namespace a2p.Application.Interfaces
{
    public interface IReadService
    {
        Task<List<OrderEntity>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);


    }
}
