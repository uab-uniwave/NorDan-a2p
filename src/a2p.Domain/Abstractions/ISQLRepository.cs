using a2p.Domain.Entities;


namespace a2p.Domain.Abstractions
{
    public interface ISQLRepository
    {

        //============================================================================================================================
        Task<(int, int)> GetSalesDocumentAsync(string order);
        Task<int> GetSalesDocumentStateAsync(int number, int version);
        Task<string?> GetGlassReferenceAsync(string description);
        Task<int> GetPrefSuiteColorConfigurationAsync(string color);
        Task<string?> GetSapaColorAsync(string color);
        //============================================================================================================================
        Task<ErrorEntity?> DeleteSalesDocumentDataAsync(int number, int version, bool DeleteExisting);
        //============================================================================================================================
        Task<ErrorEntity?> InsertPrefSuiteColorAsync(MaterialEntity material);
        Task<ErrorEntity?> InsertPrefSuiteColorConfigurationAsync(MaterialEntity material);
        Task<ErrorEntity?> InsertPrefSuiteMaterialBaseAsync(MaterialEntity material);
        Task<ErrorEntity?> InsertPrefSuiteMaterialAsync(MaterialEntity material);
        //============================================================================================================================
        Task<ErrorEntity?> InsertPrefSuiteMaterialProfileAsync(MaterialEntity material);
        Task<ErrorEntity?> InsertPrefSuiteMaterialMeterAsync(MaterialEntity material);
        Task<ErrorEntity?> InsertPrefSuiteMaterialPieceAsync(MaterialEntity material);
        Task<ErrorEntity?> InsertPrefSuiteMaterialSurfaceAsync(MaterialEntity material);
        Task<ErrorEntity?> UpdateBCMapping(MaterialEntity material);
        Task<ErrorEntity?> InsertPrefSuiteMaterialPurchaseDataAsync(MaterialEntity material);
        Task<ErrorEntity?> InsertOrderMaterialDTOAsync(MaterialEntity material, int number, int version);
        //============================================================================================================================
        Task<ErrorEntity?> InsertOrderItemAsync(ItemEntity Item, int number, int version, string idPos);
        Task<ErrorEntity?> InsertPrefSuiteMaterialNeedsMasterAsync(string order, int number, int version);
        Task<ErrorEntity?> InsertPrefSuiteMaterialNeedsAsync(string order, int number, int version);

    }
}
