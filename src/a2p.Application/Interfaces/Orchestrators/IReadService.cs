using Application.DTOs;
using Application.Models;

namespace Application.Interfaces.Orchestrators
{
    public interface IReadService
    {
        Task<List<OrderDto>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}
