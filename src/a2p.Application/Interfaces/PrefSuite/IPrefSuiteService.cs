using a2p.Application.DTOs;
using a2p.Application.Models;

namespace a2p.Application.Interfaces.PrefSuite
{
    public interface IPrefSuiteService
    {
        Task InsertItemsAsync(OrderDto order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}

