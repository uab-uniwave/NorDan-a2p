using Application.DTOs;
using Application.Models;
namespace Application.Interfaces.Orchestrators
{
    public interface IWriteService
    {
        Task WriteAsync(OrderDto orders, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}
