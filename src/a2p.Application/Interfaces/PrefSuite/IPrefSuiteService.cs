using Application.DTOs;
using Application.Models;

namespace Application.Interfaces.PrefSuite
{
    public interface IPrefSuiteService
    {
        Task InsertItemsAsync(OrderDto order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}

