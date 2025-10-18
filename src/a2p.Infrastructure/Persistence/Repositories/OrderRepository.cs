using System.Data;

using a2p.Application.Interfaces.Repositories;
using a2p.Domain.Entities;

using Dapper;

namespace a2p.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnectionFactory _factory;

        public OrderRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        // CREATE
        public async Task<OrderEntity?> CreateOrderAsync(OrderEntity order)
        {
            const string sql = @"
                INSERT INTO Uniwave_a2p_Orders (
                    Id, OrderNumber, ProjectNumber, Number, Version, OrderDate,
                    CustomerTitle, CustomerNumber, DeliveryAddress, CorrectionAvailableUnitil,
                    ResponsibleManager, SourceAppType, ItemCount, OrderCount, ErrorCount, TotalQuantity,
                    TotalUnits, TotalWeight, TotalWeightWithoutGlass, TotalWeightGlass, TotalArea, TotalHours,
                    TotalOrderCost, TotalLaborCost, TotalCost, TotalPrice, Currency, ExchangeRate, ExchangeRateDate,
                    CreatedUTCDateTime, CreatedBy
                )
                OUTPUT INSERTED.*
                VALUES (
                    @Id, @OrderNumber, @ProjectNumber, @Number, @Version, @OrderDate,
                    @CustomerTitle, @CustomerNumber, @DeliveryAddress, @CorrectionAvailableUnitil,
                    @ResponsibleManager, @SourceAppType, @ItemCount, @OrderCount, @ErrorCount, @TotalQuantity,
                    @TotalUnits, @TotalWeight, @TotalWeightWithoutGlass, @TotalWeightGlass, @TotalArea, @TotalHours,
                    @TotalOrderCost, @TotalLaborCost, @TotalCost, @TotalPrice, @Currency, @ExchangeRate, @ExchangeRateDate,
                    @CreatedUTCDateTime, @CreatedBy
                );";

            using IDbConnection db = _factory.CreateConnection();
            return await db.QuerySingleOrDefaultAsync<OrderEntity>(sql, order);
        }

        // READ BY ID
        public async Task<OrderEntity?> GetOrderAsync(Guid id)
        {
            const string sql = "SELECT * FROM Uniwave_a2p_Orders WHERE Id = @id;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.QuerySingleOrDefaultAsync<OrderEntity>(sql, new { Id = id });
        }

        // PAGED READ
        public async Task<(IEnumerable<OrderEntity> Orders, int TotalCount)> GetOrdersAsync(int page, int size)
        {
            const string sql = @"
                SELECT * FROM Uniwave_a2p_Orders
                ORDER BY OrderNumber DESC, SortOrder 
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                SELECT COUNT(*) FROM Uniwave_a2p_Order;";
            using IDbConnection db = _factory.CreateConnection();
            using SqlMapper.GridReader multi = await db.QueryMultipleAsync(sql, new { Offset = (page - 1) * size, PageSize = size });
            IEnumerable<OrderEntity> orders = await multi.ReadAsync<OrderEntity>();
            var total = await multi.ReadSingleAsync<int>();
            return (orders, total);
        }

        // UPDATE ALL ORDER DETAILS
        public async Task<int> UpdateOrderAsync(OrderEntity order)
        {
            const string sql = @"
                INSERT INTO Uniwave_a2p_Orders (
                    Id, OrderNumber, ProjectNumber, Number, Version, OrderDate,
                    CustomerTitle, CustomerNumber, DeliveryAddress, CorrectionAvailableUnitil,
                    ResponsibleManager, SourceAppType, ItemCount, OrderCount, ErrorCount, TotalQuantity,
                    TotalUnits, TotalWeight, TotalWeightWithoutGlass, TotalWeightGlass, TotalArea, TotalHours,
                    TotalOrderCost, TotalLaborCost, TotalCost, TotalPrice, Currency, ExchangeRate, ExchangeRateDate,
                    CreatedUTCDateTime, CreatedBy
                )
                OUTPUT UNSERTED.*
                VALUES (
                    @Id, @OrderNumber, @ProjectNumber, @Number, @Version, @OrderDate,
                    @CustomerTitle, @CustomerNumber, @DeliveryAddress, @CorrectionAvailableUnitil,
                    @ResponsibleManager, @SourceAppType, @ItemCount, @OrderCount, @ErrorCount, @TotalQuantity,
                    @TotalUnits, @TotalWeight, @TotalWeightWithoutGlass, @TotalWeightGlass, @TotalArea, @TotalHours,
                    @TotalOrderCost, @TotalLaborCost, @TotalCost, @TotalPrice, @Currency, @ExchangeRate, @ExchangeRateDate,
                    @CreatedUTCDateTime, @CreatedBy
                );";
            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteAsync(sql, order);
        }

        // UPDATE ALL ORDER DETAILS
        public async Task<int> UpdateOrderDeliveryAddressAsync(Guid id, string deliveryAddress)
        {
            const string sql = @"UPDATE Uniwave_a2p_Orders SET DeliveryAddress = @DeliveryAddress WHERE Id = @id;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteAsync(sql, new { Id = id, DeliveryAddress = deliveryAddress });
        }

        // DELETE BY ID
        public async Task<int> DeleteOrderAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_Orders WHERE Id = @id;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteAsync(sql, new { Id = id });
        }

    }
}