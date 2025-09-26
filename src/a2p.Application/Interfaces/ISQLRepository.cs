using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.DTO;

namespace a2p.Application.Interfaces
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
        Task<A2PErrorDto?> DeleteSalesDocumentDataAsync(int number, int version, bool DeleteExisting);
        //============================================================================================================================
        Task<A2PErrorDto?> InsertPrefSuiteColorAsync(MaterialDTO materialDTO);
        Task<A2PErrorDto?> InsertPrefSuiteColorConfigurationAsync(MaterialDTO materialDTO);
        Task<A2PErrorDto?> InsertPrefSuiteMaterialBaseAsync(MaterialDTO materialDTO);
        Task<A2PErrorDto?> InsertPrefSuiteMaterialAsync(MaterialDTO materialDTO);
        //============================================================================================================================
        Task<A2PErrorDto?> InsertPrefSuiteMaterialProfileAsync(MaterialDTO materialDTO);
        Task<A2PErrorDto?> InsertPrefSuiteMaterialMeterAsync(MaterialDTO materialDTO);
        Task<A2PErrorDto?> InsertPrefSuiteMaterialPieceAsync(MaterialDTO materialDTO);
        Task<A2PErrorDto?> InsertPrefSuiteMaterialSurfaceAsync(MaterialDTO materialDTO);
        Task<A2PErrorDto?> UpdateBCMapping(MaterialDTO materialDTO);
        Task<A2PErrorDto?> InsertPrefSuiteMaterialPurchaseDataAsync(MaterialDTO materialDTO);
        Task<A2PErrorDto?> InsertOrderMaterialDTOAsync(MaterialDTO materialDTO, int number, int version);
        //============================================================================================================================
        Task<A2PErrorDto?> InsertOrderItemDTOAsync(ItemDTO itemDTO, int number, int version, string idPos);
        Task<A2PErrorDto?> InsertPrefSuiteMaterialNeedsMasterAsync(string order, int number, int version);
        Task<A2PErrorDto?> InsertPrefSuiteMaterialNeedsAsync(string order, int number, int version);

    }
}
