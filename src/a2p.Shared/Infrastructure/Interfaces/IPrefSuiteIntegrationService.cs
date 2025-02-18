using a2p.Shared.Core.DTO;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IPrefSuiteIntegrationService
    {
        // string AddItem(Interop.PrefSales.SalesDoc salesDocument, double width, double height, int quantity, double weight, double price, string nomenclature, string model, string description);

        Task<(int, int)> GetSalesDocAsync(string order);

        Task<string?> MaterialsExistsAsync(string order);
        Task<string?> ItemsExistsAsync(string order);

        Task<string?> GetColorAsync(string color);

        Task<string> AddItem(ItemDTO itemDTO, int SalesDocumentNumber, int SalesDocumentVersion);
    }
}



