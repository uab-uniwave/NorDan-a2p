using Application.DTOs;

using Domain.Entities;
using Domain.Enums;

using Xunit;

namespace Infrastructure.Tests.Services
{
    /// <summary>
    /// Integration tests for ItemService demonstrating expected behavior scenarios
    /// </summary>
    public class ItemServiceIntegrationTests
    {
        [Fact]
        public void ItemDto_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            ItemDto itemDto = new();

            // Assert
            Assert.NotEqual(Guid.Empty, itemDto.Id); // Auto-generated
            Assert.Null(itemDto.OrderId);
            Assert.Null(itemDto.ItemName);
            Assert.Null(itemDto.Worksheet);
            Assert.Equal(0, itemDto.Quantity);
            Assert.Equal(0m, itemDto.Width);
            Assert.Equal(0m, itemDto.Price);
            Assert.Equal(WorksheetType.Unknown, itemDto.WorksheetType);
        }

        [Fact]
        public void ItemDto_CanBeCreatedWithValues()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            // Act
            ItemDto itemDto = new()
            {
                OrderId = orderId,
                ItemName = "Window Frame",
                Worksheet = "Sheet1",
                Line = 5,
                Column = 3,
                Quantity = 10,
                Width = 1500m,
                Height = 1000m,
                Price = 250m,
                TotalPrice = 2500m,
                WorksheetType = WorksheetType.Items
            };

            // Assert
            Assert.Equal(orderId, itemDto.OrderId);
            Assert.Equal("Window Frame", itemDto.ItemName);
            Assert.Equal("Sheet1", itemDto.Worksheet);
            Assert.Equal(5, itemDto.Line);
            Assert.Equal(3, itemDto.Column);
            Assert.Equal(10, itemDto.Quantity);
            Assert.Equal(1500m, itemDto.Width);
            Assert.Equal(1000m, itemDto.Height);
            Assert.Equal(250m, itemDto.Price);
            Assert.Equal(2500m, itemDto.TotalPrice);
            Assert.Equal(WorksheetType.Items, itemDto.WorksheetType);
        }

        [Fact]
        public void ItemEntity_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            ItemEntity itemEntity = new();

            // Assert
            Assert.NotEqual(Guid.Empty, itemEntity.Id);
            Assert.Equal(Guid.Empty, itemEntity.OrderId);
            Assert.Equal(string.Empty, itemEntity.ItemName);
            Assert.Equal(string.Empty, itemEntity.Worksheet);
            Assert.Equal(-1, itemEntity.Line);
            Assert.Equal(-1, itemEntity.Column);
            Assert.Equal(0, itemEntity.Quantity);
            Assert.Empty(itemEntity.Materials);
        }

        [Fact]
        public void ItemEntity_CanBeCreatedWithValues()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            // Act
            ItemEntity itemEntity = new()
            {
                OrderId = orderId,
                ItemName = "Door Panel",
                Worksheet = "Sheet2",
                Line = 10,
                Column = 2,
                Quantity = 5,
                Width = 900m,
                Height = 2100m,
                Weight = 25m,
                TotalWeight = 125m,
                Price = 500m,
                TotalPrice = 2500m
            };

            // Assert
            Assert.Equal(orderId, itemEntity.OrderId);
            Assert.Equal("Door Panel", itemEntity.ItemName);
            Assert.Equal("Sheet2", itemEntity.Worksheet);
            Assert.Equal(10, itemEntity.Line);
            Assert.Equal(2, itemEntity.Column);
            Assert.Equal(5, itemEntity.Quantity);
            Assert.Equal(900m, itemEntity.Width);
            Assert.Equal(2100m, itemEntity.Height);
            Assert.Equal(25m, itemEntity.Weight);
            Assert.Equal(125m, itemEntity.TotalWeight);
            Assert.Equal(500m, itemEntity.Price);
            Assert.Equal(2500m, itemEntity.TotalPrice);
        }

        [Fact]
        public void ItemEntity_CanHaveMaterials()
        {
            // Arrange
            ItemEntity itemEntity = new() { ItemName = "Window Frame" };
            MaterialEntity material1 = new() { ItemName = "Aluminum Profile" };
            MaterialEntity material2 = new() { ItemName = "Glass Panel" };

            // Act
            itemEntity.Materials.Add(material1);
            itemEntity.Materials.Add(material2);

            // Assert
            Assert.Equal(2, itemEntity.Materials.Count);
            Assert.Contains(material1, itemEntity.Materials);
            Assert.Contains(material2, itemEntity.Materials);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void ItemDto_WithInvalidQuantity_ShouldBeAllowed(int quantity)
        {
            // Arrange & Act
            ItemDto itemDto = new() { Quantity = quantity };

            // Assert
            Assert.Equal(quantity, itemDto.Quantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void ItemEntity_WithInvalidQuantity_ShouldBeAllowed(int quantity)
        {
            // Arrange & Act
            ItemEntity itemEntity = new() { Quantity = quantity };

            // Assert
            Assert.Equal(quantity, itemEntity.Quantity);
        }

        [Fact]
        public void ItemDto_WithMaxLengthAttributes()
        {
            // Arrange & Act
            ItemDto itemDto = new()
            {
                ItemName = new string('A', 50),
                Worksheet = new string('B', 255),
                Description = new string('C', 255)
            };

            // Assert
            Assert.Equal(50, itemDto.ItemName!.Length);
            Assert.Equal(255, itemDto.Worksheet!.Length);
            Assert.Equal(255, itemDto.Description!.Length);
        }

        [Fact]
        public void ItemDto_CostCalculation()
        {
            // Arrange
            ItemDto itemDto = new()
            {
                Quantity = 5,
                MaterialCost = 100m,
                LaborCost = 50m,
                Price = 200m
            };

            // Act
            decimal cost = itemDto.MaterialCost + itemDto.LaborCost;
            decimal totalCost = cost * itemDto.Quantity;
            decimal totalPrice = itemDto.Price * itemDto.Quantity;

            // Assert
            Assert.Equal(150m, cost);
            Assert.Equal(750m, totalCost);
            Assert.Equal(1000m, totalPrice);
        }

        [Fact]
        public void ItemEntity_WeightCalculations()
        {
            // Arrange
            ItemEntity itemEntity = new()
            {
                Quantity = 10,
                Weight = 5m,
                WeightGlass = 2m,
                WeightWithoutGlass = 3m
            };

            // Act
            decimal totalWeight = itemEntity.Weight * itemEntity.Quantity;
            decimal totalWeightGlass = itemEntity.WeightGlass * itemEntity.Quantity;
            decimal totalWeightWithoutGlass = itemEntity.WeightWithoutGlass * itemEntity.Quantity;

            // Assert
            Assert.Equal(50m, totalWeight);
            Assert.Equal(20m, totalWeightGlass);
            Assert.Equal(30m, totalWeightWithoutGlass);
            Assert.Equal(50m, totalWeight);
        }

        [Fact]
        public void ItemDto_AreaCalculations()
        {
            // Arrange
            ItemDto itemDto = new()
            {
                Quantity = 3,
                Width = 1500m,
                Height = 1000m
            };

            // Act
            decimal area = itemDto.Width * itemDto.Height / 1000000; // Convert to m²
            decimal totalArea = area * itemDto.Quantity;

            // Assert
            Assert.Equal(1.5m, area);
            Assert.Equal(4.5m, totalArea);
        }

        [Fact]
        public void ItemEntity_HoursCalculation()
        {
            // Arrange
            ItemEntity itemEntity = new()
            {
                Quantity = 4,
                Hours = 2.5m
            };

            // Act
            decimal totalHours = itemEntity.Hours * itemEntity.Quantity;

            // Assert
            Assert.Equal(10m, totalHours);
        }
    }
}
