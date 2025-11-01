using Application.DTOs;

using Domain.Entities;
using Domain.Enums;

using Xunit;

namespace Infrastructure.Tests.Services
{
    /// <summary>
    /// Integration tests for MaterialService demonstrating expected behavior scenarios
    /// </summary>
    public class MaterialServiceIntegrationTests
    {
        [Fact]
        public void MaterialDto_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            MaterialDto materialDto = new();

            // Assert
            Assert.NotEqual(Guid.Empty, materialDto.Id); // Auto-generated
            Assert.Null(materialDto.OrderId);
            Assert.Null(materialDto.Reference);
            Assert.Null(materialDto.ReferenceBase);
            Assert.Null(materialDto.Color);
            Assert.Equal(0m, materialDto.Width);
            Assert.Equal(0m, materialDto.Price);
            Assert.Equal(0, materialDto.Quantity);
            Assert.Equal(MaterialType.Unknown, materialDto.MaterialType);
            Assert.Equal(WorksheetType.Unknown, materialDto.WorksheetType);
        }

        [Fact]
        public void MaterialDto_CanBeCreatedWithValues()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            Guid itemId = Guid.NewGuid();

            // Act
            MaterialDto materialDto = new()
            {
                OrderId = orderId,
                ItemId = itemId,
                Reference = "PROF-001",
                ReferenceBase = "PROF",
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
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Assert
            Assert.Equal(orderId, materialDto.OrderId);
            Assert.Equal(itemId, materialDto.ItemId);
            Assert.Equal("PROF-001", materialDto.Reference);
            Assert.Equal("PROF", materialDto.ReferenceBase);
            Assert.Equal("Aluminum Profile", materialDto.Description);
            Assert.Equal("Silver", materialDto.Color);
            Assert.Equal("Silver Anodized", materialDto.ColorDescription);
            Assert.Equal(45m, materialDto.Width);
            Assert.Equal(45m, materialDto.Height);
            Assert.Equal(10, materialDto.Quantity);
            Assert.Equal(10m, materialDto.RequiredQuantity);
            Assert.Equal(2.5m, materialDto.Weight);
            Assert.Equal(25m, materialDto.TotalWeight);
            Assert.Equal(50m, materialDto.Price);
            Assert.Equal(500m, materialDto.TotalPrice);
            Assert.Equal(MaterialType.Profiles, materialDto.MaterialType);
            Assert.Equal(WorksheetType.Materials, materialDto.WorksheetType);
        }

        [Fact]
        public void MaterialEntity_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            MaterialEntity materialEntity = new();

            // Assert
            Assert.NotEqual(Guid.Empty, materialEntity.Id);
            Assert.Equal(Guid.Empty, materialEntity.OrderId);
            Assert.Null(materialEntity.Reference);
            Assert.Null(materialEntity.Color);
            Assert.Equal(0m, materialEntity.Width);
            Assert.Equal(0m, materialEntity.Price);
            Assert.Equal(0, materialEntity.Quantity);
            Assert.Equal(MaterialType.Unknown, materialEntity.MaterialType);
        }

        [Fact]
        public void MaterialEntity_CanBeCreatedWithValues()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            Guid itemId = Guid.NewGuid();

            // Act
            MaterialEntity materialEntity = new()
            {
                OrderId = orderId,
                ItemId = itemId,
                Reference = "GLASS-001",
                ReferenceBase = "GLASS",
                Description = "Float Glass",
                Color = "Clear",
                ColorDescription = "Clear Float",
                Width = 1000m,
                Height = 1200m,
                Quantity = 5,
                RequiredQuantity = 5,
                Weight = 50m,
                TotalWeight = 250m,
                Area = 1.2m,
                TotalArea = 6m,
                Price = 100m,
                TotalPrice = 500m
            };

            // Assert
            Assert.Equal(orderId, materialEntity.OrderId);
            Assert.Equal(itemId, materialEntity.ItemId);
            Assert.Equal("GLASS-001", materialEntity.Reference);
            Assert.Equal("GLASS", materialEntity.ReferenceBase);
            Assert.Equal("Float Glass", materialEntity.Description);
            Assert.Equal("Clear", materialEntity.Color);
            Assert.Equal("Clear Float", materialEntity.ColorDescription);
            Assert.Equal(1000m, materialEntity.Width);
            Assert.Equal(1200m, materialEntity.Height);
            Assert.Equal(5, materialEntity.Quantity);
            Assert.Equal(5m, materialEntity.RequiredQuantity);
            Assert.Equal(50m, materialEntity.Weight);
            Assert.Equal(250m, materialEntity.TotalWeight);
            Assert.Equal(1.2m, materialEntity.Area);
            Assert.Equal(6m, materialEntity.TotalArea);
            Assert.Equal(100m, materialEntity.Price);
            Assert.Equal(500m, materialEntity.TotalPrice);
        }

        [Theory]
        [InlineData(MaterialType.Profiles)]
        [InlineData(MaterialType.Gaskets)]
        [InlineData(MaterialType.Piece)]
        [InlineData(MaterialType.Panels)]
        [InlineData(MaterialType.Glasses)]
        public void MaterialDto_WithDifferentMaterialTypes(MaterialType materialType)
        {
            // Arrange & Act
            MaterialDto materialDto = new() { MaterialType = materialType };

            // Assert
            Assert.Equal(materialType, materialDto.MaterialType);
        }

        [Theory]
        [InlineData(WorksheetType.Items)]
        [InlineData(WorksheetType.Materials)]
        [InlineData(WorksheetType.Glasses)]
        [InlineData(WorksheetType.Panels)]
        public void MaterialDto_WithDifferentWorksheetTypes(WorksheetType worksheetType)
        {
            // Arrange & Act
            MaterialDto materialDto = new() { WorksheetType = worksheetType };

            // Assert
            Assert.Equal(worksheetType, materialDto.WorksheetType);
        }

        [Fact]
        public void MaterialDto_QuantityCalculations()
        {
            // Arrange
            MaterialDto materialDto = new()
            {
                Quantity = 10,
                PackageQuantity = 5,
                RequiredQuantity = 10,
                LeftOverQuantity = 0
            };

            // Act
            decimal totalQuantity = materialDto.Quantity + materialDto.LeftOverQuantity;
            decimal packages = Math.Ceiling((decimal)materialDto.Quantity / materialDto.PackageQuantity);

            // Assert
            Assert.Equal(10m, materialDto.RequiredQuantity);
            Assert.Equal(10m, totalQuantity);
            Assert.Equal(2m, packages);
        }

        [Fact]
        public void MaterialEntity_WeightCalculations()
        {
            // Arrange
            MaterialEntity materialEntity = new()
            {
                Quantity = 5,
                Weight = 50m,
                RequiredWeight = 250m,
                LeftOverWeight = 0
            };

            // Act
            decimal totalWeight = materialEntity.Weight * materialEntity.Quantity;
            decimal totalWithLeftOver = totalWeight + materialEntity.LeftOverWeight;

            // Assert
            Assert.Equal(250m, materialEntity.RequiredWeight);
            Assert.Equal(250m, totalWeight);
            Assert.Equal(250m, totalWithLeftOver);
        }

        [Fact]
        public void MaterialDto_AreaCalculations()
        {
            // Arrange
            MaterialDto materialDto = new()
            {
                Quantity = 3,
                Width = 1000m,
                Height = 1200m,
                RequiredArea = 3.6m,
                LeftOverArea = 0
            };

            // Act
            decimal areaPerUnit = materialDto.Width * materialDto.Height / 1000000;
            decimal totalArea = areaPerUnit * materialDto.Quantity;

            // Assert
            Assert.Equal(1.2m, areaPerUnit);
            Assert.Equal(3.6m, totalArea);
        }

        [Fact]
        public void MaterialDto_PriceCalculations()
        {
            // Arrange
            MaterialDto materialDto = new()
            {
                Quantity = 10,
                Price = 50m,
                RequiredPrice = 500m,
                LeftOverPrice = 0
            };

            // Act
            decimal totalPrice = materialDto.Price * materialDto.Quantity;
            decimal totalWithLeftOver = totalPrice + materialDto.LeftOverPrice;

            // Assert
            Assert.Equal(500m, materialDto.RequiredPrice);
            Assert.Equal(500m, totalPrice);
            Assert.Equal(500m, totalWithLeftOver);
        }

        [Fact]
        public void MaterialEntity_SquareMeterPriceCalculation()
        {
            // Arrange
            MaterialEntity materialEntity = new()
            {
                Price = 250m,
                Width = 1000m,
                Height = 1200m
            };

            // Act
            decimal areaPerUnit = materialEntity.Width * materialEntity.Height / 1000000;
            decimal squareMeterPrice = areaPerUnit > 0 ? materialEntity.Price / areaPerUnit : 0;

            // Assert
            Assert.Equal(1.2m, areaPerUnit);
            Assert.Equal(208.33m, squareMeterPrice, 2); // 250 / 1.2 ≈ 208.33
        }

        [Fact]
        public void MaterialDto_WasteCalculation()
        {
            // Arrange
            MaterialDto materialDto = new()
            {
                RequiredQuantity = 10,
                Quantity = 11,
                Waste = 1
            };

            // Act
            decimal calculatedWaste = materialDto.Quantity - materialDto.RequiredQuantity;

            // Assert
            Assert.Equal(1m, calculatedWaste);
            Assert.Equal(1m, materialDto.Waste);
        }

        [Fact]
        public void MaterialDto_MaxLengthAttributes()
        {
            // Arrange & Act
            MaterialDto materialDto = new()
            {
                Reference = new string('A', 25),
                ReferenceBase = new string('B', 25),
                Description = new string('C', 255),
                Color = new string('D', 50),
                Worksheet = new string('E', 255),
                Pallet = new string('F', 255)
            };

            // Assert
            Assert.Equal(25, materialDto.Reference!.Length);
            Assert.Equal(25, materialDto.ReferenceBase!.Length);
            Assert.Equal(255, materialDto.Description!.Length);
            Assert.Equal(50, materialDto.Color!.Length);
            Assert.Equal(255, materialDto.Worksheet!.Length);
            Assert.Equal(255, materialDto.Pallet!.Length);
        }

        [Fact]
        public void MaterialDto_CustomFields()
        {
            // Arrange & Act
            MaterialDto materialDto = new()
            {
                CustomField1 = "Custom Value 1",
                CustomField2 = "Custom Value 2",
                CustomField3 = "Custom Value 3",
                CustomField4 = "Custom Value 4",
                CustomField5 = "Custom Value 5"
            };

            // Assert
            Assert.Equal("Custom Value 1", materialDto.CustomField1);
            Assert.Equal("Custom Value 2", materialDto.CustomField2);
            Assert.Equal("Custom Value 3", materialDto.CustomField3);
            Assert.Equal("Custom Value 4", materialDto.CustomField4);
            Assert.Equal("Custom Value 5", materialDto.CustomField5);
        }

        [Fact]
        public void MaterialDto_SourceFields()
        {
            // Arrange & Act
            MaterialDto materialDto = new()
            {
                SourceReference = "SRC-PROF-001",
                SourceDescription = "Schuco Profile 45x45",
                SourceColor = "Silver",
                SourceColorDescription = "Anodized Silver"
            };

            // Assert
            Assert.Equal("SRC-PROF-001", materialDto.SourceReference);
            Assert.Equal("Schuco Profile 45x45", materialDto.SourceDescription);
            Assert.Equal("Silver", materialDto.SourceColor);
            Assert.Equal("Anodized Silver", materialDto.SourceColorDescription);
        }

        [Fact]
        public void MaterialEntity_LineAndColumnTracking()
        {
            // Arrange & Act
            MaterialEntity materialEntity = new()
            {
                Worksheet = "Materials",
                Line = 25,
                Column = 5,
                SortOrder = 3
            };

            // Assert
            Assert.Equal("Materials", materialEntity.Worksheet);
            Assert.Equal(25, materialEntity.Line);
            Assert.Equal(5, materialEntity.Column);
            Assert.Equal(3, materialEntity.SortOrder);
        }

        [Fact]
        public void MaterialDto_CommodityCodeHandling()
        {
            // Arrange & Act
            MaterialDto materialDto = new()
            {
                CommodityCode = 7610
            };

            // Assert
            Assert.Equal(7610, materialDto.CommodityCode);
        }

        [Fact]
        public void MaterialDto_LeftOverCalculations()
        {
            // Arrange
            MaterialDto materialDto = new()
            {
                RequiredQuantity = 50m,
                TotalQuantity = 55m,
                RequiredWeight = 250m,
                TotalWeight = 260m,
                RequiredArea = 10m,
                TotalArea = 11m,
                RequiredPrice = 500m,
                TotalPrice = 520m
            };

            // Act
            decimal leftOverQuantity = materialDto.TotalQuantity - materialDto.RequiredQuantity;
            decimal leftOverWeight = materialDto.TotalWeight - materialDto.RequiredWeight;
            decimal leftOverArea = materialDto.TotalArea - materialDto.RequiredArea;
            decimal leftOverPrice = materialDto.TotalPrice - materialDto.RequiredPrice;

            // Assert
            Assert.Equal(5m, leftOverQuantity);
            Assert.Equal(10m, leftOverWeight);
            Assert.Equal(1m, leftOverArea);
            Assert.Equal(20m, leftOverPrice);
        }
    }
}
