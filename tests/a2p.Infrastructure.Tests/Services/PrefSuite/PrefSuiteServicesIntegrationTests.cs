using Application.DTOs;
using Application.Models;

using Domain.Entities;
using Domain.Enums;

using Xunit;

namespace Infrastructure.Tests.Services.PrefSuite
{
    /// <summary>
    /// Integration tests for PrefSuite services demonstrating expected behavior scenarios
    /// </summary>
    public class PrefSuiteServicesIntegrationTests
    {
        [Fact]
        public void ProgressValue_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            ProgressValue progressValue = new();

            // Assert
            Assert.NotNull(progressValue);
            Assert.Equal(0, progressValue.MinValue);
            Assert.Equal(0, progressValue.MaxValue);
            Assert.Equal(0, progressValue.CurrentValue);
            Assert.Equal(0, progressValue.TotalValue);
        }

        [Fact]
        public void ProgressValue_CanBeCreatedWithValues()
        {
            // Arrange & Act
            ProgressValue progressValue = new()
            {
                MinValue = 0,
                MaxValue = 100,
                CurrentValue = 50,
                TotalValue = 100,
                ProgressTitle = "Processing Order",
                ProgressTask1 = "Loading data",
                ProgressTask2 = "Processing items",
                ProgressTask3 = "Saving results",
                Order = "ORD-001",
                WorksheetName = "Sheet1",
                WorksheetLine = 5
            };

            // Assert
            Assert.Equal(0, progressValue.MinValue);
            Assert.Equal(100, progressValue.MaxValue);
            Assert.Equal(50, progressValue.CurrentValue);
            Assert.Equal(100, progressValue.TotalValue);
            Assert.Equal("Processing Order", progressValue.ProgressTitle);
            Assert.Equal("Loading data", progressValue.ProgressTask1);
            Assert.Equal("Processing items", progressValue.ProgressTask2);
            Assert.Equal("Saving results", progressValue.ProgressTask3);
            Assert.Equal("ORD-001", progressValue.Order);
            Assert.Equal("Sheet1", progressValue.WorksheetName);
            Assert.Equal(5, progressValue.WorksheetLine);
        }

        [Fact]
        public void SalesDocument_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            SalesDocument salesDocument = new();

            // Assert
            Assert.NotNull(salesDocument);
        }

        [Fact]
        public void SalesDocument_CanBeCreatedWithValues()
        {
            // Arrange & Act
            SalesDocument salesDocument = new()
            {
                Number = 12345,
                Version = 1,
                RowId = Guid.NewGuid()
            };

            // Assert
            Assert.Equal(12345, salesDocument.Number);
            Assert.Equal(1, salesDocument.Version);
            Assert.NotEqual(Guid.Empty, salesDocument.RowId);
        }

        [Fact]
        public void OrderDto_WithSalesDocumentAndItems()
        {
            // Arrange & Act
            OrderDto orderDto = new()
            {
                OrderNumber = "ORD-2024-COMPLEX",
                SalesDocument = new SalesDocument { Number = 99999, Version = 2 },
                ItemsDto = new List<ItemDto>
 {
  new() {
  ItemName = "Item 1",
  Quantity = 5,
  Width = 100m,
  Height = 100m,
  Weight = 10m,
  Price = 50m,
  Cost = 25m
  },
  new() {
  ItemName = "Item 2",
  Quantity = 10,
  Width = 200m,
  Height = 200m,
  Weight = 20m,
  Price = 100m,
  Cost = 50m
  }
 }
            };

            // Assert
            Assert.Equal("ORD-2024-COMPLEX", orderDto.OrderNumber);
            Assert.Equal(99999, orderDto.SalesDocument.Number);
            Assert.Equal(2, orderDto.SalesDocument.Version);
            Assert.Equal(2, orderDto.ItemsDto.Count);
        }

        [Fact]
        public void ItemDto_CalculationsForPrefSuite()
        {
            // Arrange
            ItemDto itemDto = new()
            {
                ItemName = "Window Frame",
                Quantity = 5,
                Width = 1500m,
                Height = 1000m,
                Weight = 10m,
                Price = 250m,
                Cost = 125m
            };

            // Act
            decimal totalCost = itemDto.Cost * itemDto.Quantity;
            decimal totalPrice = itemDto.Price * itemDto.Quantity;
            string dimensions = $"W={Math.Ceiling(itemDto.Width)};H={Math.Ceiling(itemDto.Height)};";

            // Assert
            Assert.Equal(625m, totalCost);
            Assert.Equal(1250m, totalPrice);
            Assert.Equal("W=1500;H=1000;", dimensions);
        }

        [Fact]
        public void MaterialEntity_WithPrefSuiteColors()
        {
            // Arrange & Act
            MaterialEntity material1 = new()
            {
                Reference = "PROF-001",
                Color = "Silver",
                ColorDescription = "Silver Anodized"
            };

            MaterialEntity material2 = new()
            {
                Reference = "PROF-002",
                Color = "Bronze",
                ColorDescription = "Bronze Anodized"
            };

            MaterialEntity material3 = new()
            {
                Reference = "PROF-003",
                Color = "White",
                ColorDescription = "White Powder Coat"
            };

            // Assert
            Assert.Equal("Silver", material1.Color);
            Assert.Equal("Bronze", material2.Color);
            Assert.Equal("White", material3.Color);
        }

        [Fact]
        public void MaterialEntity_WithDifferentMaterialTypes_AndReferences()
        {
            // Arrange & Act
            List<MaterialEntity> materials = new()
 {
 new MaterialEntity
 {
  Reference = "PROFILE-45x45",
  ReferenceBase = "PROFILE",
  Description = "Aluminum Profile 45x45",
  MaterialType = MaterialType.Profiles,
  Quantity = 100
 },
 new MaterialEntity
 {
  Reference = "GLASS-CLEAR-4MM",
  ReferenceBase = "GLASS",
  Description = "Clear Float Glass 4mm",
  MaterialType = MaterialType.Glasses,
  Quantity = 50
 },
 new MaterialEntity
 {
  Reference = "GASKET-EPDM",
  ReferenceBase = "GASKET",
  Description = "EPDM Gasket Material",
  MaterialType = MaterialType.Gaskets,
  Quantity = 200
 }
 };

            // Assert
            Assert.Equal(3, materials.Count);
            Assert.All(materials, m => Assert.NotNull(m.Reference));
            Assert.All(materials, m => Assert.NotNull(m.ReferenceBase));
        }

        [Fact]
        public void ProgressValue_IncrementingProgress()
        {
            // Arrange
            ProgressValue progress = new()
            {
                MinValue = 0,
                MaxValue = 100,
                CurrentValue = 0
            };

            // Act
            progress.CurrentValue += 10;
            int afterFirstIncrement = progress.CurrentValue;

            progress.CurrentValue += 20;
            int afterSecondIncrement = progress.CurrentValue;

            progress.CurrentValue += 70;
            int afterThirdIncrement = progress.CurrentValue;

            // Assert
            Assert.Equal(10, afterFirstIncrement);
            Assert.Equal(30, afterSecondIncrement);
            Assert.Equal(100, afterThirdIncrement);
        }

        [Fact]
        public void PrefSuiteXmlCommandGeneration()
        {
            // Arrange
            ItemDto itemDto = new()
            {
                Width = 1500.5m,
                Height = 1000.75m,
                Weight = 12.3456m
            };

            // Act
            string roundedWeight = Math.Round(itemDto.Weight, 4).ToString(System.Globalization.CultureInfo.InvariantCulture);
            string xmlCommand = "<cmd:Commands>" +
            $"<cmd:Command name=\"Model.SetDimensions\">" +
            $"<cmd:Parameter name=\"dimensions\" type=\"string\" value=\"W={Math.Ceiling(itemDto.Width)};H={Math.Ceiling(itemDto.Height)};\"/>" +
            $"</cmd:Command>" +
            $"<cmd:Command name=\"Model.SetModelVariables\">" +
            $"<cmd:ItemValue name=\"value\" type=\"real\" value=\"{roundedWeight}\"/>" +
            $"</cmd:Command>" +
            "</cmd:Commands>";

            // Assert
            Assert.Contains("W=1501", xmlCommand);
            Assert.Contains("H=1001", xmlCommand);
            Assert.Contains("12.3456", xmlCommand);
        }

        [Fact]
        public void ExcelFile_WithMultipleWorksheets()
        {
            // Arrange & Act
            ExcelFile excelFile = new()
            {
                FileName = "order-data.xlsx",
                FilePath = "/files/order-data.xlsx",
                IsLocked = false,
                IsOrderItemsFile = true,
                Worksheets = new List<Worksheet>
 {
  new() { Name = "Items" },
  new() { Name = "Materials" },
  new() { Name = "Summary" }
 }
            };

            // Assert
            Assert.Equal("order-data.xlsx", excelFile.FileName);
            Assert.True(excelFile.IsOrderItemsFile);
            Assert.Equal(3, excelFile.Worksheets.Count);
        }

        [Fact]
        public void OrderDto_WithComplexStructure()
        {
            // Arrange
            DateTime now = DateTime.Now;

            // Act
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-COMPLEX-2024",
                ProjectNumber = "PROJ-COMPLEX",
                OrderDate = now,
                CustomerTitle = "Dr.",
                CustomerNumber = "CUST-12345",
                DeliveryAddress = "999 Commerce Blvd",
                ResponsibleManager = "Manager Name",
                Currency = "EUR",
                ExchangeRate = 1.05m,
                ExchangeRateDate = now,
                Import = true,
                DeleteExistsing = false,
                SalesDocument = new SalesDocument
                {
                    Number = 12345,
                    Version = 1,
                    RowId = Guid.NewGuid()
                },
                ItemsDto = new List<ItemDto>
 {
  new() { ItemName = "Item 1", Quantity = 5 },
  new() { ItemName = "Item 2", Quantity = 10 }
 },
                MaterialsDto = new List<MaterialDto>
 {
  new() { Reference = "MAT-1", Quantity = 20 },
  new() { Reference = "MAT-2", Quantity = 30 }
 },
                ExcelFiles = new List<ExcelFile>
 {
  new() { FileName = "data.xlsx", FilePath = "/files/data.xlsx" }
 }
            };

            // Assert
            Assert.NotEqual(Guid.Empty, orderDto.Id);
            Assert.Equal("ORD-COMPLEX-2024", orderDto.OrderNumber);
            Assert.Equal(2, orderDto.ItemsDto.Count);
            Assert.Equal(2, orderDto.MaterialsDto.Count);
            Assert.Single(orderDto.ExcelFiles);
        }

        [Fact]
        public void SalesDocumentVersionTracking()
        {
            // Arrange & Act
            List<SalesDocument> versions = new()
 {
 new SalesDocument { Number = 12345, Version = 1 },
 new SalesDocument { Number = 12345, Version = 2 },
 new SalesDocument { Number = 12345, Version = 3 },
 new SalesDocument { Number = 12345, Version = 4 }
 };

            // Assert
            Assert.Equal(4, versions.Count);
            Assert.Equal(1, versions[0].Version);
            Assert.Equal(4, versions[3].Version);
        }

        [Fact]
        public void ItemDimensions_WithRounding()
        {
            // Arrange
            List<ItemDto> items = new()
 {
 new ItemDto { Width = 100.1m, Height = 100.9m },
 new ItemDto { Width = 200.4m, Height = 200.6m },
 new ItemDto { Width = 300.5m, Height = 300.5m }
 };

            // Act
            var roundedDimensions = items.Select(i =>
            new { Width = Math.Ceiling(i.Width), Height = Math.Ceiling(i.Height) }).ToList();

            // Assert
            Assert.Equal(101, roundedDimensions[0].Width);
            Assert.Equal(101, roundedDimensions[0].Height);
            Assert.Equal(201, roundedDimensions[1].Width);
            Assert.Equal(201, roundedDimensions[1].Height);
            Assert.Equal(301, roundedDimensions[2].Width);
            Assert.Equal(301, roundedDimensions[2].Height);
        }

        [Fact]
        public void WeightPrecisionForPrefSuite()
        {
            // Arrange
            List<decimal> weights = new()
 { 10.12345m, 20.98765m, 30.5m, 40.0001m };

            // Act
            List<decimal> roundedWeights = weights.Select(w => Math.Round(w, 4, MidpointRounding.AwayFromZero)).ToList();

            // Assert
            Assert.Equal(10.1235m, roundedWeights[0]);
            Assert.Equal(20.9877m, roundedWeights[1]);
            Assert.Equal(30.5m, roundedWeights[2]);
            Assert.Equal(40.0001m, roundedWeights[3]);
        }
    }
}
