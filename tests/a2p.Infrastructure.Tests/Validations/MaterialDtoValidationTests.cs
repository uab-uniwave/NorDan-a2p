using Application.DTOs;
using Application.Validations;

using Domain.Enums;

using FluentValidation.Results;

using Xunit;

namespace Infrastructure.Tests.Validations
{
    /// <summary>
    /// Tests for MaterialDto validation rules
    /// </summary>
    public class MaterialDtoValidationTests
    {
        [Fact]
        public void MaterialDtoValidator_WithValidMaterial_ShouldPass()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                Reference = "PROF-001",
                ReferenceBase = "PROF",
                Description = "Aluminum Profile",
                Color = "Silver",
                Worksheet = "Sheet1",
                Line = 1,
                Column = 1,
                Quantity = 10,
                RequiredQuantity = 10,
                Width = 45m,
                Height = 45m,
                Weight = 2.5m,
                Price = 50m,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithNullReference_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = null,
                Quantity = 10,
                RequiredQuantity = 10,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(MaterialDto.Reference));
        }

        [Fact]
        public void MaterialDtoValidator_WithEmptyReference_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = string.Empty,
                Quantity = 10,
                RequiredQuantity = 10,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithReferenceExceedingMaxLength_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = new string('X', 26), // Exceeds max length of 25
                Quantity = 10,
                RequiredQuantity = 10,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithNegativeQuantity_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = "PROF-001",
                Quantity = -1, // Invalid: negative quantity
                RequiredQuantity = 10,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithZeroQuantity_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = "PROF-001",
                Quantity = 0, // Invalid: zero quantity
                RequiredQuantity = 10,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithZeroRequiredQuantity_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = "PROF-001",
                Quantity = 10,
                RequiredQuantity = 0, // Invalid: must be > 0
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithNegativePrice_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = "PROF-001",
                Quantity = 10,
                RequiredQuantity = 10,
                Price = -50m, // Invalid: negative price
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithNegativeWidth_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = "PROF-001",
                Quantity = 10,
                RequiredQuantity = 10,
                Width = -45m, // Invalid: negative width
                Height = 45m,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithNegativeHeight_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = "PROF-001",
                Quantity = 10,
                RequiredQuantity = 10,
                Width = 45m,
                Height = -45m, // Invalid: negative height
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithNegativeWeight_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = "PROF-001",
                Quantity = 10,
                RequiredQuantity = 10,
                Weight = -2.5m, // Invalid: negative weight
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithMaterialTypeUnknown_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = "PROF-001",
                Quantity = 10,
                RequiredQuantity = 10,
                MaterialType = MaterialType.Unknown, // Invalid: unknown type
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithWorksheetTypeUnknown_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = "PROF-001",
                Quantity = 10,
                RequiredQuantity = 10,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Unknown // Invalid: unknown type
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithNullOrderId_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = null, // Invalid: required
                Reference = "PROF-001",
                Quantity = 10,
                RequiredQuantity = 10,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithEmptyOrderId_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.Empty, // Invalid: empty guid
                Reference = "PROF-001",
                Quantity = 10,
                RequiredQuantity = 10,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithDescriptionExceedingMaxLength_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = "PROF-001",
                Description = new string('X', 256), // Exceeds max length of 255
                Quantity = 10,
                RequiredQuantity = 10,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithColorExceedingMaxLength_ShouldFail()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                Reference = "PROF-001",
                Color = new string('X', 51), // Exceeds max length of 50
                Quantity = 10,
                RequiredQuantity = 10,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void MaterialDtoValidator_WithValidOptionalFields_ShouldPass()
        {
            // Arrange
            MaterialDtoValidator validator = new();
            MaterialDto materialDto = new()
            {
                OrderId = Guid.NewGuid(),
                ItemId = Guid.NewGuid(), // Optional but valid
                Reference = "PROF-001",
                ReferenceBase = "PROF", // Optional but valid
                Description = "Profile Description",
                Color = "Silver",
                Quantity = 10,
                RequiredQuantity = 10,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials,
                CustomField1 = "Custom 1", // Optional custom fields
                CommodityCode = 7610 // Optional commodity code
            };

            // Act
            ValidationResult result = validator.Validate(materialDto);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
