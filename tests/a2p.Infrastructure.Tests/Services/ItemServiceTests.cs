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
    public class ItemServiceTests
    {
        private readonly Mock<IItemRepository> _mockRepository;
        private readonly Mock<IValidator<ItemDto>> _mockValidator;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<ItemService>> _mockLogger;
        private readonly IItemService _itemService;

        public ItemServiceTests()
        {
            _mockRepository = new Mock<IItemRepository>();
            _mockValidator = new Mock<IValidator<ItemDto>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<ItemService>>();

            _itemService = new ItemService(
            _mockRepository.Object,
            _mockValidator.Object,
            _mockMapper.Object,
            _mockLogger.Object);
        }

        #region CreateItemAsync Tests

        [Fact]
        public async Task CreateItemAsync_WithValidDto_ReturnsSuccessResult()
        {
            // Arrange
            ItemDto itemDto = CreateValidItemDto();
            ItemEntity itemEntity = CreateValidItemEntity();

            _mockValidator
            .Setup(v => v.ValidateAsync(itemDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<ItemEntity>(itemDto))
            .Returns(itemEntity);

            _mockRepository
            .Setup(r => r.CreateItemAsync(It.IsAny<ItemEntity>()))
            .ReturnsAsync(itemEntity);

            // Act
            ValidationResult<ItemEntity> result = await _itemService.CreateItemAsync(itemDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Test Item", result.Value.ItemName);
            _mockRepository.Verify(r => r.CreateItemAsync(It.IsAny<ItemEntity>()), Times.Once);
        }

        [Fact]
        public async Task CreateItemAsync_WithInvalidDto_ReturnsFailureResult()
        {
            // Arrange
            ItemDto itemDto = CreateInvalidItemDto();
            ValidationFailure validationFailure = new("ItemName", "ItemName is required");

            _mockValidator
            .Setup(v => v.ValidateAsync(itemDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { validationFailure }));

            // Act
            ValidationResult<ItemEntity> result = await _itemService.CreateItemAsync(itemDto);

            // Assert
            Assert.False(result.IsSuccess);
            _mockRepository.Verify(r => r.CreateItemAsync(It.IsAny<ItemEntity>()), Times.Never);
        }

        [Fact]
        public async Task CreateItemAsync_WhenRepositoryReturnsNull_ReturnsFailureResult()
        {
            // Arrange
            ItemDto itemDto = CreateValidItemDto();
            ItemEntity itemEntity = CreateValidItemEntity();

            _mockValidator
            .Setup(v => v.ValidateAsync(itemDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<ItemEntity>(itemDto))
            .Returns(itemEntity);

            _mockRepository
            .Setup(r => r.CreateItemAsync(It.IsAny<ItemEntity>()))
            .ReturnsAsync((ItemEntity?)null);

            // Act
            ValidationResult<ItemEntity> result = await _itemService.CreateItemAsync(itemDto);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateItemAsync_WhenMapperReturnsNull_ThrowsInvalidOperationException()
        {
            // Arrange
            ItemDto itemDto = CreateValidItemDto();

            _mockValidator
            .Setup(v => v.ValidateAsync(itemDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<ItemEntity>(itemDto))
            .Returns((ItemEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
            () => _itemService.CreateItemAsync(itemDto));
        }

        #endregion

        #region UpdateItemAsync Tests

        [Fact]
        public async Task UpdateItemAsync_WithValidDto_ReturnsSuccessResult()
        {
            // Arrange
            ItemDto itemDto = CreateValidItemDto();
            ItemEntity itemEntity = CreateValidItemEntity();
            ItemEntity existingItem = CreateValidItemEntity();

            _mockValidator
            .Setup(v => v.ValidateAsync(itemDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<ItemEntity>(itemDto))
            .Returns(itemEntity);

            _mockRepository
            .Setup(r => r.GetItemAsync(itemEntity.Id))
            .ReturnsAsync(existingItem);

            _mockRepository
            .Setup(r => r.UpdateItemAsync(It.IsAny<ItemEntity>()))
            .ReturnsAsync(1);

            // Act
            ValidationResult<ItemEntity> result = await _itemService.UpdateItemAsync(itemDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            _mockRepository.Verify(r => r.UpdateItemAsync(It.IsAny<ItemEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateItemAsync_WithNonExistentId_ReturnsFailureResult()
        {
            // Arrange
            ItemDto itemDto = CreateValidItemDto();
            ItemEntity itemEntity = CreateValidItemEntity();

            _mockValidator
            .Setup(v => v.ValidateAsync(itemDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<ItemEntity>(itemDto))
            .Returns(itemEntity);

            _mockRepository
            .Setup(r => r.GetItemAsync(itemEntity.Id))
            .ReturnsAsync((ItemEntity?)null);

            // Act
            ValidationResult<ItemEntity> result = await _itemService.UpdateItemAsync(itemDto);

            // Assert
            Assert.False(result.IsSuccess);
            _mockRepository.Verify(r => r.UpdateItemAsync(It.IsAny<ItemEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateItemAsync_WithInvalidDto_ReturnsFailureResult()
        {
            // Arrange
            ItemDto itemDto = CreateInvalidItemDto();
            ValidationFailure validationFailure = new("ItemName", "ItemName is required");

            _mockValidator
            .Setup(v => v.ValidateAsync(itemDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { validationFailure }));

            // Act
            ValidationResult<ItemEntity> result = await _itemService.UpdateItemAsync(itemDto);

            // Assert
            Assert.False(result.IsSuccess);
            _mockRepository.Verify(r => r.UpdateItemAsync(It.IsAny<ItemEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateItemAsync_WhenRepositoryUpdateFails_ReturnsFailureResult()
        {
            // Arrange
            ItemDto itemDto = CreateValidItemDto();
            ItemEntity itemEntity = CreateValidItemEntity();
            ItemEntity existingItem = CreateValidItemEntity();

            _mockValidator
            .Setup(v => v.ValidateAsync(itemDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<ItemEntity>(itemDto))
            .Returns(itemEntity);

            _mockRepository
            .Setup(r => r.GetItemAsync(itemEntity.Id))
            .ReturnsAsync(existingItem);

            _mockRepository
            .Setup(r => r.UpdateItemAsync(It.IsAny<ItemEntity>()))
            .ReturnsAsync(0);

            // Act
            ValidationResult<ItemEntity> result = await _itemService.UpdateItemAsync(itemDto);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region GetItemAsync Tests

        [Fact]
        public async Task GetItemAsync_WithValidId_ReturnsSuccessResult()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();
            ItemEntity itemEntity = CreateValidItemEntity();
            itemEntity.Id = itemId;

            _mockRepository
            .Setup(r => r.GetItemAsync(itemId))
            .ReturnsAsync(itemEntity);

            // Act
            Result<ItemEntity> result = await _itemService.GetItemAsync(itemId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(itemId, result.Value.Id);
        }

        [Fact]
        public async Task GetItemAsync_WithNonExistentId_ReturnsFailureResult()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetItemAsync(itemId))
            .ReturnsAsync((ItemEntity?)null);

            // Act
            Result<ItemEntity> result = await _itemService.GetItemAsync(itemId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetItemAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetItemAsync(itemId))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            Result<ItemEntity> result = await _itemService.GetItemAsync(itemId);

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

        #region GetOrderItemsAsync Tests

        [Fact]
        public async Task GetOrderItemsAsync_WithValidOrderId_ReturnsSuccessResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            List<ItemEntity> items = new()
 { CreateValidItemEntity(), CreateValidItemEntity() };

            _mockRepository
            .Setup(r => r.GetOrderItemsAsync(orderId))
            .ReturnsAsync(items);

            // Act
            Result<IEnumerable<ItemEntity>?> result = await _itemService.GetOrderItemsAsync(orderId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count());
        }

        [Fact]
        public async Task GetOrderItemsAsync_WithNonExistentOrderId_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetOrderItemsAsync(orderId))
            .ReturnsAsync(new List<ItemEntity>());

            // Act
            Result<IEnumerable<ItemEntity>?> result = await _itemService.GetOrderItemsAsync(orderId);

            // Assert
            // Note: Current implementation always returns Failure, even with empty list
            // This may need to be reviewed in the service implementation
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetOrderItemsAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetOrderItemsAsync(orderId))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            Result<IEnumerable<ItemEntity>?> result = await _itemService.GetOrderItemsAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region DeleteItemAsync Tests

        [Fact]
        public async Task DeleteItemAsync_WithValidId_ReturnsSuccessResult()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();
            ItemEntity itemEntity = CreateValidItemEntity();
            itemEntity.Id = itemId;

            _mockRepository
            .Setup(r => r.GetItemAsync(itemId))
            .ReturnsAsync(itemEntity);

            _mockRepository
            .Setup(r => r.DeleteItemAsync(itemId))
            .ReturnsAsync(1);

            // Act
            Result<bool> result = await _itemService.DeleteItemAsync(itemId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            _mockRepository.Verify(r => r.DeleteItemAsync(itemId), Times.Once);
        }

        [Fact]
        public async Task DeleteItemAsync_WithNonExistentId_ReturnsFailureResult()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetItemAsync(itemId))
            .ReturnsAsync((ItemEntity?)null);

            // Act
            Result<bool> result = await _itemService.DeleteItemAsync(itemId);

            // Assert
            Assert.False(result.IsSuccess);
            _mockRepository.Verify(r => r.DeleteItemAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task DeleteItemAsync_WhenRepositoryDeleteFails_ReturnsFailureResult()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();
            ItemEntity itemEntity = CreateValidItemEntity();
            itemEntity.Id = itemId;

            _mockRepository
            .Setup(r => r.GetItemAsync(itemId))
            .ReturnsAsync(itemEntity);

            _mockRepository
            .Setup(r => r.DeleteItemAsync(itemId))
            .ReturnsAsync(0);

            // Act
            Result<bool> result = await _itemService.DeleteItemAsync(itemId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteItemAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetItemAsync(itemId))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            Result<bool> result = await _itemService.DeleteItemAsync(itemId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region DeleteOrderItemsAsync Tests

        [Fact]
        public async Task DeleteOrderItemsAsync_WithValidOrderId_ReturnsSuccessResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            List<ItemEntity> items = new()
 { CreateValidItemEntity() };

            _mockRepository
            .Setup(r => r.GetOrderItemsAsync(orderId))
            .ReturnsAsync(items);

            _mockRepository
            .Setup(r => r.DeleteOrderItemsAsync(orderId))
            .ReturnsAsync(1);

            // Act
            Result<bool> result = await _itemService.DeleteOrderItemsAsync(orderId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            _mockRepository.Verify(r => r.DeleteOrderItemsAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderItemsAsync_WithNonExistentOrderId_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetOrderItemsAsync(orderId))
            .ReturnsAsync((IEnumerable<ItemEntity>?)null);

            // Act
            Result<bool> result = await _itemService.DeleteOrderItemsAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
            _mockRepository.Verify(r => r.DeleteOrderItemsAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOrderItemsAsync_WhenRepositoryDeleteFails_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            List<ItemEntity> items = new()
 { CreateValidItemEntity() };

            _mockRepository
            .Setup(r => r.GetOrderItemsAsync(orderId))
            .ReturnsAsync(items);

            _mockRepository
            .Setup(r => r.DeleteOrderItemsAsync(orderId))
            .ReturnsAsync(0);

            // Act
            Result<bool> result = await _itemService.DeleteOrderItemsAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteOrderItemsAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockRepository
            .Setup(r => r.GetOrderItemsAsync(orderId))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            Result<bool> result = await _itemService.DeleteOrderItemsAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region Helper Methods

        private ItemDto CreateValidItemDto()
        {
            return new ItemDto
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ItemName = "Test Item",
                Worksheet = "Sheet1",
                Line = 1,
                Column = 1,
                SortOrder = 1,
                Description = "Test Description",
                Quantity = 5,
                Width = 100m,
                Height = 200m,
                Weight = 10m,
                WeightWithoutGlass = 8m,
                WeightGlass = 2m,
                TotalWeight = 50m,
                TotalWeightWithoutGlass = 40m,
                TotalWeightGlass = 10m,
                Area = 20m,
                TotalArea = 100m,
                Hours = 5m,
                TotalHours = 25m,
                MaterialCost = 100m,
                LaborCost = 50m,
                Cost = 150m,
                TotalMaterialCost = 500m,
                TotalLaborCost = 250m,
                TotalCost = 750m,
                Price = 200m,
                TotalPrice = 1000m,
                WorksheetType = WorksheetType.Items
            };
        }

        private ItemDto CreateInvalidItemDto()
        {
            return new ItemDto
            {
                Id = Guid.NewGuid(),
                ItemName = null, // Invalid: required field
                Worksheet = "Sheet1",
                Quantity = 0 // Invalid: must be > 0
            };
        }

        private ItemEntity CreateValidItemEntity()
        {
            return new ItemEntity
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ItemName = "Test Item",
                Worksheet = "Sheet1",
                Line = 1,
                Column = 1,
                SortOrder = 1,
                Description = "Test Description",
                Quantity = 5,
                Width = 100m,
                Height = 200m,
                Weight = 10m,
                WeightWithoutGlass = 8m,
                WeightGlass = 2m,
                TotalWeight = 50m,
                TotalWeightWithoutGlass = 40m,
                TotalWeightGlass = 10m,
                Area = 20m,
                TotalArea = 100m,
                Hours = 5m,
                TotalHours = 25m,
                MaterialCost = 100m,
                CreatedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now
            };
        }

        #endregion
    }
}
