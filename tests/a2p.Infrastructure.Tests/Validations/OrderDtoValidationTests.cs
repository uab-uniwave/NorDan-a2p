using Application.DTOs;
using Application.Models;
using Application.Validations;

using Domain.Enums;

using FluentValidation.Results;

using Xunit;

namespace Infrastructure.Tests.Validations

{
    /// <summary>
    /// Tests for OrderDto validation rules
    /// </summary>
    public class OrderDtoValidationTests
    {
        [Fact]
        public void OrderDtoValidator_WithValidOrder_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-2024-001",
                ProjectNumber = "PROJ-001",
                OrderDate = DateTime.Now,
                CustomerTitle = "Mr.",
                CustomerNumber = "CUST-001",
                DeliveryAddress = "123 Main St",
                ResponsibleManager = "John Doe",
                Currency = "EUR",
                ExchangeRate = 1.0m,
                Import = true,
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_WithNullOrderNumber_ShouldFail()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = null, // Invalid: required
                ProjectNumber = "PROJ-001",
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(OrderDto.OrderNumber));
        }

        [Fact]
        public void OrderDtoValidator_WithEmptyOrderNumber_ShouldFail()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = string.Empty, // Invalid: empty
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_WithNegativeExchangeRate_ShouldFail()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                ExchangeRate = -1.0m, // Invalid: negative
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_WithZeroExchangeRate_ShouldFail()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                ExchangeRate = 0m, // Invalid: zero
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_WithValidExchangeRates_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            decimal[] validRates = new[] { 0.5m, 1.0m, 1.5m, 2.0m, 10.0m };

            // Act & Assert
            foreach (decimal rate in validRates)
            {
                OrderDto orderDto = new()
                {
                    Id = Guid.NewGuid(),
                    OrderNumber = "ORD-001",
                    ExchangeRate = rate,
                    ItemsDto = [],
                    MaterialsDto = [],
                    ExcelFiles = []
                };

                ValidationResult result = validator.Validate(orderDto);
                Assert.True(result.IsValid, $"Exchange rate {rate} should be valid");
            }
        }

        [Fact]
        public void OrderDtoValidator_WithNullOrderDate_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                OrderDate = null, // Optional field
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_WithOptionalFields_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                ProjectNumber = null, // Optional
                CustomerTitle = null, // Optional
                CustomerNumber = null, // Optional
                DeliveryAddress = null, // Optional
                ResponsibleManager = null, // Optional
                Currency = null, // Optional
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_WithEmptyId_ShouldFail()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.Empty, // Invalid: empty guid
                OrderNumber = "ORD-001",
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_WithValidId_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            Guid orderId = Guid.NewGuid();
            OrderDto orderDto = new()
            {
                Id = orderId, // Valid: non-empty guid
                OrderNumber = "ORD-001",
                Currency = "EUR",
                ExchangeRate = 1.0m,
                SalesDocument = new SalesDocument { Number = 1, Version = 0 },
                ItemsDto = new List<ItemDto>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderId,
                        ItemName = "Item 1",
                        Width = 10,
                        Height = 20,
                        Weight = 5,
                        TotalWeight = 5,
                        WorksheetType = Domain.Enums.WorksheetType.Items
                    }
                },
                MaterialsDto = new List<MaterialDto>
                {
                    new()
                    {
                        OrderId = orderId,
                        ReferenceBase = "REF-BASE",
                        Reference = "MAT-001",
                        Color = "Red",
                        MaterialType = MaterialType.Piece,
                        WorksheetType = WorksheetType.Materials
                    }
                },
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_ImportFlagTrue_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                Import = true, // Valid
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_ImportFlagFalse_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                Import = false, // Valid
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_DeleteExistingFlagTrue_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                DeleteExistsing = true, // Valid
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_DeleteExistingFlagFalse_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                DeleteExistsing = false, // Valid
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_WithItemsAndMaterials_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            Guid orderId = Guid.NewGuid();
            OrderDto orderDto = new()
            {
                Id = orderId,
                OrderNumber = "ORD-001",
                ItemsDto = new List<ItemDto>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderId,
                        ItemName = "Item 1",
                        Width = 10,
                        Height = 20,
                        Weight = 5,
                        TotalWeight = 5,
                        WorksheetType = Domain.Enums.WorksheetType.Items
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderId,
                        ItemName = "Item 2",
                        Width = 15,
                        Height = 25,
                        Weight = 8,
                        TotalWeight = 8,
                        WorksheetType = Domain.Enums.WorksheetType.Items
                    }
                },
                MaterialsDto = new List<MaterialDto>
                {
                    new()
                    {
                        OrderId = orderId,
                        ReferenceBase = "REF-BASE-1",
                        Reference = "MAT-001",
                        Color = "Red",
                        MaterialType = MaterialType.Piece,
                        WorksheetType = Domain.Enums.WorksheetType.Materials
                    },
                    new()
                    {
                        OrderId = orderId,
                        ReferenceBase = "REF-BASE-2",
                        Reference = "MAT-002",
                        Color = "Blue",
                        MaterialType = MaterialType.Gaskets,
                        WorksheetType = Domain.Enums.WorksheetType.Materials
                    },
                    new()
                    {
                        OrderId = orderId,
                        ReferenceBase = "REF-BASE-3",
                        Reference = "MAT-003",
                        Color = "Green",
                        MaterialType = MaterialType.Profiles,
                        WorksheetType = Domain.Enums.WorksheetType.Materials
                    }
                },
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_WithExcelFiles_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                ExcelFiles = new List<ExcelFile>
 {
  new() { FileName = "file1.xlsx", FilePath = "/path/file1.xlsx" },
  new() { FileName = "file2.xlsx", FilePath = "/path/file2.xlsx" }
 },
                ItemsDto = [],
                MaterialsDto = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_WithSalesDocument_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            Guid orderId = Guid.NewGuid();
            OrderDto orderDto = new()
            {
                Id = orderId,
                OrderNumber = "ORD-001",
                Currency = "EUR",
                ExchangeRate = 1.0m,
                SalesDocument = new SalesDocument
                {
                    Number = 12345,
                    Version = 1,
                    RowId = Guid.NewGuid()
                },
                ItemsDto = new List<ItemDto>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderId,
                        ItemName = "Item 1",
                        Width = 10,
                        Height = 20,
                        Weight = 5,
                        TotalWeight = 5,
                        WorksheetType = Domain.Enums.WorksheetType.Items
                    }
                },
                MaterialsDto = new List<MaterialDto>
                {
                    new()
                    {
                        OrderId = orderId,
                        ReferenceBase = "REF-BASE-1",
                        Reference = "MAT-001",
                        Color = "Red",
                        MaterialType = MaterialType.Piece,
                        WorksheetType = Domain.Enums.WorksheetType.Materials
                    }
                },
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_WithCurrencyCodes_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            string[] currencyCodes = new[] { "EUR", "USD", "GBP", "CHF", "SEK", "NOK", "DKK" };

            // Act & Assert
            foreach (string? currency in currencyCodes)
            {
                OrderDto orderDto = new()
                {
                    Id = Guid.NewGuid(),
                    OrderNumber = "ORD-001",
                    Currency = currency,
                    ItemsDto = [],
                    MaterialsDto = [],
                    ExcelFiles = []
                };

                ValidationResult result = validator.Validate(orderDto);
                Assert.True(result.IsValid, $"Currency {currency} should be valid");
            }
        }

        [Fact]
        public void OrderDtoValidator_OrderNumberWithSpecialCharacters_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            Guid orderId = Guid.NewGuid();
            OrderDto orderDto = new()
            {
                Id = orderId,
                OrderNumber = "ORD-2024-001-A/B", // With special characters
                Currency = "USD",
                SalesDocument = new SalesDocument { Number = 1, Version = 0 },
                ExchangeRate = 1.0m,
                ItemsDto = new List<ItemDto>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderId,
                        ItemName = "Item 1",
                        Width = 10,
                        Height = 20,
                        Weight = 5,
                        TotalWeight = 5,
                        WorksheetType = Domain.Enums.WorksheetType.Items
                    }
                },
                MaterialsDto = new List<MaterialDto>
                {
                    new()
                    {
                        OrderId = orderId,
                        ReferenceBase = "REF-BASE",
                        Reference = "REF-001",
                        Color = "Red",
                        MaterialType = MaterialType.Glasses ,
                        WorksheetType = WorksheetType.Materials,
                        Quantity = 1,
                        RequiredQuantity = 1m
                    }
                },
                ExcelFiles = []
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderDtoValidator_ComplexOrder_AllFieldsPopulated_ShouldPass()
        {
            // Arrange
            OrderDtoValidator validator = new();
            DateTime now = DateTime.Now;
            OrderDto orderDto = new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-COMPLEX-2024",
                ProjectNumber = "PROJ-COMPLEX",
                OrderDate = now,
                CustomerTitle = "Dr.",
                CustomerNumber = "CUST-99999",
                DeliveryAddress = "999 Commerce Blvd, Tech City",
                CorrectionAvailableUntil = now.AddDays(30),
                ResponsibleManager = "Manager Name",
                Currency = "EUR",
                ExchangeRate = 1.05m,
                ExchangeRateDate = now,
                Import = true,
                DeleteExistsing = false,
                ItemsDto = new List<ItemDto>
                 {
                  new() { ItemName = "Item 1", Quantity = 5 }
                 },
                MaterialsDto = new List<MaterialDto>
                 {
                  new() { Reference = "MAT-1", Quantity = 10 }
                 },
                ExcelFiles = new List<ExcelFile>
                 {
                  new() { FileName = "data.xlsx", FilePath = "/path/data.xlsx" }
                     },
                SalesDocument = new SalesDocument { Number = 12345, Version = 1 }
            };

            // Act
            ValidationResult result = validator.Validate(orderDto);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
