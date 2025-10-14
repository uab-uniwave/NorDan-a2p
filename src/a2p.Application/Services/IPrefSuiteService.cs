using a2p.Domain.Entities;
using a2p.Domain.Models;

namespace a2p.Application.Services
{
    public interface IPrefSuiteService
    {
        Task<(OrderEntity, ProgressValue)> InsertItemsAsync(OrderEntity order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}

