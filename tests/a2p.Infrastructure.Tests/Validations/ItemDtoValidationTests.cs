using Application.DTOs;
using Application.Validations;

using Domain.Enums;

using FluentValidation.Results;

using Xunit;

namespace Infrastructure.Tests.Validations
{
    /// <summary>
    /// Tests for ItemDto validation rules
    /// </summary>
    public class ItemDtoValidationTests
    {
        [Fact]
        public void ItemDtoValidator_WithValidItem_ShouldPass()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = Guid.NewGuid(),
                ItemName = "Valid Item",
                Worksheet = "Sheet1",
                Line = 1,
                Column = 1,
                Quantity = 5,
                Width = 100m,
                Height = 200m,
                Weight = 50m,
                TotalWeight = 250m,
                Price = 150m,
                WorksheetType = WorksheetType.Items
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ItemDtoValidator_WithNullItemName_ShouldFail()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = Guid.NewGuid(),
                ItemName = null,
                Worksheet = "Sheet1"
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(ItemDto.ItemName));
        }

        [Fact]
        public void ItemDtoValidator_WithEmptyItemName_ShouldFail()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = Guid.NewGuid(),
                ItemName = string.Empty,
                Worksheet = "Sheet1"
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ItemDtoValidator_WithItemNameExceedingMaxLength_ShouldFail()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = Guid.NewGuid(),
                ItemName = new string('X', 51), // Exceeds max length of 50
                Worksheet = "Sheet1"
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ItemDtoValidator_WithNegativeQuantity_ShouldFail()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = Guid.NewGuid(),
                ItemName = "Valid Item",
                Worksheet = "Sheet1",
                Quantity = -1 // Invalid: negative quantity
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ItemDtoValidator_WithZeroQuantity_ShouldFail()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = Guid.NewGuid(),
                ItemName = "Valid Item",
                Worksheet = "Sheet1",
                Quantity = 0 // Invalid: zero quantity
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ItemDtoValidator_WithNegativePrice_ShouldFail()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = Guid.NewGuid(),
                ItemName = "Valid Item",
                Worksheet = "Sheet1",
                Quantity = 5,
                Price = -10m // Invalid: negative price
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ItemDtoValidator_WithNegativeDimensions_ShouldFail()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = Guid.NewGuid(),
                ItemName = "Valid Item",
                Worksheet = "Sheet1",
                Quantity = 5,
                Width = -100m, // Invalid: negative width
                Height = 200m
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ItemDtoValidator_WithZeroDimensions_ShouldFail()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = Guid.NewGuid(),
                ItemName = "Valid Item",
                Worksheet = "Sheet1",
                Quantity = 5,
                Width = 0m, // Invalid: zero width
                Height = 200m
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ItemDtoValidator_WithWorksheetTypeUnknown_ShouldFail()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = Guid.NewGuid(),
                ItemName = "Valid Item",
                Worksheet = "Sheet1",
                Quantity = 5,
                Width = 100m,
                Height = 200m,
                WorksheetType = WorksheetType.Unknown // Invalid: unknown type
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ItemDtoValidator_WithNullOrderId_ShouldFail()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = null, // Invalid: required
                ItemName = "Valid Item",
                Worksheet = "Sheet1"
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ItemDtoValidator_WithEmptyOrderId_ShouldFail()
        {
            // Arrange
            ItemDtoValidator validator = new();
            ItemDto itemDto = new()
            {
                OrderId = Guid.Empty, // Invalid: empty guid
                ItemName = "Valid Item",
                Worksheet = "Sheet1"
            };

            // Act
            ValidationResult result = validator.Validate(itemDto);

            // Assert
            Assert.False(result.IsValid);
        }
    }
}
