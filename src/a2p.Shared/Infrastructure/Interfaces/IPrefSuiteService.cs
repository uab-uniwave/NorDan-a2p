using a2p.Shared.Application.Services.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IPrefSuiteService
    {

        Task GetSalesDocumentStates(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

        Task<string?> GetColorAsync(string color);

        Task InsertItemsAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}

