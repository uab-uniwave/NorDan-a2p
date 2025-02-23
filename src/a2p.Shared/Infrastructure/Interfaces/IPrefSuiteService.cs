using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Services.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IPrefSuiteService
    {
        // string InserItemAsync(Interop.PrefSales.SalesDoc salesDocument, double width, double height, int quantity, double weight, double price, string nomenclature, string model, string description);

        Task<(int, int)> GetSalesDocAsync(string order);

        Task<string?> MaterialsExistsAsync(string order);
        Task<string?> ItemsExistsAsync(string order);

        Task<string?> GetColorAsync(string color);

        Task<string?> InsertItemAsync(ItemDTO itemDTO, int SalesDocumentNumber, int SalesDocumentVersion);
        Task<List<string?>> InsertItemsAsync(List<ItemDTO> itemDTO, int SalesDocumentNumber, int SalesDocumentVersion, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}

