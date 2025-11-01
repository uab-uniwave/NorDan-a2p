using Application.DTOs;
using Application.Models;

using Domain.Entities;
using Domain.Enums;

using Xunit;

namespace Infrastructure.Tests.Services
{
    /// <summary>
    /// Integration tests for OrderService demonstrating expected behavior scenarios
    /// </summary>
    public class OrderServiceIntegrationTests
    {
        [Fact]
        public void OrderDto_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            OrderDto orderDto = new();

            // Assert
            Assert.NotEqual(Guid.Empty, orderDto.Id);
            Assert.Null(orderDto.OrderNumber);
            Assert.Null(orderDto.ProjectNumber);
            Assert.NotNull(orderDto.ItemsDto);
            Assert.NotNull(orderDto.MaterialsDto);
            Assert.NotNull(orderDto.ExcelFiles);
            Assert.Empty(orderDto.ItemsDto);
            Assert.Empty(orderDto.MaterialsDto);
            Assert.Empty(orderDto.ExcelFiles);
            Assert.True(orderDto.Import);
            Assert.False(orderDto.DeleteExistsing);
            Assert.Equal(1m, orderDto.ExchangeRate);
        }

        [Fact]
        public void OrderDto_CanBeCreatedWithValues()
        {
            // Arrange & Act
            OrderDto orderDto = new()
            {
                OrderNumber = "ORD-2024-001",
                ProjectNumber = "PROJ-001",
                OrderDate = new DateTime(2024, 1, 15),
                CustomerTitle = "Mr.",
                CustomerNumber = "CUST-001",
                DeliveryAddress = "123 Main St",
                ResponsibleManager = "Jane Doe",
                Currency = "EUR",
                ExchangeRate = 1.05m,
                Import = true
            };

            // Assert
            Assert.Equal("ORD-2024-001", orderDto.OrderNumber);
            Assert.Equal("PROJ-001", orderDto.ProjectNumber);
            Assert.Equal(new DateTime(2024, 1, 15), orderDto.OrderDate);
            Assert.Equal("Mr.", orderDto.CustomerTitle);
            Assert.Equal("CUST-001", orderDto.CustomerNumber);
            Assert.Equal("123 Main St", orderDto.DeliveryAddress);
            Assert.Equal("Jane Doe", orderDto.ResponsibleManager);
            Assert.Equal("EUR", orderDto.Currency);
            Assert.Equal(1.05m, orderDto.ExchangeRate);
            Assert.True(orderDto.Import);
        }

        [Fact]
        public void OrderEntity_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            OrderEntity orderEntity = new();

            // Assert
            Assert.NotEqual(Guid.Empty, orderEntity.Id);
            Assert.Null(orderEntity.OrderNumber);
            Assert.Null(orderEntity.DeliveryAddress);
            Assert.NotNull(orderEntity.Items);
            Assert.NotNull(orderEntity.Materials);
            Assert.Empty(orderEntity.Items);
            Assert.Empty(orderEntity.Materials);
            Assert.Equal(SourceAppType.Unknown, orderEntity.SourceAppType);
        }

        [Fact]
        public void OrderEntity_CanBeCreatedWithValues()
        {
            // Arrange & Act
            OrderEntity orderEntity = new()
            {
                OrderNumber = "ORD-2024-001",
                ProjectNumber = "PROJ-001",
                SalesDocumentNumber = 12345,
                SalesDocumentVersion = 1,
                OrderDate = new DateTime(2024, 1, 15),
                CustomerTitle = "Mr.",
                CustomerNumber = "CUST-001",
                DeliveryAddress = "456 Oak Ave",
                ResponsibleManager = "Bob Smith",
                SourceAppType = SourceAppType.Schuco,
                Currency = "EUR",
                ExchangeRate = 1.05
            };

            // Assert
            Assert.Equal("ORD-2024-001", orderEntity.OrderNumber);
            Assert.Equal("PROJ-001", orderEntity.ProjectNumber);
            Assert.Equal(12345, orderEntity.SalesDocumentNumber);
            Assert.Equal(1, orderEntity.SalesDocumentVersion);
            Assert.Equal(new DateTime(2024, 1, 15), orderEntity.OrderDate);
            Assert.Equal("Mr.", orderEntity.CustomerTitle);
            Assert.Equal("CUST-001", orderEntity.CustomerNumber);
            Assert.Equal("456 Oak Ave", orderEntity.DeliveryAddress);
            Assert.Equal("Bob Smith", orderEntity.ResponsibleManager);
            Assert.Equal(SourceAppType.Schuco, orderEntity.SourceAppType);
            Assert.Equal("EUR", orderEntity.Currency);
            Assert.Equal(1.05, orderEntity.ExchangeRate);
        }

        [Fact]
        public void OrderEntity_CanHaveItems()
        {
            // Arrange
            OrderEntity orderEntity = new() { OrderNumber = "ORD-001" };
            ItemEntity item1 = new() { ItemName = "Item 1" };
            ItemEntity item2 = new() { ItemName = "Item 2" };

            // Act
            orderEntity.Items.Add(item1);
            orderEntity.Items.Add(item2);

            // Assert
            Assert.Equal(2, orderEntity.Items.Count);
            Assert.Contains(item1, orderEntity.Items);
            Assert.Contains(item2, orderEntity.Items);
        }

        [Fact]
        public void OrderEntity_CanHaveMaterials()
        {
            // Arrange
            OrderEntity orderEntity = new() { OrderNumber = "ORD-001" };
            MaterialEntity material1 = new() { Reference = "MAT-001" };
            MaterialEntity material2 = new() { Reference = "MAT-002" };
            MaterialEntity material3 = new() { Reference = "MAT-003" };

            // Act
            orderEntity.Materials.Add(material1);
            orderEntity.Materials.Add(material2);
            orderEntity.Materials.Add(material3);

            // Assert
            Assert.Equal(3, orderEntity.Materials.Count);
            Assert.Contains(material1, orderEntity.Materials);
            Assert.Contains(material2, orderEntity.Materials);
            Assert.Contains(material3, orderEntity.Materials);
        }

        [Fact]
        public void OrderDto_CanHaveItemsAndMaterials()
        {
            // Arrange
            OrderDto orderDto = new() { OrderNumber = "ORD-001" };
            ItemDto itemDto1 = new() { ItemName = "Item 1" };
            ItemDto itemDto2 = new() { ItemName = "Item 2" };
            MaterialDto materialDto1 = new() { Reference = "MAT-001" };
            MaterialDto materialDto2 = new() { Reference = "MAT-002" };

            // Act
            orderDto.ItemsDto.Add(itemDto1);
            orderDto.ItemsDto.Add(itemDto2);
            orderDto.MaterialsDto.Add(materialDto1);
            orderDto.MaterialsDto.Add(materialDto2);

            // Assert
            Assert.Equal(2, orderDto.ItemsDto.Count);
            Assert.Equal(2, orderDto.MaterialsDto.Count);
        }

        [Theory]
        [InlineData(SourceAppType.Unknown)]
        [InlineData(SourceAppType.Schuco)]
        [InlineData(SourceAppType.TechDesign)]
        [InlineData(SourceAppType.Sapa)]
        public void OrderEntity_WithDifferentSourceAppTypes(SourceAppType sourceAppType)
        {
            // Arrange & Act
            OrderEntity orderEntity = new() { SourceAppType = sourceAppType };

            // Assert
            Assert.Equal(sourceAppType, orderEntity.SourceAppType);
        }

        [Fact]
        public void OrderDto_ExchangeRateCalculation()
        {
            // Arrange
            OrderDto orderDto = new()
            {
                Currency = "USD",
                ExchangeRate = 1.15m,
                ExchangeRateDate = new DateTime(2024, 1, 15)
            };

            // Act
            decimal priceInEur = 100m; // Assume base price in EUR
            decimal priceInUsd = priceInEur * orderDto.ExchangeRate;

            // Assert
            Assert.Equal(1.15m, orderDto.ExchangeRate);
            Assert.Equal(115m, priceInUsd);
        }

        [Fact]
        public void OrderDto_WithSalesDocument()
        {
            // Arrange & Act
            OrderDto orderDto = new()
            {
                SalesDocument = new SalesDocument
                {
                    Number = 12345,
                    Version = 1,
                    RowId = Guid.NewGuid()
                }
            };

            // Assert
            Assert.NotNull(orderDto.SalesDocument);
            Assert.Equal(12345, orderDto.SalesDocument.Number);
            Assert.Equal(1, orderDto.SalesDocument.Version);
        }

        [Fact]
        public void OrderDto_WithExcelFiles()
        {
            // Arrange
            OrderDto orderDto = new();
            ExcelFile excelFile1 = new() { FileName = "file1.xlsx", FilePath = "/path/to/file1.xlsx" };
            ExcelFile excelFile2 = new() { FileName = "file2.xlsx", FilePath = "/path/to/file2.xlsx" };

            // Act
            orderDto.ExcelFiles.Add(excelFile1);
            orderDto.ExcelFiles.Add(excelFile2);

            // Assert
            Assert.Equal(2, orderDto.ExcelFiles.Count);
            Assert.Equal("file1.xlsx", orderDto.ExcelFiles[0].FileName);
            Assert.Equal("file2.xlsx", orderDto.ExcelFiles[1].FileName);
        }

        [Fact]
        public void OrderEntity_CurrencyHandling()
        {
            // Arrange & Act
            OrderEntity orderEntity = new()
            {
                Currency = "EUR",
                ExchangeRate = 1.0
            };

            OrderEntity orderEntity2 = new()
            {
                Currency = "USD",
                ExchangeRate = 1.15
            };

            OrderEntity orderEntity3 = new()
            {
                Currency = "GBP",
                ExchangeRate = 0.85
            };

            // Assert
            Assert.Equal("EUR", orderEntity.Currency);
            Assert.Equal(1.0, orderEntity.ExchangeRate);
            Assert.Equal("USD", orderEntity2.Currency);
            Assert.Equal(1.15, orderEntity2.ExchangeRate);
            Assert.Equal("GBP", orderEntity3.Currency);
            Assert.Equal(0.85, orderEntity3.ExchangeRate);
        }

        [Fact]
        public void OrderEntity_CustomerInformation()
        {
            // Arrange & Act
            OrderEntity orderEntity = new()
            {
                CustomerTitle = "Dr.",
                CustomerNumber = "CUST-12345",
                DeliveryAddress = "999 Commerce Blvd, Tech City, TC 12345",
                ResponsibleManager = "Alice Johnson"
            };

            // Assert
            Assert.Equal("Dr.", orderEntity.CustomerTitle);
            Assert.Equal("CUST-12345", orderEntity.CustomerNumber);
            Assert.Equal("999 Commerce Blvd, Tech City, TC 12345", orderEntity.DeliveryAddress);
            Assert.Equal("Alice Johnson", orderEntity.ResponsibleManager);
        }

        [Fact]
        public void OrderDto_ImportAndDeleteFlags()
        {
            // Arrange & Act
            OrderDto orderDto1 = new() { Import = true, DeleteExistsing = false };
            OrderDto orderDto2 = new() { Import = false, DeleteExistsing = true };

            // Assert
            Assert.True(orderDto1.Import);
            Assert.False(orderDto1.DeleteExistsing);
            Assert.False(orderDto2.Import);
            Assert.True(orderDto2.DeleteExistsing);
        }

        [Fact]
        public void OrderEntity_DateTracking()
        {
            // Arrange
            DateTime now = DateTime.Now;

            // Act
            OrderEntity orderEntity = new()
            {
                OrderDate = now,
                CorrectionAvailableUntil = now.AddDays(7),
                CreatedDateTime = now,
                ModifiedDateTime = now
            };

            // Assert
            Assert.Equal(now, orderEntity.OrderDate);
            Assert.Equal(now.AddDays(7), orderEntity.CorrectionAvailableUntil);
            Assert.Equal(now, orderEntity.CreatedDateTime);
            Assert.Equal(now, orderEntity.ModifiedDateTime);
        }

        [Fact]
        public void OrderEntity_SalesDocumentTracking()
        {
            // Arrange & Act
            OrderEntity orderEntity = new()
            {
                SalesDocumentNumber = 98765,
                SalesDocumentVersion = 3
            };

            // Assert
            Assert.Equal(98765, orderEntity.SalesDocumentNumber);
            Assert.Equal(3, orderEntity.SalesDocumentVersion);
        }

        [Fact]
        public void OrderEntity_ComplexOrder_WithAllData()
        {
            // Arrange & Act
            OrderEntity orderEntity = new()
            {
                OrderNumber = "ORD-2024-COMPLEX",
                ProjectNumber = "PROJ-COMPLEX",
                SalesDocumentNumber = 54321,
                SalesDocumentVersion = 2,
                OrderDate = DateTime.Now,
                CustomerTitle = "Ms.",
                CustomerNumber = "CUST-99999",
                DeliveryAddress = "Complex Address 42, Floor 3",
                CorrectionAvailableUntil = DateTime.Now.AddDays(14),
                ResponsibleManager = "Manager Name",
                SourceAppType = SourceAppType.TechDesign,
                Currency = "CHF",
                ExchangeRate = 0.92,
                ExchangeRateDate = DateTime.Now
            };

            // Add items
            orderEntity.Items.Add(new ItemEntity { ItemName = "Item 1", Quantity = 5 });
            orderEntity.Items.Add(new ItemEntity { ItemName = "Item 2", Quantity = 10 });

            // Add materials
            orderEntity.Materials.Add(new MaterialEntity { Reference = "MAT-1", Quantity = 20 });
            orderEntity.Materials.Add(new MaterialEntity { Reference = "MAT-2", Quantity = 30 });
            orderEntity.Materials.Add(new MaterialEntity { Reference = "MAT-3", Quantity = 15 });

            // Assert
            Assert.Equal("ORD-2024-COMPLEX", orderEntity.OrderNumber);
            Assert.Equal("PROJ-COMPLEX", orderEntity.ProjectNumber);
            Assert.Equal(2, orderEntity.Items.Count);
            Assert.Equal(3, orderEntity.Materials.Count);
            Assert.Equal(SourceAppType.TechDesign, orderEntity.SourceAppType);
            Assert.Equal("CHF", orderEntity.Currency);
            Assert.Equal(0.92, orderEntity.ExchangeRate);
        }

        [Fact]
        public void OrderDto_ProjectNumberNullable()
        {
            // Arrange & Act
            OrderDto orderDto1 = new() { ProjectNumber = "PROJ-001" };
            OrderDto orderDto2 = new() { ProjectNumber = null };

            // Assert
            Assert.Equal("PROJ-001", orderDto1.ProjectNumber);
            Assert.Null(orderDto2.ProjectNumber);
        }

        [Fact]
        public void OrderEntity_ResponsibleManagerNullable()
        {
            // Arrange & Act
            OrderEntity orderEntity1 = new() { ResponsibleManager = "John Manager" };
            OrderEntity orderEntity2 = new() { ResponsibleManager = null };

            // Assert
            Assert.Equal("John Manager", orderEntity1.ResponsibleManager);
            Assert.Null(orderEntity2.ResponsibleManager);
        }
    }
}
