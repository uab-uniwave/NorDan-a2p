using a2p.Application.DTOs;
using a2p.Application.Models;
namespace a2p.Application.Interfaces.Orchestrators
{
    public interface IWriteService
    {
        Task WriteAsync(OrderDto orders, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}
