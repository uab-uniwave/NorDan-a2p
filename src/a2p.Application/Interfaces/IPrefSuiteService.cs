using a2p.Application.DTOs;
using a2p.Application.Models;

namespace a2p.Application.Interfaces
{
    public interface IPrefSuiteService
    {
        Task InsertItemsAsync(ExcelOrderDto order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}

