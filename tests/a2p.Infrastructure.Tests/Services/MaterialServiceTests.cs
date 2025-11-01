using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

using AutoMapper;

using Domain.Entities;
using Domain.Enums;
using Domain.Shared;

using FluentValidation;
using FluentValidation.Results;

using Infrastructure.Persistence.Services;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Infrastructure.Tests.Services
{
    public class MaterialServiceTests
    {
        private readonly Mock<IMaterialRepository> _mockRepository;
        private readonly Mock<IValidator<MaterialDto>> _mockValidator;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<MaterialService>> _mockLogger;
        private readonly IMaterialService _materialService;

        public MaterialServiceTests()
        {
            _mockRepository = new Mock<IMaterialRepository>();
            _mockValidator = new Mock<IValidator<MaterialDto>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<MaterialService>>();

            _materialService = new MaterialService(
            _mockRepository.Object,
            _mockValidator.Object,
            _mockMapper.Object,
            _mockLogger.Object);
        }

        #region CreateMaterialAsync Tests

        [Fact]
        public async Task CreateMaterialAsync_WithValidDto_ReturnsSuccessResult()
        {
            // Arrange
            MaterialDto materialDto = CreateValidMaterialDto();
            MaterialEntity materialEntity = CreateValidMaterialEntity();

            _mockValidator
            .Setup(v => v.ValidateAsync(materialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<MaterialEntity>(materialDto))
            .Returns(materialEntity);

            _mockRepository
            .Setup(r => r.CreateMaterialAsync(It.IsAny<MaterialEntity>()))
            .ReturnsAsync(materialEntity);

            // Act
            ValidationResult<MaterialEntity> result = await _materialService.CreateMaterialAsync(materialDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("PROFILE-001", result.Value.Reference);
            _mockRepository.Verify(r => r.CreateMaterialAsync(It.IsAny<MaterialEntity>()), Times.Once);
        }

        [Fact]
        public async Task CreateMaterialAsync_WithInvalidDto_ReturnsFailureResult()
        {
            // Arrange
            MaterialDto materialDto = CreateInvalidMaterialDto();
            ValidationFailure validationFailure = new("Reference", "Reference is required");

            _mockValidator
            .Setup(v => v.ValidateAsync(materialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { validationFailure }));

            // Act
            ValidationResult<MaterialEntity> result = await _materialService.CreateMaterialAsync(materialDto);

            // Assert
            Assert.False(result.IsSuccess);
            _mockRepository.Verify(r => r.CreateMaterialAsync(It.IsAny<MaterialEntity>()), Times.Never);
        }

        [Fact]
        public async Task CreateMaterialAsync_WhenRepositoryReturnsNull_ReturnsFailureResult()
        {
            // Arrange
            MaterialDto materialDto = CreateValidMaterialDto();
            MaterialEntity materialEntity = CreateValidMaterialEntity();

            _mockValidator
            .Setup(v => v.ValidateAsync(materialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<MaterialEntity>(materialDto))
            .Returns(materialEntity);

            _mockRepository
            .Setup(r => r.CreateMaterialAsync(It.IsAny<MaterialEntity>()))
            .ReturnsAsync((MaterialEntity?)null);

            // Act
            ValidationResult<MaterialEntity> result = await _materialService.CreateMaterialAsync(materialDto);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateMaterialAsync_WhenMapperReturnsNull_ThrowsInvalidOperationException()
        {
            // Arrange
            MaterialDto materialDto = CreateValidMaterialDto();

            _mockValidator
            .Setup(v => v.ValidateAsync(materialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<MaterialEntity>(materialDto))
            .Returns((MaterialEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
            () => _materialService.CreateMaterialAsync(materialDto));
        }

        [Fact]
        public async Task CreateMaterialAsync_WhenSqlExceptionThrown_ReturnsDatabaseError()
        {
            // Arrange
            MaterialDto materialDto = CreateValidMaterialDto();
            MaterialEntity materialEntity = CreateValidMaterialEntity();

            _mockValidator
            .Setup(v => v.ValidateAsync(materialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<MaterialEntity>(materialDto))
            .Returns(materialEntity);

            _mockRepository
            .Setup(r => r.CreateMaterialAsync(It.IsAny<MaterialEntity>()))
            .ThrowsAsync(new InvalidOperationException("SQL Error"));

            // Act
            ValidationResult<MaterialEntity> result = await _materialService.CreateMaterialAsync(materialDto);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region UpdateMaterialAsync Tests

        [Fact]
        public async Task UpdateMaterialAsync_WithValidDto_ReturnsSuccessResult()
        {
            // Arrange
            MaterialDto materialDto = CreateValidMaterialDto();
            MaterialEntity materialEntity = CreateValidMaterialEntity();
            MaterialEntity existingMaterial = CreateValidMaterialEntity();

            _mockValidator
            .Setup(v => v.ValidateAsync(materialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<MaterialEntity>(materialDto))
            .Returns(materialEntity);

            _mockRepository
            .Setup(r => r.GetMaterialAsync(materialEntity.Id))
            .ReturnsAsync(existingMaterial);

            _mockRepository
            .Setup(r => r.UpdateMaterialAsync(It.IsAny<MaterialEntity>()))
            .ReturnsAsync(1);

            // Act
            ValidationResult<MaterialEntity> result = await _materialService.UpdateMaterialAsync(materialDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            _mockRepository.Verify(r => r.UpdateMaterialAsync(It.IsAny<MaterialEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMaterialAsync_WithNonExistentId_ReturnsFailureResult()
        {
            // Arrange
            MaterialDto materialDto = CreateValidMaterialDto();
            MaterialEntity materialEntity = CreateValidMaterialEntity();

            _mockValidator
            .Setup(v => v.ValidateAsync(materialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<MaterialEntity>(materialDto))
            .Returns(materialEntity);

            _mockRepository
            .Setup(r => r.GetMaterialAsync(materialEntity.Id))
            .ReturnsAsync((MaterialEntity?)null);

            // Act
            ValidationResult<MaterialEntity> result = await _materialService.UpdateMaterialAsync(materialDto);

            // Assert
            Assert.False(result.IsSuccess);
            _mockRepository.Verify(r => r.UpdateMaterialAsync(It.IsAny<MaterialEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateMaterialAsync_WithInvalidDto_ReturnsFailureResult()
        {
            // Arrange
            MaterialDto materialDto = CreateInvalidMaterialDto();
            ValidationFailure validationFailure = new("Reference", "Reference is required");

            _mockValidator
            .Setup(v => v.ValidateAsync(materialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { validationFailure }));

            // Act
            ValidationResult<MaterialEntity> result = await _materialService.UpdateMaterialAsync(materialDto);

            // Assert
            Assert.False(result.IsSuccess);
            _mockRepository.Verify(r => r.UpdateMaterialAsync(It.IsAny<MaterialEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateMaterialAsync_WhenRepositoryUpdateFails_ReturnsFailureResult()
        {
            // Arrange
            MaterialDto materialDto = CreateValidMaterialDto();
            MaterialEntity materialEntity = CreateValidMaterialEntity();
            MaterialEntity existingMaterial = CreateValidMaterialEntity();

            _mockValidator
            .Setup(v => v.ValidateAsync(materialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<MaterialEntity>(materialDto))
            .Returns(materialEntity);

            _mockRepository
            .Setup(r => r.GetMaterialAsync(materialEntity.Id))
            .ReturnsAsync(existingMaterial);

            _mockRepository
            .Setup(r => r.UpdateMaterialAsync(It.IsAny<MaterialEntity>()))
            .ReturnsAsync(0);

            // Act
            ValidationResult<MaterialEntity> result = await _materialService.UpdateMaterialAsync(materialDto);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region GetMaterialByIdAsync Tests

        [Fact]
        public async Task GetMaterialByIdAsync_WithValidId_ReturnsSuccessResult()
        {
            // Arrange
            Guid materialId = Guid.NewGuid();
            MaterialEntity materialEntity = CreateValidMaterialEntity();
            materialEntity.Id = materialId;

            _mockRepository
            .Setup(r => r.GetMaterialAsync(materialId))
            .ReturnsAsync(materialEntity);

            // Act
            Result<MaterialEntity> result = await _materialService.GetMaterialByIdAsync(materialId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(materialId, result.Value.Id);
        }

        [Fact]
        public async Task GetMaterialByIdAsync_WithNonExistentId_ReturnsFailureResult()
        {
            // Arrange
            Guid materialId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetMaterialAsync(materialId))
            .ReturnsAsync((MaterialEntity?)null);

            // Act
            Result<MaterialEntity> result = await _materialService.GetMaterialByIdAsync(materialId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetMaterialByIdAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            Guid materialId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetMaterialAsync(materialId))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            Result<MaterialEntity> result = await _materialService.GetMaterialByIdAsync(materialId);

            // Assert
            Assert.False(result.IsSuccess);
            _mockLogger.Verify(
            x => x.Log(
             Microsoft.Extensions.Logging.LogLevel.Error,
             It.IsAny<EventId>(),
             It.IsAny<It.IsAnyType>(),
             It.IsAny<Exception>(),
             It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }

        #endregion

        #region GetOrderMaterialsAsync Tests

        [Fact]
        public async Task GetOrderMaterialsAsync_WithValidOrderId_ReturnsSuccessResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            List<MaterialEntity> materials = new()
 {
 CreateValidMaterialEntity(),
 CreateValidMaterialEntity()
 };

            _mockRepository
            .Setup(r => r.GetOrderMaterialsAsync(orderId))
            .ReturnsAsync(materials);

            // Act
            Result<IEnumerable<MaterialEntity>?> result = await _materialService.GetOrderMaterialsAsync(orderId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count());
        }

        [Fact]
        public async Task GetOrderMaterialsAsync_WithNonExistentOrderId_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetOrderMaterialsAsync(orderId))
            .ReturnsAsync((IEnumerable<MaterialEntity>?)null);

            // Act
            Result<IEnumerable<MaterialEntity>?> result = await _materialService.GetOrderMaterialsAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetOrderMaterialsAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetOrderMaterialsAsync(orderId))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            Result<IEnumerable<MaterialEntity>?> result = await _materialService.GetOrderMaterialsAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region DeleteMaterialAsync Tests

        [Fact]
        public async Task DeleteMaterialAsync_WithValidId_ReturnsSuccessResult()
        {
            // Arrange
            Guid materialId = Guid.NewGuid();
            MaterialEntity materialEntity = CreateValidMaterialEntity();
            materialEntity.Id = materialId;

            _mockRepository
            .Setup(r => r.GetMaterialAsync(materialId))
            .ReturnsAsync(materialEntity);

            _mockRepository
            .Setup(r => r.DeleteMaterialsdAsync(materialId))
            .ReturnsAsync(1);

            // Act
            Result<bool> result = await _materialService.DeleteMaterialAsync(materialId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            _mockRepository.Verify(r => r.DeleteMaterialsdAsync(materialId), Times.Once);
        }

        [Fact]
        public async Task DeleteMaterialAsync_WithNonExistentId_ReturnsFailureResult()
        {
            // Arrange
            Guid materialId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetMaterialAsync(materialId))
            .ReturnsAsync((MaterialEntity?)null);

            // Act
            Result<bool> result = await _materialService.DeleteMaterialAsync(materialId);

            // Assert
            Assert.False(result.IsSuccess);
            _mockRepository.Verify(r => r.DeleteMaterialsdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task DeleteMaterialAsync_WhenRepositoryDeleteFails_ReturnsFailureResult()
        {
            // Arrange
            Guid materialId = Guid.NewGuid();
            MaterialEntity materialEntity = CreateValidMaterialEntity();
            materialEntity.Id = materialId;

            _mockRepository
            .Setup(r => r.GetMaterialAsync(materialId))
            .ReturnsAsync(materialEntity);

            _mockRepository
            .Setup(r => r.DeleteMaterialsdAsync(materialId))
            .ReturnsAsync(0);

            // Act
            Result<bool> result = await _materialService.DeleteMaterialAsync(materialId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteMaterialAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            Guid materialId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetMaterialAsync(materialId))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            Result<bool> result = await _materialService.DeleteMaterialAsync(materialId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region DeleteOrderMaterialAsync Tests

        [Fact]
        public async Task DeleteOrderMaterialAsync_WithValidOrderId_ReturnsSuccessResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            List<MaterialEntity> materials = new()
 { CreateValidMaterialEntity() };

            _mockRepository
            .Setup(r => r.GetOrderMaterialsAsync(orderId))
            .ReturnsAsync(materials);

            _mockRepository
            .Setup(r => r.DeleteOrderMaterialsAsync(orderId))
            .ReturnsAsync(1);

            // Act
            Result<bool> result = await _materialService.DeleteOrderMaterialAsync(orderId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            _mockRepository.Verify(r => r.DeleteOrderMaterialsAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderMaterialAsync_WithNonExistentOrderId_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetOrderMaterialsAsync(orderId))
            .ReturnsAsync((IEnumerable<MaterialEntity>?)null);

            // Act
            Result<bool> result = await _materialService.DeleteOrderMaterialAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
            _mockRepository.Verify(r => r.DeleteOrderMaterialsAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOrderMaterialAsync_WhenRepositoryDeleteFails_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            List<MaterialEntity> materials = new()
 { CreateValidMaterialEntity() };

            _mockRepository
            .Setup(r => r.GetOrderMaterialsAsync(orderId))
            .ReturnsAsync(materials);

            _mockRepository
            .Setup(r => r.DeleteOrderMaterialsAsync(orderId))
            .ReturnsAsync(0);

            // Act
            Result<bool> result = await _materialService.DeleteOrderMaterialAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteOrderMaterialAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetOrderMaterialsAsync(orderId))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            Result<bool> result = await _materialService.DeleteOrderMaterialAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region Helper Methods

        private MaterialDto CreateValidMaterialDto()
        {
            return new MaterialDto
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                Reference = "PROFILE-001",
                ReferenceBase = "PROFILE",
                Description = "Aluminum Profile",
                Color = "Silver",
                ColorDescription = "Silver Anodized",
                Worksheet = "Sheet1",
                Line = 1,
                Column = 1,
                ItemName = "Aluminum Profile 45x45",
                SortOrder = 1,
                Width = 45m,
                Height = 45m,
                Quantity = 10,
                RequiredQuantity = 10,
                PackageQuantity = 5,
                TotalQuantity = 10,
                Weight = 2.5m,
                RequiredWeight = 25m,
                TotalWeight = 25m,
                Area = 0.2025m,
                TotalArea = 2.025m,
                RequiredArea = 2.025m,
                Waste = 0.05m,
                Price = 50m,
                TotalPrice = 500m,
                RequiredPrice = 500m,
                SquareMeterPrice = 246.91m,
                Pallet = "PAL-001",
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials,
                SourceReference = "SRC-001",
                SourceDescription = "Schuco Profile",
                SourceColor = "Silver",
                SourceColorDescription = "Silver Anodized",
                CommodityCode = 7610
            };
        }

        private MaterialDto CreateInvalidMaterialDto()
        {
            return new MaterialDto
            {
                Id = Guid.NewGuid(),
                Reference = null, // Invalid: required field
                Quantity = 0, // Invalid: must be > 0
                RequiredQuantity = 0 // Invalid: must be > 0
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
                Worksheet = "Sheet1",
                Line = 1,
                Column = 1,
                ItemName = "Aluminum Profile 45x45",
                SortOrder = 1,
                Width = 45m,
                Height = 45m,
                Quantity = 10,
                RequiredQuantity = 10,
                PackageQuantity = 5,
                TotalQuantity = 10,
                Weight = 2.5m,
                RequiredWeight = 25m,
                TotalWeight = 25m,
                Area = 0.2025m,
                TotalArea = 2.025m,
                RequiredArea = 2.025m,
                Waste = 0.05m,
                Price = 50m,
                TotalPrice = 500m,
                RequiredPrice = 500m,
                SquareMeterPrice = 246.91m,
                Pallet = "PAL-001",
                MaterialType = MaterialType.Profiles,
                CreatedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now
            };
        }

        #endregion
    }
}
