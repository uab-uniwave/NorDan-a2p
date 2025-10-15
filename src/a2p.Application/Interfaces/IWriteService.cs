using a2p.Application.DTOs;
using a2p.Application.Models;
namespace a2p.Application.Interfaces
{
    public interface IWriteService
    {
        Task WriteAsync(ExcelOrderDto orders, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}
