using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Models;

using Domain.Entities;

using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.PrefSuiteServices
{
    public class PrefSuiteDataService : IPrefSuiteDataService
    {
        private readonly IPrefSuiteRepository _repository;
        private readonly ILogger<PrefSuiteDataService> _logger;

        public PrefSuiteDataService(IPrefSuiteRepository repository, ILogger<PrefSuiteDataService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SalesDocument?> GetSalesDocumentByOrderNumberAsync(string orderNumber)
        => await _repository.GetSalesDocumentByOrderAsync(orderNumber);

        public async Task<SalesDocument?> GetSalesDocumentByRowIdAsync(Guid rowId)
        => await _repository.GetSalesDocumentByRowIdAsync(rowId);

        public async Task<string?> GetGlassReferenceAsync(string description)
        => await _repository.GetGlassReferenceAsync(description);

        public async Task<int?> GetPrefSuiteColorConfigurationAsync(string color)
        => await _repository.GetPrefSuiteColorConfigurationAsync(color);

        public async Task<int?> GetCommodityCode(string sourceReference)
        => await _repository.GetCommodityCode(sourceReference);

        public async Task<decimal?> GetTechDesignWeight(string sourceReference)
        => await _repository.GetTechDesignWeight(sourceReference);

        public async Task<string?> GetSapaColorAsync(string color)
        => await _repository.GetSapaColorAsync(color);

        public async Task DeleteSalesDocumentDataAsync(int number, int version, bool deleteExisting)
        => await _repository.DeleteSalesDocumentDataAsync(number, version, deleteExisting);

        public async Task InsertPrefSuiteColorAsync(MaterialEntity material)
        => await _repository.InsertPrefSuiteColorAsync(material);

        public async Task InsertPrefSuiteColorConfigurationAsync(MaterialEntity material)
        => await _repository.InsertPrefSuiteColorConfigurationAsync(material);

        public async Task InsertPrefSuiteMaterialBaseAsync(MaterialEntity material)
        => await _repository.InsertPrefSuiteMaterialBaseAsync(material);

        public async Task InsertPrefSuiteMaterialAsync(MaterialEntity material)
        => await _repository.InsertPrefSuiteMaterialAsync(material);

        public async Task InsertPrefSuiteMaterialProfileAsync(MaterialEntity material)
        => await _repository.InsertPrefSuiteMaterialProfileAsync(material);

        public async Task InsertPrefSuiteMaterialMeterAsync(MaterialEntity material)
        => await _repository.InsertPrefSuiteMaterialMeterAsync(material);

        public async Task InsertPrefSuiteMaterialPieceAsync(MaterialEntity material)
        => await _repository.InsertPrefSuiteMaterialPieceAsync(material);

        public async Task InsertPrefSuiteMaterialSurfaceAsync(MaterialEntity material)
        => await _repository.InsertPrefSuiteMaterialSurfaceAsync(material);

        public async Task InsertPrefSuiteMaterialPurchaseDataAsync(MaterialEntity material)
        => await _repository.InsertPrefSuiteMaterialPurchaseDataAsync(material);

        public async Task UpdateBCMapping(MaterialEntity material)
        => await _repository.UpdateBCMapping(material);

        public async Task InsertPrefSuiteMaterialNeedsMasterAsync(Guid? orderId)
        => await _repository.InsertPrefSuiteMaterialNeedsMasterAsync(orderId);

        public async Task InsertPrefSuiteMaterialNeedsAsync(Guid? orderId)
        => await _repository.InsertPrefSuiteMaterialNeedsAsync(orderId);
    }
}
