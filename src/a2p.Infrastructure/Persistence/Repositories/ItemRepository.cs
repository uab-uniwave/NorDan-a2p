using System.Data;

using a2p.Application.Interfaces.Repositories;
using a2p.Domain.Entities;

using Dapper;

namespace a2p.Infrastructure.Persistence.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IDbConnectionFactory _factory;

        public ItemRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        // CREATE
        public async Task<ItemEntity?> CreateItemAsync(ItemEntity item)
        {
            const string sql = @"INSERT INTO [dbo].[Uniwave_a2p_Items]  
                    ( 
                    [Id], 
                    [OrderNumber], 
                    [Worksheet], 
                    [Line], 
                    [Column], 
                    [SalesDocumentNumber], 
                    [SalesDocumentVersion], 
                    [ItemName], 
                    [SortOrder], 
                    [Description], 
                    [Quantity], 
                    [Width], 
                    [Height], 
                    [Weight], 
                    [WeightWithoutGlass], 
                    [WeightGlass], 
                    [TotalWeight], 
                    [TotalWeightWithoutGlass], 
                    [TotalWeightGlass], 
                    [Area], 
                    [TotalArea], 
                    [Hours], 
                    [TotalHours], 
                    [MaterialCost], 
                    [LaborCost], 
                    [Cost], 
                    [TotalMaterialCost], 
                    [TotalLaborCost], 
                    [TotalCost], 
                    [Price], 
                    [TotalPrice], 
                    [CurrencyCode], 
                    [ExchangeRateEUR], 
                    [MaterialCostEUR], 
                    [LaborCostEUR], 
                    [CostEUR], 
                    [TotalMaterialCostEUR], 
                    [TotalLaborCostEUR], 
                    [TotalCostEUR], 
                    [PriceEUR], 
                    [TotalPriceEUR], 
                    [WorksheetType], 
                    [CreatedUTCDateTime], 
                    [ModifiedUTCDateTime] 
                    )  
                    OUTPUT INSERTED .*
                    VALUES  
                    ( 
                    @Id, 
                    @OrderNumber, 
                    @Worksheet, 
                    @Line, 
                    @Column, 
                    @SalesDocumentNumber, 
                    @SalesDocumentVersion, 
                    @ItemName, 
                    @SortOrder, 
                    @Description, 
                    @Quantity, 
                    @Width, 
                    @Height, 
                    @Weight, 
                    @WeightWithoutGlass, 
                    @WeightGlass, 
                    @TotalWeight, 
                    @TotalWeightWithoutGlass, 
                    @TotalWeightGlass, 
                    @Area, 
                    @TotalArea, 
                    @Hours, 
                    @TotalHours, 
                    @MaterialCost, 
                    @LaborCost, 
                    @Cost, 
                    @TotalMaterialCost, 
                    @TotalLaborCost, 
                    @TotalCost, 
                    @Price, 
                    @TotalPrice, 
                    @CurrencyCode, 
                    @ExchangeRateEUR, 
                    @MaterialCostEUR, 
                    @LaborCostEUR, 
                    @CostEUR, 
                    @TotalMaterialCostEUR, 
                    @TotalLaborCostEUR, 
                    @TotalCostEUR, 
                    @PriceEUR, 
                    @TotalPriceEUR, 
                    @WorksheetType, 
                    @CreatedUTCDateTime, 
                    @ModifiedUTCDateTime 
                    )";
            using IDbConnection db = _factory.CreateConnection();

            return await db.QuerySingleOrDefaultAsync<ItemEntity>(sql, item);
        }

        // READ BY ID
        public async Task<ItemEntity?> GetItemAsync(Guid id)
        {
            const string sql = "SELECT * FROM Uniwave_a2p_Materials WHERE Id = @id;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.QuerySingleOrDefaultAsync<ItemEntity>(sql, new { Id = id });
        }

        // PAGED READ BY ORDER NUMBER
        public async Task<(IEnumerable<ItemEntity> Ir, int TotalCount)> GetOrderItems(Guid id, int page, int size)
        {
            const string sql = @"
                SELECT * FROM Uniwave_a2p_Materials WHERE OrderId = @id
                ORDER BY SortOrder
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                SELECT COUNT(*) FROM Uniwave_a2p_Material WHERE  OrderId = @id;";

            using IDbConnection db = _factory.CreateConnection();
            using SqlMapper.GridReader multi = await db.QueryMultipleAsync(sql, new { OrderId = id, Offset = (page - 1) * size, PageSize = size });
            IEnumerable<ItemEntity> items = await multi.ReadAsync<ItemEntity>();
            var total = await multi.ReadSingleAsync<int>();
            return (items, total);
        }

        // PAGED READ`
        public async Task<(IEnumerable<ItemEntity> Items, int TotalCount)> GetItemsAsync(int page, int size)
        {
            const string sql = @"
                SELECT * FROM Uniwave_a2p_Materials
                ORDER BY OrderNumber DESC, SortOrder 
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                SELECT COUNT(*) FROM Uniwave_a2p_Material;";
            using IDbConnection db = _factory.CreateConnection();
            using SqlMapper.GridReader multi = await db.QueryMultipleAsync(sql, new { Offset = (page - 1) * size, PageSize = size });
            IEnumerable<ItemEntity> items = await multi.ReadAsync<ItemEntity>();
            var total = await multi.ReadSingleAsync<int>();
            return (items, total);
        }

        // UPDATE ALL ORDER DETAILS
        public async Task<int> UpdateItemAsync(ItemEntity item)
        {
            const string sql = @"
            INSERT INTO [Uniwave_a2p_Items]
            (
                    [Id],
                    [OrderNumber],
                    [Worksheet],
                    [Line],
                    [Column],
                    [SalesDocumentNumber],
                    [SalesDocumentVersion],
                    [ItemName],
                    [SortOrder],
                    [Description],
                    [Quantity],
                    [Width],
                    [Height],
                    [Weight],
                    [WeightWithoutGlass],
                    [WeightGlass],
                    [TotalWeight],
                    [TotalWeightWithoutGlass],
                    [TotalWeightGlass],
                    [Area],
                    [TotalArea],
                    [Hours],
                    [TotalHours],
                    [MaterialCost],
                    [LaborCost],
                    [Cost],
                    [TotalMaterialCost],
                    [TotalLaborCost],
                    [TotalCost],
                    [Price],
                    [TotalPrice],
                    [CurrencyCode],
                    [ExchangeRateEUR],
                    [MaterialCostEUR],
                    [LaborCostEUR],
                    [CostEUR],
                    [TotalMaterialCostEUR],
                    [TotalLaborCostEUR],
                    [TotalCostEUR],
                    [PriceEUR],
                    [TotalPriceEUR],
                    [WorksheetType],
                    [CreatedUTCDateTime],
                    [ModifiedUTCDateTime]
                    )
                    OUTPUT INSERTED .*
                    VALUES
                    (
                    @Id,
                    @OrderNumber,
                    @Worksheet,
                    @Line,
                    @Column,
                    @SalesDocumentNumber,
                    @SalesDocumentVersion,
                    @ItemName,
                    @SortOrder,
                    @Description,
                    @Quantity,
                    @Width,
                    @Height,
                    @Weight,
                    @WeightWithoutGlass,
                    @WeightGlass,
                    @TotalWeight,
                    @TotalWeightWithoutGlass,
                    @TotalWeightGlass,
                    @Area,
                    @TotalArea,
                    @Hours,
                    @TotalHours,
                    @MaterialCost,
                    @LaborCost,
                    @Cost,
                    @TotalMaterialCost,
                    @TotalLaborCost,
                    @TotalCost,
                    @Price,
                    @TotalPrice,
                    @CurrencyCode,
                    @ExchangeRateEUR,
                    @MaterialCostEUR,
                    @LaborCostEUR,
                    @CostEUR,
                    @TotalMaterialCostEUR,
                    @TotalLaborCostEUR,
                    @TotalCostEUR,
                    @PriceEUR,
                    @TotalPriceEUR,
                    @WorksheetType,
                    @CreatedUTCDateTime,
                    @ModifiedUTCDateTime
                    )";
            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteAsync(sql, item);
        }

        // DELETE BY ID
        public async Task<int> DeleteItemAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_Materials WHERE Id = @id;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteAsync(sql, new { Id = id });
        }

        // DELETE BY ORDER ID
        public async Task<int> DeleteItemByOrderIdAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_Materials WHERE OrderId = @id;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteAsync(sql, new { Id = id });
        }
    }
}