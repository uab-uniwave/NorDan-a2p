using a2p.Application.DTOs;
using a2p.Application.Models;

namespace a2p.Application.Interfaces.Orchestrators
{
    public interface IReadService
    {
        Task<List<OrderDto>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);


    }
}
