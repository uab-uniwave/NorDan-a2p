using Application.Models;

using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IPrefSuiteRepository
    {

        Task<SalesDocument?> GetSalesDocumentByOrderAsync(string orderNumber);
        Task<SalesDocument?> GetSalesDocumentByRowIdAsync(Guid rowId);

        Task<string?> GetGlassReferenceAsync(string description);

        Task<int?> GetPrefSuiteColorConfigurationAsync(string color);

        Task<int?> GetCommodityCode(string sourceReference);

        Task<decimal?> GetTechDesignWeight(string sourceReference);

        Task<string?> GetSapaColorAsync(string color);

        Task DeleteSalesDocumentDataAsync(int number, int version, bool deleteExisting);

        Task InsertPrefSuiteColorAsync(MaterialEntity material);

        Task InsertPrefSuiteColorConfigurationAsync(MaterialEntity material);

        Task InsertPrefSuiteMaterialBaseAsync(MaterialEntity material);

        Task InsertPrefSuiteMaterialAsync(MaterialEntity material);

        Task InsertPrefSuiteMaterialProfileAsync(MaterialEntity material);

        Task InsertPrefSuiteMaterialMeterAsync(MaterialEntity material);

        Task InsertPrefSuiteMaterialPieceAsync(MaterialEntity material);

        Task InsertPrefSuiteMaterialSurfaceAsync(MaterialEntity material);

        Task InsertPrefSuiteMaterialPurchaseDataAsync(MaterialEntity material);

        Task UpdateBCMapping(MaterialEntity material);

        Task InsertPrefSuiteMaterialNeedsMasterAsync(Guid? orderId);

        Task InsertPrefSuiteMaterialNeedsAsync(Guid? orderId);
    }
}
