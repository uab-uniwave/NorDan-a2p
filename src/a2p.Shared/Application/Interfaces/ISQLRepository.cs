using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.DTO;

namespace a2p.Shared.Application.Interfaces
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
        Task<A2PError?> DeleteSalesDocumentDataAsync(int number, int version, bool DeleteExisting);
        //============================================================================================================================
        Task<A2PError?> InsertPrefSuiteColorAsync(MaterialDTO materialDTO);
        Task<A2PError?> InsertPrefSuiteColorConfigurationAsync(MaterialDTO materialDTO);
        Task<A2PError?> InsertPrefSuiteMaterialBaseAsync(MaterialDTO materialDTO);
        Task<A2PError?> InsertPrefSuiteMaterialAsync(MaterialDTO materialDTO);
        //============================================================================================================================
        Task<A2PError?> InsertPrefSuiteMaterialProfileAsync(MaterialDTO materialDTO);
        Task<A2PError?> InsertPrefSuiteMaterialMeterAsync(MaterialDTO materialDTO);
        Task<A2PError?> InsertPrefSuiteMaterialPieceAsync(MaterialDTO materialDTO);
        Task<A2PError?> InsertPrefSuiteMaterialSurfaceAsync(MaterialDTO materialDTO);
        Task<A2PError?> UpdateBCMapping(MaterialDTO materialDTO);
        Task<A2PError?> InsertPrefSuiteMaterialPurchaseDataAsync(MaterialDTO materialDTO);
        Task<A2PError?> InsertOrderMaterialDTOAsync(MaterialDTO materialDTO, int number, int version);
        //============================================================================================================================
        Task<A2PError?> InsertOrderItemDTOAsync(ItemDTO itemDTO, int number, int version, string idPos);
        Task<A2PError?> InsertPrefSuiteMaterialNeedsMasterAsync(string order, int number, int version);
        Task<A2PError?> InsertPrefSuiteMaterialNeedsAsync(string order, int number, int version);

    }
}
