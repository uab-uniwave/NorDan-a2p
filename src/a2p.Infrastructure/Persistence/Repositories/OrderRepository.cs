using Application.Interfaces.Repositories;

using Dapper;

using Domain.Entities;

using Infrastructure.Data;

using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperService _dapper;
        private readonly ILogger<OrderRepository> _logger;
        private readonly IItemRepository _itemRepository;
        private readonly IMaterialRepository _materialRepository;

        public OrderRepository(DapperService dapper, ILogger<OrderRepository> logger, IItemRepository itemRepository, IMaterialRepository materialRepository)
        {
            _dapper = dapper;
            _logger = logger;
            _itemRepository = itemRepository;
            _materialRepository = materialRepository;
        }

        // CREATE
        public async Task<OrderEntity?> CreateOrderAsync(OrderEntity order)
        {

            List<ItemEntity> resultItems = new();
            List<MaterialEntity> resultMeterials = new();
            const string sql = @"INSERT INTO Uniwave_a2p_Orders 
 ([Id]
 ,[OrderNumber]
 ,[ProjectNumber]
 ,[SalesDocumentNumber]
 ,[SalesDocumentVersion]
 ,[OrderDate]
 ,[CustomerTitle]
 ,[CustomerNumber]
 ,[DeliveryAddress]
 ,[CorrectionAvailableUntil]
 ,[ResponsibleManager]
 ,[Currency]
 ,[ExchangeRate]
 ,[ExchangeRateDate]
 ,[CreatedUTCDateTime]
 ,[ModifiedUTCDateTime]
 ,[CreatedBy]
 ,[ModifiedBy])
 OUTPUT INSERTED.*
 VALUES 
 (@Id
 ,@OrderNumber
 ,@ProjectNumber
 ,@SalesDocumentNumber
 ,@SalesDocumentVersion
 ,@OrderDate
 ,@CustomerTitle
 ,@CustomerNumber
 ,@DeliveryAddress
 ,@CorrectionAvailableUntil
 ,@ResponsibleManager
 ,@Currency
 ,@ExchangeRate
 ,@ExchangeRateDate
 ,@CreatedUTCDateTime
 ,@ModifiedUTCDateTime
 ,@CreatedBy
 ,@ModifiedBy)";

            foreach (ItemEntity item in order.Items)
            {
                item.OrderId = order.Id;
                ItemEntity? resultItem = await _itemRepository.CreateItemAsync(item);
                if (resultItem != null)
                {
                    resultItems.Add(resultItem);
                }
            }

            foreach (MaterialEntity material in order.Materials)
            {
                material.OrderId = order.Id;
                MaterialEntity? resultMaterial = await _materialRepository.CreateMaterialAsync(material);
                if (resultMaterial != null)
                {
                    resultMeterials.Add(resultMaterial);
                }
            }

            OrderEntity? resultOrder = await _dapper.QuerySingleOrDefaultAsync<OrderEntity>(sql, order);
            if (resultOrder != null)
            {
                resultOrder.Items = resultItems;
                resultOrder.Materials = resultMeterials;

            }
            return resultOrder;
        }
        // READ BY ID
        public async Task<OrderEntity?> GetOrderAsync(Guid id)
        {
            const string sql = "SELECT * FROM Uniwave_a2p_Orders WHERE Id = @id;";

            OrderEntity? order = await _dapper.QuerySingleOrDefaultAsync<OrderEntity>(sql, new { Id = id });

            if (order != null)
            {
                IEnumerable<ItemEntity> items = await _itemRepository.GetOrderItemsAsync(order.Id);
                order.Items = items.ToList();
                IEnumerable<MaterialEntity> materials = await _materialRepository.GetOrderMaterialsAsync(order.Id);
                order.Items = items.ToList();
            }

            return order;
        }

        // PAGED READ
        public async Task<(IEnumerable<OrderEntity> Orders, int TotalCount)> GetOrdersAsync(int page, int size)
        {
            const string sql = @"
 SELECT * FROM Uniwave_a2p_Orders
 ORDER BY OrderNumber DESC, SortOrder 
 OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
 SELECT COUNT(*) FROM Uniwave_a2p_Order;";

            SqlMapper.GridReader multi = await _dapper.QueryMultipleAsync(sql, new { Offset = (page - 1) * size, PageSize = size });
            IEnumerable<OrderEntity> orders = await multi.ReadAsync<OrderEntity>();
            int total = await multi.ReadSingleAsync<int>();
            return (orders, total);
        }

        // UPDATE ALL ORDER DETAILS
        public async Task<int> UpdateOrderAsync(OrderEntity order)
        {
            List<ItemEntity> resultItems = new();
            List<MaterialEntity> resultMeterials = new();
            const string sql = @"INSERT INTO Uniwave_a2p_Orders 
 ([Id]
 , [OrderNumber]
 , [ProjectNumber]
 , [SalesDocumentNumber]
 , [SalesDocumentVersion]
 , [OrderDate]
 , [CustomerTitle]
 , [CustomerNumber]
 , [DeliveryAddress]
 , [CorrectionAvailableUntil]
 , [ResponsibleManager]
 , [Currency]
 , [ExchangeRate]
 , [ExchangeRateDate]
 , [CreatedUTCDateTime]
 , [ModifiedUTCDateTime]
 , [CreatedBy]
 , [ModifiedBy])
 OUTPUT UNSERTED.*
 VALUES
 (@Id
 ,@OrderNumber
 , @ProjectNumber
 , @SalesDocumentNumber
 , @SalesDocumentVersion
 , @OrderDate
 , @CustomerTitle
 , @CustomerNumber
 , @DeliveryAddress
 , @CorrectionAvailableUntil
 , @ResponsibleManager
 , @Currency
 , @ExchangeRate
 , @ExchangeRateDate
 , @CreatedUTCDateTime
 , @ModifiedUTCDateTime
 , @CreatedBy
 , @ModifiedBy)";

            foreach (ItemEntity item in order.Items)
            {
                await _itemRepository.UpdateItemAsync(item);
            }

            foreach (MaterialEntity material in order.Materials)
            {
                await _materialRepository.UpdateMaterialAsync(material);
            }

            return await _dapper.ExecuteAsync(sql, order);
        }

        // UPDATE ALL ORDER DETAILS
        public async Task<int> UpdateOrderDeliveryAddressAsync(Guid id, string deliveryAddress)
        {
            const string sql = @"UPDATE Uniwave_a2p_Orders SET DeliveryAddress = @DeliveryAddress WHERE Id = @id;";

            return await _dapper.ExecuteAsync(sql, new { Id = id, DeliveryAddress = deliveryAddress });
        }

        // DELETE BY ID
        public async Task<int> DeleteOrderAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_Orders WHERE Id = @id;";

            return await _dapper.ExecuteAsync(sql, new { Id = id });
        }

    }
}