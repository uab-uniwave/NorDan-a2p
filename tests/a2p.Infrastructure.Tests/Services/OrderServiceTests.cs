using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Models;

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
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IValidator<OrderDto>> _mockOrderValidator;
        private readonly Mock<IValidator<ItemDto>> _mockItemValidator;
        private readonly Mock<IValidator<MaterialDto>> _mockMaterialValidator;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<OrderService>> _mockLogger;
        private readonly IOrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderValidator = new Mock<IValidator<OrderDto>>();
            _mockItemValidator = new Mock<IValidator<ItemDto>>();
            _mockMaterialValidator = new Mock<IValidator<MaterialDto>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<OrderService>>();

            _orderService = new OrderService(
            _mockOrderRepository.Object,
            _mockOrderValidator.Object,
            _mockItemValidator.Object,
            _mockMaterialValidator.Object,
            _mockMapper.Object,
            _mockLogger.Object);
        }

        #region CreateOrderAsync Tests

        [Fact]
        public async Task CreateOrderAsync_WithValidDto_ReturnsSuccessResult()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDto();
            OrderEntity orderEntity = CreateValidOrderEntity();

            _mockOrderValidator
            .Setup(v => v.ValidateAsync(orderDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<OrderEntity>(orderDto))
            .Returns(orderEntity);

            _mockMapper
            .Setup(m => m.Map<ItemEntity>(It.IsAny<ItemDto>()))
            .Returns(new ItemEntity());

            _mockMapper
            .Setup(m => m.Map<MaterialEntity>(It.IsAny<MaterialDto>()))
            .Returns(new MaterialEntity());

            _mockOrderRepository
            .Setup(r => r.CreateOrderAsync(It.IsAny<OrderEntity>()))
            .ReturnsAsync(orderEntity);

            // Act
            ValidationResult<OrderEntity> result = await _orderService.CreateOrderAsync(orderDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("ORD-2024-001", result.Value.OrderNumber);
            _mockOrderRepository.Verify(r => r.CreateOrderAsync(It.IsAny<OrderEntity>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_WithInvalidDto_ReturnsFailureResult()
        {
            // Arrange
            OrderDto orderDto = CreateInvalidOrderDto();
            ValidationFailure validationFailure = new("OrderNumber", "OrderNumber is required");

            _mockOrderValidator
            .Setup(v => v.ValidateAsync(orderDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { validationFailure }));

            // Act
            ValidationResult<OrderEntity> result = await _orderService.CreateOrderAsync(orderDto);

            // Assert
            Assert.False(result.IsSuccess);
            _mockOrderRepository.Verify(r => r.CreateOrderAsync(It.IsAny<OrderEntity>()), Times.Never);
        }

        [Fact]
        public async Task CreateOrderAsync_WithItemsAndMaterials_IncludesNestedEntities()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDtoWithItemsAndMaterials(2, 3);
            OrderEntity orderEntity = CreateValidOrderEntity();
            ItemEntity itemEntity = new() { Id = Guid.NewGuid() };
            MaterialEntity materialEntity = new() { Id = Guid.NewGuid() };

            _mockOrderValidator
            .Setup(v => v.ValidateAsync(orderDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<OrderEntity>(orderDto))
            .Returns(orderEntity);

            _mockMapper
            .Setup(m => m.Map<ItemEntity>(It.IsAny<ItemDto>()))
            .Returns(itemEntity);

            _mockMapper
            .Setup(m => m.Map<MaterialEntity>(It.IsAny<MaterialDto>()))
            .Returns(materialEntity);

            _mockOrderRepository
            .Setup(r => r.CreateOrderAsync(It.IsAny<OrderEntity>()))
            .ReturnsAsync(orderEntity);

            // Act
            ValidationResult<OrderEntity> result = await _orderService.CreateOrderAsync(orderDto);

            // Assert
            Assert.True(result.IsSuccess);
            // Verify mapper was called for items and materials
            _mockMapper.Verify(m => m.Map<ItemEntity>(It.IsAny<ItemDto>()), Times.Exactly(2));
            _mockMapper.Verify(m => m.Map<MaterialEntity>(It.IsAny<MaterialDto>()), Times.Exactly(3));
        }

        [Fact]
        public async Task CreateOrderAsync_WhenMapperReturnsNull_ThrowsInvalidOperationException()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDto();

            _mockOrderValidator
            .Setup(v => v.ValidateAsync(orderDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<OrderEntity>(orderDto))
            .Returns((OrderEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
            () => _orderService.CreateOrderAsync(orderDto));
        }

        [Fact]
        public async Task CreateOrderAsync_WhenRepositoryReturnsNull_ReturnsFailureResult()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDto();
            OrderEntity orderEntity = CreateValidOrderEntity();

            _mockOrderValidator
            .Setup(v => v.ValidateAsync(orderDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<OrderEntity>(orderDto))
            .Returns(orderEntity);

            _mockMapper
            .Setup(m => m.Map<ItemEntity>(It.IsAny<ItemDto>()))
            .Returns(new ItemEntity());

            _mockMapper
            .Setup(m => m.Map<MaterialEntity>(It.IsAny<MaterialDto>()))
            .Returns(new MaterialEntity());

            _mockOrderRepository
            .Setup(r => r.CreateOrderAsync(It.IsAny<OrderEntity>()))
            .ReturnsAsync((OrderEntity?)null);

            // Act
            ValidationResult<OrderEntity> result = await _orderService.CreateOrderAsync(orderDto);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region UpdateOrderAsync Tests

        [Fact]
        public async Task UpdateOrderAsync_WithValidDto_ReturnsSuccessResult()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDto();
            OrderEntity orderEntity = CreateValidOrderEntity();
            OrderEntity existingOrder = CreateValidOrderEntity();

            _mockOrderValidator
            .Setup(v => v.ValidateAsync(orderDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<OrderEntity>(orderDto))
            .Returns(orderEntity);

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderEntity.Id))
            .ReturnsAsync(existingOrder);

            _mockOrderRepository
            .Setup(r => r.UpdateOrderAsync(It.IsAny<OrderEntity>()))
            .ReturnsAsync(1);

            // Act
            ValidationResult<OrderEntity> result = await _orderService.UpdateOrderAsync(orderDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            _mockOrderRepository.Verify(r => r.UpdateOrderAsync(It.IsAny<OrderEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_WithNonExistentId_ReturnsFailureResult()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDto();
            OrderEntity orderEntity = CreateValidOrderEntity();

            _mockOrderValidator
            .Setup(v => v.ValidateAsync(orderDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<OrderEntity>(orderDto))
            .Returns(orderEntity);

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderEntity.Id))
            .ReturnsAsync((OrderEntity?)null);

            // Act
            ValidationResult<OrderEntity> result = await _orderService.UpdateOrderAsync(orderDto);

            // Assert
            Assert.False(result.IsSuccess);
            _mockOrderRepository.Verify(r => r.UpdateOrderAsync(It.IsAny<OrderEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateOrderAsync_WithInvalidDto_ReturnsFailureResult()
        {
            // Arrange
            OrderDto orderDto = CreateInvalidOrderDto();
            ValidationFailure validationFailure = new("OrderNumber", "OrderNumber is required");

            _mockOrderValidator
            .Setup(v => v.ValidateAsync(orderDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { validationFailure }));

            // Act
            ValidationResult<OrderEntity> result = await _orderService.UpdateOrderAsync(orderDto);

            // Assert
            Assert.False(result.IsSuccess);
            _mockOrderRepository.Verify(r => r.UpdateOrderAsync(It.IsAny<OrderEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateOrderAsync_WhenRepositoryUpdateFails_ReturnsFailureResult()
        {
            // Arrange
            OrderDto orderDto = CreateValidOrderDto();
            OrderEntity orderEntity = CreateValidOrderEntity();
            OrderEntity existingOrder = CreateValidOrderEntity();

            _mockOrderValidator
            .Setup(v => v.ValidateAsync(orderDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

            _mockMapper
            .Setup(m => m.Map<OrderEntity>(orderDto))
            .Returns(orderEntity);

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderEntity.Id))
            .ReturnsAsync(existingOrder);

            _mockOrderRepository
            .Setup(r => r.UpdateOrderAsync(It.IsAny<OrderEntity>()))
            .ReturnsAsync(0);

            // Act
            ValidationResult<OrderEntity> result = await _orderService.UpdateOrderAsync(orderDto);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region GetOrderByIdAsync Tests

        [Fact]
        public async Task GetOrderByIdAsync_WithValidId_ReturnsSuccessResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            OrderEntity orderEntity = CreateValidOrderEntity();
            orderEntity.Id = orderId;

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderId))
            .ReturnsAsync(orderEntity);

            // Act
            Result<OrderEntity> result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(orderId, result.Value.Id);
        }

        [Fact]
        public async Task GetOrderByIdAsync_WithNonExistentId_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderId))
            .ReturnsAsync((OrderEntity?)null);

            // Act
            Result<OrderEntity> result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetOrderByIdAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderId))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            Result<OrderEntity> result = await _orderService.GetOrderByIdAsync(orderId);

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

        #region GetOrdersAsync (Paged) Tests

        [Fact]
        public async Task GetOrdersAsync_WithValidPageParameters_ReturnsPagedResult()
        {
            // Arrange
            int page = 1;
            int size = 10;
            List<OrderEntity> orders = new()
 {
 CreateValidOrderEntity(),
 CreateValidOrderEntity(),
 CreateValidOrderEntity()
 };
            int totalCount = 25;

            _mockOrderRepository
            .Setup(r => r.GetOrdersAsync(page, size))
            .ReturnsAsync((orders, totalCount));

            // Act
            PagedResult<OrderEntity> result = await _orderService.GetOrdersAsync(page, size);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Count());
            Assert.Equal(25, result.TotalCount);
            Assert.Equal(1, result.PageIndex);
            Assert.Equal(10, result.PageSize);
        }

        [Fact]
        public async Task GetOrdersAsync_WithDifferentPages_ReturnsCorrectPages()
        {
            // Arrange
            int page = 2;
            int size = 5;
            List<OrderEntity> orders = new()
 { CreateValidOrderEntity() };
            int totalCount = 10;

            _mockOrderRepository
            .Setup(r => r.GetOrdersAsync(page, size))
            .ReturnsAsync((orders, totalCount));

            // Act
            PagedResult<OrderEntity> result = await _orderService.GetOrdersAsync(page, size);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.PageIndex);
            Assert.Equal(5, result.PageSize);
        }

        [Fact]
        public async Task GetOrdersAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            int page = 1;
            int size = 10;

            _mockOrderRepository
            .Setup(r => r.GetOrdersAsync(page, size))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            PagedResult<OrderEntity> result = await _orderService.GetOrdersAsync(page, size);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region UpdateOrderDeliveryAddressAsync Tests

        [Fact]
        public async Task UpdateOrderDeliveryAddressAsync_WithValidData_ReturnsSuccessResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            string newAddress = "New Delivery Address";
            OrderEntity existingOrder = CreateValidOrderEntity();

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderId))
            .ReturnsAsync(existingOrder);

            _mockOrderRepository
            .Setup(r => r.UpdateOrderDeliveryAddressAsync(orderId, newAddress))
            .ReturnsAsync(1);

            // Act
            Result<bool> result = await _orderService.UpdateOrderDeliveryAddressAsync(orderId, newAddress);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            _mockOrderRepository.Verify(
            r => r.UpdateOrderDeliveryAddressAsync(orderId, newAddress),
            Times.Once);
        }

        [Fact]
        public async Task UpdateOrderDeliveryAddressAsync_WithNonExistentId_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            string newAddress = "New Address";

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderId))
            .ReturnsAsync((OrderEntity?)null);

            // Act
            Result<bool> result = await _orderService.UpdateOrderDeliveryAddressAsync(orderId, newAddress);

            // Assert
            Assert.False(result.IsSuccess);
            _mockOrderRepository.Verify(
            r => r.UpdateOrderDeliveryAddressAsync(It.IsAny<Guid>(), It.IsAny<string>()),
            Times.Never);
        }

        [Fact]
        public async Task UpdateOrderDeliveryAddressAsync_WhenUpdateFails_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            string newAddress = "New Address";
            OrderEntity existingOrder = CreateValidOrderEntity();

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderId))
            .ReturnsAsync(existingOrder);

            _mockOrderRepository
            .Setup(r => r.UpdateOrderDeliveryAddressAsync(orderId, newAddress))
            .ReturnsAsync(0);

            // Act
            Result<bool> result = await _orderService.UpdateOrderDeliveryAddressAsync(orderId, newAddress);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateOrderDeliveryAddressAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            string newAddress = "New Address";

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderId))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            Result<bool> result = await _orderService.UpdateOrderDeliveryAddressAsync(orderId, newAddress);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region DeleteOrderByIdAsync Tests

        [Fact]
        public async Task DeleteOrderByIdAsync_WithValidId_ReturnsSuccessResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            OrderEntity orderEntity = CreateValidOrderEntity();
            orderEntity.Id = orderId;

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderId))
            .ReturnsAsync(orderEntity);

            _mockOrderRepository
            .Setup(r => r.DeleteOrderAsync(orderId))
            .ReturnsAsync(1);

            // Act
            Result<bool> result = await _orderService.DeleteOrderByIdAsync(orderId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            _mockOrderRepository.Verify(r => r.DeleteOrderAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderByIdAsync_WithNonExistentId_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderId))
            .ReturnsAsync((OrderEntity?)null);

            // Act
            Result<bool> result = await _orderService.DeleteOrderByIdAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
            _mockOrderRepository.Verify(r => r.DeleteOrderAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOrderByIdAsync_WhenRepositoryDeleteFails_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            OrderEntity orderEntity = CreateValidOrderEntity();
            orderEntity.Id = orderId;

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderId))
            .ReturnsAsync(orderEntity);

            _mockOrderRepository
            .Setup(r => r.DeleteOrderAsync(orderId))
            .ReturnsAsync(0);

            // Act
            Result<bool> result = await _orderService.DeleteOrderByIdAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteOrderByIdAsync_WhenExceptionThrown_ReturnsFailureResult()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            _mockOrderRepository
            .Setup(r => r.GetOrderAsync(orderId))
            .ThrowsAsync(new Exception("Database error"));

            // Act
            Result<bool> result = await _orderService.DeleteOrderByIdAsync(orderId);

            // Assert
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region Helper Methods

        private OrderDto CreateValidOrderDto()
        {
            return new OrderDto
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-2024-001",
                ProjectNumber = "PROJ-001",
                OrderDate = DateTime.Now,
                CustomerTitle = "Mr.",
                CustomerNumber = "CUST-001",
                DeliveryAddress = "123 Main St, City, Country",
                CorrectionAvailableUntil = DateTime.Now.AddDays(7),
                ResponsibleManager = "John Doe",
                Currency = "EUR",
                ExchangeRate = 1.0m,
                ExchangeRateDate = DateTime.Now,
                Import = true,
                DeleteExistsing = false,
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = [],
                SalesDocument = new SalesDocument()
            };
        }

        private OrderDto CreateValidOrderDtoWithItemsAndMaterials(int itemCount, int materialCount)
        {
            OrderDto dto = CreateValidOrderDto();

            for (int i = 0; i < itemCount; i++)
            {
                dto.ItemsDto.Add(CreateValidItemDto());
            }

            for (int i = 0; i < materialCount; i++)
            {
                dto.MaterialsDto.Add(CreateValidMaterialDto());
            }

            return dto;
        }

        private OrderDto CreateInvalidOrderDto()
        {
            return new OrderDto
            {
                Id = Guid.NewGuid(),
                OrderNumber = null, // Invalid: required
                ItemsDto = [],
                MaterialsDto = [],
                ExcelFiles = []
            };
        }

        private OrderEntity CreateValidOrderEntity()
        {
            return new OrderEntity
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-2024-001",
                ProjectNumber = "PROJ-001",
                SalesDocumentNumber = 12345,
                SalesDocumentVersion = 1,
                OrderDate = DateTime.Now,
                CustomerTitle = "Mr.",
                CustomerNumber = "CUST-001",
                DeliveryAddress = "123 Main St, City, Country",
                CorrectionAvailableUntil = DateTime.Now.AddDays(7),
                ResponsibleManager = "John Doe",
                SourceAppType = SourceAppType.Schuco,
                Currency = "EUR",
                ExchangeRate = 1.0,
                ExchangeRateDate = DateTime.Now,
                Items = [],
                Materials = [],
                CreatedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now
            };
        }

        private ItemDto CreateValidItemDto()
        {
            return new ItemDto
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ItemName = "Window Frame",
                Worksheet = "Sheet1",
                Line = 1,
                Column = 1,
                SortOrder = 1,
                Description = "Standard window frame",
                Quantity = 5,
                Width = 1500m,
                Height = 1000m,
                Weight = 10m,
                Area = 1.5m,
                Price = 250m
            };
        }

        private MaterialDto CreateValidMaterialDto()
        {
            return new MaterialDto
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                Reference = "PROFILE-001",
                ReferenceBase = "PROFILE",
                Description = "Aluminum Profile",
                Color = "Silver",
                Quantity = 10,
                RequiredQuantity = 10,
                Width = 45m,
                Height = 45m,
                Weight = 2.5m,
                Price = 50m,
                MaterialType = MaterialType.Profiles,
                WorksheetType = WorksheetType.Materials
            };
        }

        #endregion
    }
}
