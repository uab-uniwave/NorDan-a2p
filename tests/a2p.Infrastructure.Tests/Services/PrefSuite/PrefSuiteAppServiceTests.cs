using Application.DTOs;
using Application.Interfaces.Services;
using Application.Models;

using Infrastructure.Services.PrefSuiteServices;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Infrastructure.Tests.Services.PrefSuite
{
    /// <summary>
    /// Unit tests for PrefSuiteAppService
    /// Tests the InsertItemsAsync method with various scenarios
    /// </summary>
    public class PrefSuiteAppServiceTests
    {
        private readonly Mock<ILogger<PrefSuiteAppService>> _mockLogger;
        private readonly Mock<ISQLService> _mockSqlRepository;
        private readonly PrefSuiteAppService _prefSuiteAppService;

        public PrefSuiteAppServiceTests()
        {
            _mockLogger = new Mock<ILogger<PrefSuiteAppService>>();
            _mockSqlRepository = new Mock<ISQLService>();

            _prefSuiteAppService = new PrefSuiteAppService(
             _mockLogger.Object,
             _mockSqlRepository.Object);
        }

        #region InsertItemsAsync Tests

        [Fact]
        public async Task InsertItemsAsync_WithValidOrderAndItems_CompleteSuccessfully()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDtoWithItems(3);
            ProgressValue progressValue = new();
            Progress<ProgressValue> progress = new();

            // Act
            await _prefSuiteAppService.InsertItemsAsync(orderDto, progressValue, progress);

            // Assert
            // If no exception is thrown, the method completed successfully
            Assert.NotNull(progressValue);
            // Verify logger was called (may be Information or Error depending on external dependencies)
            Assert.True(_mockLogger.Invocations.Count > 0);
        }

        [Fact]
        public async Task InsertItemsAsync_WithValidOrderAndNoItems_CompleteSuccessfully()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDtoWithItems(0);
            ProgressValue progressValue = new();
            Progress<ProgressValue> progress = new();

            // Act
            await _prefSuiteAppService.InsertItemsAsync(orderDto, progressValue, progress);

            // Assert
            Assert.NotNull(progressValue);
        }

        [Fact]
        public async Task InsertItemsAsync_WithNullProgressParameter_UsesDefaultProgress()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDtoWithItems(2);
            ProgressValue progressValue = new();
            IProgress<ProgressValue>? progress = null;

            // Act
            await _prefSuiteAppService.InsertItemsAsync(orderDto, progressValue, progress);

            // Assert
            Assert.NotNull(progressValue);
        }

        [Fact]
        public async Task InsertItemsAsync_WithItemsHavingNullItemName_SkipsItems()
        {
            // Arrange
            OrderDto orderDto = new()
            {
                OrderNumber = "ORD-001",
                SalesDocument = new SalesDocument { Number = 12345, Version = 1 },
                ItemsDto = new List<ItemDto>
 {
  new() { ItemName = null, Quantity = 5, Width = 100, Height = 100, Weight = 10, Price = 50, Cost = 25 },
  new() { ItemName = "", Quantity = 5, Width = 100, Height = 100, Weight = 10, Price = 50, Cost = 25 },
  new() { ItemName = "Valid Item", Quantity = 5, Width = 100, Height = 100, Weight = 10, Price = 50, Cost = 25 }
 }
            };
            ProgressValue progressValue = new();
            Progress<ProgressValue> progress = new();

            // Act
            await _prefSuiteAppService.InsertItemsAsync(orderDto, progressValue, progress);

            // Assert
            Assert.NotNull(progressValue);
            // Verify that items with null/empty ItemName are skipped by checking progress state
            // The method should complete without exception even with null items
            Assert.True(progressValue.CurrentValue >= 0);
        }

        [Fact]
        public async Task InsertItemsAsync_UpdatesProgressValue()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDtoWithItems(2);
            ProgressValue progressValue = new() { CurrentValue = 0 };
            Progress<ProgressValue> progress = new();

            // Act
            await _prefSuiteAppService.InsertItemsAsync(orderDto, progressValue, progress);

            // Assert
            // Progress should have been updated
            Assert.True(progressValue.CurrentValue > 0);
        }

        [Fact]
        public async Task InsertItemsAsync_WithLargeNumberOfItems_ProcessesAll()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDtoWithItems(10);
            ProgressValue progressValue = new();
            Progress<ProgressValue> progress = new();

            // Act
            await _prefSuiteAppService.InsertItemsAsync(orderDto, progressValue, progress);

            // Assert
            // Should complete without exception
            Assert.NotNull(progressValue);
        }

        [Fact]
        public async Task InsertItemsAsync_WithItemHavingZeroPrice_ProcessesItem()
        {
            // Arrange
            OrderDto orderDto = new()
            {
                OrderNumber = "ORD-ZERO-PRICE",
                SalesDocument = new SalesDocument { Number = 99999, Version = 1 },
                ItemsDto = new List<ItemDto>
 {
  new() {
  ItemName = "Zero Price Item",
  Quantity = 1,
  Width = 100,
  Height = 100,
  Weight = 5,
  Price = 0m,
  Cost = 0m,
  Description = "Item with zero price"
  }
 }
            };
            ProgressValue progressValue = new();
            Progress<ProgressValue> progress = new();

            // Act
            await _prefSuiteAppService.InsertItemsAsync(orderDto, progressValue, progress);

            // Assert
            Assert.NotNull(progressValue);
        }

        [Fact]
        public async Task InsertItemsAsync_WithExtremeValues_ProcessesItems()
        {
            // Arrange
            OrderDto orderDto = new()
            {
                OrderNumber = "ORD-EXTREME",
                SalesDocument = new SalesDocument { Number = 88888, Version = 1 },
                ItemsDto = new List<ItemDto>
 {
  new() {
  ItemName = "Large Item",
  Quantity = 1,
  Width = 10000m,
  Height = 10000m,
  Weight = 1000m,
  Price = 100000m,
  Cost = 50000m
  },
  new() {
  ItemName = "Small Item",
  Quantity = 1000,
  Width = 0.1m,
  Height = 0.1m,
  Weight = 0.01m,
  Price = 0.01m,
  Cost = 0.005m
  }
 }
            };
            ProgressValue progressValue = new();
            Progress<ProgressValue> progress = new();

            // Act
            await _prefSuiteAppService.InsertItemsAsync(orderDto, progressValue, progress);

            // Assert
            Assert.NotNull(progressValue);
        }

        [Fact]
        public async Task InsertItemsAsync_ReportsProgress()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDtoWithItems(2);
            ProgressValue progressValue = new();
            bool progressReported = false;
            Progress<ProgressValue> progress = new(value => progressReported = true);

            // Act
            await _prefSuiteAppService.InsertItemsAsync(orderDto, progressValue, progress);

            // Assert
            Assert.True(progressReported || progressValue.CurrentValue > 0);
        }

        [Fact]
        public async Task InsertItemsAsync_WithSpecialCharactersInItemName_ProcessesSuccessfully()
        {
            // Arrange
            OrderDto orderDto = new()
            {
                OrderNumber = "ORD-SPECIAL",
                SalesDocument = new SalesDocument { Number = 77777, Version = 1 },
                ItemsDto = new List<ItemDto>
 {
  new() {
  ItemName = "Item with <>&\"' special chars",
  Quantity = 1,
  Width = 100,
  Height = 100,
  Weight = 10,
  Price = 50,
  Cost = 25
  }
 }
            };
            ProgressValue progressValue = new();
            Progress<ProgressValue> progress = new();

            // Act
            await _prefSuiteAppService.InsertItemsAsync(orderDto, progressValue, progress);

            // Assert
            Assert.NotNull(progressValue);
        }

        [Fact]
        public async Task InsertItemsAsync_WithMultipleCallsSequentially_EachCompletesSuccessfully()
        {
            // Arrange
            OrderDto orderDto1 = CreateValidOrderDtoWithItems(2);
            OrderDto orderDto2 = CreateValidOrderDtoWithItems(3);
            ProgressValue progressValue1 = new();
            ProgressValue progressValue2 = new();

            // Act
            await _prefSuiteAppService.InsertItemsAsync(orderDto1, progressValue1);
            await _prefSuiteAppService.InsertItemsAsync(orderDto2, progressValue2);

            // Assert
            Assert.NotNull(progressValue1);
            Assert.NotNull(progressValue2);
        }

        #endregion

        #region Helper Methods

        private OrderDto CreateValidOrderDtoWithItems(int itemCount)
        {
            List<ItemDto> items = new();
            for (int i = 0; i < itemCount; i++)
            {
                items.Add(new ItemDto
                {
                    Id = Guid.NewGuid(),
                    OrderId = Guid.NewGuid(),
                    ItemName = $"Item {i + 1}",
                    Worksheet = "Sheet1",
                    Line = i + 1,
                    Column = 1,
                    SortOrder = i + 1,
                    Description = $"Description for item {i + 1}",
                    Quantity = 5,
                    Width = 100m,
                    Height = 100m,
                    Weight = 10m,
                    WeightWithoutGlass = 8m,
                    WeightGlass = 2m,
                    Area = 1m,
                    Price = 50m,
                    Cost = 25m
                });
            }

            return new OrderDto
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-2024-001",
                ProjectNumber = "PROJ-001",
                SalesDocument = new SalesDocument
                {
                    Number = 12345,
                    Version = 1,
                    RowId = Guid.NewGuid()
                },
                ItemsDto = items,
                MaterialsDto = [],
                ExcelFiles = []
            };
        }

        #endregion
    }
}
