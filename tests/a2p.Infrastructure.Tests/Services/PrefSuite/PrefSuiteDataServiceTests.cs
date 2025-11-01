using Application.Interfaces.Repositories;
using Application.Models;

using Domain.Entities;

using Infrastructure.Services.PrefSuiteServices;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Infrastructure.Tests.Services.PrefSuite
{
    /// <summary>
    /// Unit tests for PrefSuiteDataService
    /// Tests all data query and insert methods with various scenarios
    /// </summary>
    public class PrefSuiteDataServiceTests
    {
        private readonly Mock<IPrefSuiteRepository> _mockRepository;
        private readonly Mock<ILogger<PrefSuiteDataService>> _mockLogger;
        private readonly PrefSuiteDataService _prefSuiteDataService;

        public PrefSuiteDataServiceTests()
        {
            _mockRepository = new Mock<IPrefSuiteRepository>();
            _mockLogger = new Mock<ILogger<PrefSuiteDataService>>();

            _prefSuiteDataService = new PrefSuiteDataService(
            _mockRepository.Object,
            _mockLogger.Object);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithNullRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            new PrefSuiteDataService(null!, _mockLogger.Object));
        }

        [Fact]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            new PrefSuiteDataService(_mockRepository.Object, null!));
        }

        #endregion

        #region GetSalesDocumentByOrderNumberAsync Tests

        [Fact]
        public async Task GetSalesDocumentByOrderNumberAsync_WithValidOrderNumber_ReturnsSalesDocument()
        {
            // Arrange
            string orderNumber = "ORD-2024-001";
            SalesDocument salesDocument = CreateValidSalesDocument();

            _mockRepository
            .Setup(r => r.GetSalesDocumentByOrderAsync(orderNumber))
            .ReturnsAsync(salesDocument);

            // Act
            SalesDocument? result = await _prefSuiteDataService.GetSalesDocumentByOrderNumberAsync(orderNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(12345, result.Number);
            _mockRepository.Verify(r => r.GetSalesDocumentByOrderAsync(orderNumber), Times.Once);
        }

        [Fact]
        public async Task GetSalesDocumentByOrderNumberAsync_WithNonExistentOrderNumber_ReturnsNull()
        {
            // Arrange
            string orderNumber = "NONEXISTENT";

            _mockRepository
            .Setup(r => r.GetSalesDocumentByOrderAsync(orderNumber))
            .ReturnsAsync((SalesDocument?)null);

            // Act
            SalesDocument? result = await _prefSuiteDataService.GetSalesDocumentByOrderNumberAsync(orderNumber);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetSalesDocumentByRowIdAsync Tests

        [Fact]
        public async Task GetSalesDocumentByRowIdAsync_WithValidRowId_ReturnsSalesDocument()
        {
            // Arrange
            Guid rowId = Guid.NewGuid();
            SalesDocument salesDocument = CreateValidSalesDocument();

            _mockRepository
            .Setup(r => r.GetSalesDocumentByRowIdAsync(rowId))
            .ReturnsAsync(salesDocument);

            // Act
            SalesDocument? result = await _prefSuiteDataService.GetSalesDocumentByRowIdAsync(rowId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(12345, result.Number);
        }

        [Fact]
        public async Task GetSalesDocumentByRowIdAsync_WithNonExistentRowId_ReturnsNull()
        {
            // Arrange
            Guid rowId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetSalesDocumentByRowIdAsync(rowId))
            .ReturnsAsync((SalesDocument?)null);

            // Act
            SalesDocument? result = await _prefSuiteDataService.GetSalesDocumentByRowIdAsync(rowId);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetGlassReferenceAsync Tests

        [Fact]
        public async Task GetGlassReferenceAsync_WithValidDescription_ReturnsReference()
        {
            // Arrange
            string description = "Clear Float Glass";
            string reference = "GLASS-CLEAR-001";

            _mockRepository
            .Setup(r => r.GetGlassReferenceAsync(description))
            .ReturnsAsync(reference);

            // Act
            string? result = await _prefSuiteDataService.GetGlassReferenceAsync(description);

            // Assert
            Assert.Equal(reference, result);
        }

        [Fact]
        public async Task GetGlassReferenceAsync_WithNonExistentDescription_ReturnsNull()
        {
            // Arrange
            string description = "Unknown Glass";

            _mockRepository
            .Setup(r => r.GetGlassReferenceAsync(description))
            .ReturnsAsync((string?)null);

            // Act
            string? result = await _prefSuiteDataService.GetGlassReferenceAsync(description);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetPrefSuiteColorConfigurationAsync Tests

        [Fact]
        public async Task GetPrefSuiteColorConfigurationAsync_WithValidColor_ReturnsConfiguration()
        {
            // Arrange
            string color = "Silver";
            int configuration = 5;

            _mockRepository
            .Setup(r => r.GetPrefSuiteColorConfigurationAsync(color))
            .ReturnsAsync(configuration);

            // Act
            int? result = await _prefSuiteDataService.GetPrefSuiteColorConfigurationAsync(color);

            // Assert
            Assert.Equal(configuration, result);
        }

        [Fact]
        public async Task GetPrefSuiteColorConfigurationAsync_WithNonExistentColor_ReturnsNull()
        {
            // Arrange
            string color = "UnknownColor";

            _mockRepository
            .Setup(r => r.GetPrefSuiteColorConfigurationAsync(color))
            .ReturnsAsync((int?)null);

            // Act
            int? result = await _prefSuiteDataService.GetPrefSuiteColorConfigurationAsync(color);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetCommodityCodeAsync Tests

        [Fact]
        public async Task GetCommodityCode_WithValidSourceReference_ReturnsCommodityCode()
        {
            // Arrange
            string sourceReference = "PROF-45x45";
            int commodityCode = 7610;

            _mockRepository
            .Setup(r => r.GetCommodityCode(sourceReference))
            .ReturnsAsync(commodityCode);

            // Act
            int? result = await _prefSuiteDataService.GetCommodityCode(sourceReference);

            // Assert
            Assert.Equal(commodityCode, result);
        }

        [Fact]
        public async Task GetCommodityCode_WithNonExistentReference_ReturnsNull()
        {
            // Arrange
            string sourceReference = "NONEXISTENT";

            _mockRepository
            .Setup(r => r.GetCommodityCode(sourceReference))
            .ReturnsAsync((int?)null);

            // Act
            int? result = await _prefSuiteDataService.GetCommodityCode(sourceReference);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetTechDesignWeightAsync Tests

        [Fact]
        public async Task GetTechDesignWeight_WithValidSourceReference_ReturnsWeight()
        {
            // Arrange
            string sourceReference = "PROF-45x45";
            decimal weight = 2.5m;

            _mockRepository
            .Setup(r => r.GetTechDesignWeight(sourceReference))
            .ReturnsAsync(weight);

            // Act
            decimal? result = await _prefSuiteDataService.GetTechDesignWeight(sourceReference);

            // Assert
            Assert.Equal(weight, result);
        }

        [Fact]
        public async Task GetTechDesignWeight_WithNonExistentReference_ReturnsNull()
        {
            // Arrange
            string sourceReference = "NONEXISTENT";

            _mockRepository
            .Setup(r => r.GetTechDesignWeight(sourceReference))
            .ReturnsAsync((decimal?)null);

            // Act
            decimal? result = await _prefSuiteDataService.GetTechDesignWeight(sourceReference);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetSapaColorAsync Tests

        [Fact]
        public async Task GetSapaColorAsync_WithValidColor_ReturnsColor()
        {
            // Arrange
            string color = "Silver";
            string sapaColor = "SAPA-SILVER-001";

            _mockRepository
            .Setup(r => r.GetSapaColorAsync(color))
            .ReturnsAsync(sapaColor);

            // Act
            string? result = await _prefSuiteDataService.GetSapaColorAsync(color);

            // Assert
            Assert.Equal(sapaColor, result);
        }

        [Fact]
        public async Task GetSapaColorAsync_WithNonExistentColor_ReturnsNull()
        {
            // Arrange
            string color = "UnknownColor";

            _mockRepository
            .Setup(r => r.GetSapaColorAsync(color))
            .ReturnsAsync((string?)null);

            // Act
            string? result = await _prefSuiteDataService.GetSapaColorAsync(color);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region DeleteSalesDocumentDataAsync Tests

        [Fact]
        public async Task DeleteSalesDocumentDataAsync_WithValidParameters_CompletesSuccessfully()
        {
            // Arrange
            int number = 12345;
            int version = 1;
            bool deleteExisting = true;

            _mockRepository
            .Setup(r => r.DeleteSalesDocumentDataAsync(number, version, deleteExisting))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.DeleteSalesDocumentDataAsync(number, version, deleteExisting);

            // Assert
            _mockRepository.Verify(r => r.DeleteSalesDocumentDataAsync(number, version, deleteExisting), Times.Once);
        }

        #endregion

        #region InsertPrefSuiteMaterialAsync Tests

        [Fact]
        public async Task InsertPrefSuiteColorAsync_WithValidMaterial_CompletesSuccessfully()
        {
            // Arrange
            MaterialEntity material = CreateValidMaterialEntity();

            _mockRepository
            .Setup(r => r.InsertPrefSuiteColorAsync(material))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteColorAsync(material);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteColorAsync(material), Times.Once);
        }

        [Fact]
        public async Task InsertPrefSuiteColorConfigurationAsync_WithValidMaterial_CompletesSuccessfully()
        {
            // Arrange
            MaterialEntity material = CreateValidMaterialEntity();

            _mockRepository
            .Setup(r => r.InsertPrefSuiteColorConfigurationAsync(material))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteColorConfigurationAsync(material);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteColorConfigurationAsync(material), Times.Once);
        }

        [Fact]
        public async Task InsertPrefSuiteMaterialBaseAsync_WithValidMaterial_CompletesSuccessfully()
        {
            // Arrange
            MaterialEntity material = CreateValidMaterialEntity();

            _mockRepository
            .Setup(r => r.InsertPrefSuiteMaterialBaseAsync(material))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteMaterialBaseAsync(material);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteMaterialBaseAsync(material), Times.Once);
        }

        [Fact]
        public async Task InsertPrefSuiteMaterialAsync_WithValidMaterial_CompletesSuccessfully()
        {
            // Arrange
            MaterialEntity material = CreateValidMaterialEntity();

            _mockRepository
            .Setup(r => r.InsertPrefSuiteMaterialAsync(material))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteMaterialAsync(material);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteMaterialAsync(material), Times.Once);
        }

        [Fact]
        public async Task InsertPrefSuiteMaterialProfileAsync_WithValidMaterial_CompletesSuccessfully()
        {
            // Arrange
            MaterialEntity material = CreateValidMaterialEntity();

            _mockRepository
            .Setup(r => r.InsertPrefSuiteMaterialProfileAsync(material))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteMaterialProfileAsync(material);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteMaterialProfileAsync(material), Times.Once);
        }

        [Fact]
        public async Task InsertPrefSuiteMaterialMeterAsync_WithValidMaterial_CompletesSuccessfully()
        {
            // Arrange
            MaterialEntity material = CreateValidMaterialEntity();

            _mockRepository
            .Setup(r => r.InsertPrefSuiteMaterialMeterAsync(material))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteMaterialMeterAsync(material);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteMaterialMeterAsync(material), Times.Once);
        }

        [Fact]
        public async Task InsertPrefSuiteMaterialPieceAsync_WithValidMaterial_CompletesSuccessfully()
        {
            // Arrange
            MaterialEntity material = CreateValidMaterialEntity();

            _mockRepository
            .Setup(r => r.InsertPrefSuiteMaterialPieceAsync(material))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteMaterialPieceAsync(material);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteMaterialPieceAsync(material), Times.Once);
        }

        [Fact]
        public async Task InsertPrefSuiteMaterialSurfaceAsync_WithValidMaterial_CompletesSuccessfully()
        {
            // Arrange
            MaterialEntity material = CreateValidMaterialEntity();

            _mockRepository
            .Setup(r => r.InsertPrefSuiteMaterialSurfaceAsync(material))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteMaterialSurfaceAsync(material);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteMaterialSurfaceAsync(material), Times.Once);
        }

        [Fact]
        public async Task InsertPrefSuiteMaterialPurchaseDataAsync_WithValidMaterial_CompletesSuccessfully()
        {
            // Arrange
            MaterialEntity material = CreateValidMaterialEntity();

            _mockRepository
            .Setup(r => r.InsertPrefSuiteMaterialPurchaseDataAsync(material))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteMaterialPurchaseDataAsync(material);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteMaterialPurchaseDataAsync(material), Times.Once);
        }

        #endregion

        #region UpdateBCMapping Tests

        [Fact]
        public async Task UpdateBCMapping_WithValidMaterial_CompletesSuccessfully()
        {
            // Arrange
            MaterialEntity material = CreateValidMaterialEntity();

            _mockRepository
            .Setup(r => r.UpdateBCMapping(material))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.UpdateBCMapping(material);

            // Assert
            _mockRepository.Verify(r => r.UpdateBCMapping(material), Times.Once);
        }

        #endregion

        #region InsertPrefSuiteMaterialNeedsAsync Tests

        [Fact]
        public async Task InsertPrefSuiteMaterialNeedsMasterAsync_WithValidOrderId_CompletesSuccessfully()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.InsertPrefSuiteMaterialNeedsMasterAsync(orderId))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteMaterialNeedsMasterAsync(orderId);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteMaterialNeedsMasterAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task InsertPrefSuiteMaterialNeedsMasterAsync_WithNullOrderId_CompletesSuccessfully()
        {
            // Arrange
            Guid? orderId = null;

            _mockRepository
            .Setup(r => r.InsertPrefSuiteMaterialNeedsMasterAsync(orderId))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteMaterialNeedsMasterAsync(orderId);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteMaterialNeedsMasterAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task InsertPrefSuiteMaterialNeedsAsync_WithValidOrderId_CompletesSuccessfully()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.InsertPrefSuiteMaterialNeedsAsync(orderId))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteMaterialNeedsAsync(orderId);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteMaterialNeedsAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task InsertPrefSuiteMaterialNeedsAsync_WithNullOrderId_CompletesSuccessfully()
        {
            // Arrange
            Guid? orderId = null;

            _mockRepository
            .Setup(r => r.InsertPrefSuiteMaterialNeedsAsync(orderId))
            .Returns(Task.CompletedTask);

            // Act
            await _prefSuiteDataService.InsertPrefSuiteMaterialNeedsAsync(orderId);

            // Assert
            _mockRepository.Verify(r => r.InsertPrefSuiteMaterialNeedsAsync(orderId), Times.Once);
        }

        #endregion

        #region Helper Methods

        private SalesDocument CreateValidSalesDocument()
        {
            return new SalesDocument
            {
                Number = 12345,
                Version = 1,
                RowId = Guid.NewGuid()
            };
        }

        private MaterialEntity CreateValidMaterialEntity()
        {
            return new MaterialEntity
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                Reference = "PROFILE-001",
                ReferenceBase = "PROFILE",
                Description = "Aluminum Profile",
                Color = "Silver",
                ColorDescription = "Silver Anodized",
                Width = 45m,
                Height = 45m,
                Quantity = 10,
                RequiredQuantity = 10,
                Weight = 2.5m,
                TotalWeight = 25m,
                Price = 50m,
                TotalPrice = 500m,
                CreatedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now
            };
        }

        #endregion
    }
}
