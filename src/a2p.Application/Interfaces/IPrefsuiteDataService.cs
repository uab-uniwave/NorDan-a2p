using a2p.Domain.Entities;

namespace a2p.Application.Interfaces
{
    public interface IPrefSuiteDataService
    {

        //============================================================================================================================
        Task<(int?, int?)> GetSalesDocumentAsync(string order);
        Task<int> GetSalesDocumentStateAsync(int number, int version);
        Task<string?> GetGlassReferenceAsync(string description);
        Task<int> GetPrefSuiteColorConfigurationAsync(string color);
        Task<string?> GetSapaColorAsync(string color);
        //============================================================================================================================
        Task DeleteSalesDocumentDataAsync(int number, int version, bool DeleteExisting);
        //============================================================================================================================
        Task InsertPrefSuiteColorAsync(MaterialEntity material);
        Task InsertPrefSuiteColorConfigurationAsync(MaterialEntity material);
        Task InsertPrefSuiteMaterialBaseAsync(MaterialEntity material);
        Task InsertPrefSuiteMaterialAsync(MaterialEntity material);
        //============================================================================================================================
        Task InsertPrefSuiteMaterialProfileAsync(MaterialEntity material);
        Task InsertPrefSuiteMaterialMeterAsync(MaterialEntity material);
        Task InsertPrefSuiteMaterialPieceAsync(MaterialEntity material);
        Task InsertPrefSuiteMaterialSurfaceAsync(MaterialEntity material);
        Task UpdateBCMapping(MaterialEntity material);
        Task InsertPrefSuiteMaterialPurchaseDataAsync(MaterialEntity material);
        //============================================================================================================================
        Task InsertPrefSuiteMaterialNeedsMasterAsync(string order, int number, int version);
        Task InsertPrefSuiteMaterialNeedsAsync(string order, int number, int version);

    }
}
