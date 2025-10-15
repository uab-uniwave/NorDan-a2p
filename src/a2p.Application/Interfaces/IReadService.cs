using a2p.Application.DTOs;
using a2p.Application.Models;

namespace a2p.Application.Interfaces
{
    public interface IReadService
    {
        Task<List<ExcelOrderDto>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);


    }
}
