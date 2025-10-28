using Application.Interfaces.Repositories;

using Domain.Entities;

using Infrastructure.Data;

using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly DapperService _dapper;
        private readonly ILogger<ItemRepository> _logger;

        public ItemRepository(DapperService dapper, ILogger<ItemRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        // CREATE
        public async Task<ItemEntity?> CreateItemAsync(ItemEntity item)
        {
            const string sql = @"INSERT INTO [dbo].[Uniwave_a2p_Items]  
            ([Id]
           ,[OrderId]
           ,[ItemName]
           ,[SortOrder]
           ,[Description]
           ,[Quantity]
           ,[Width]
           ,[Height]
           ,[Weight]
           ,[WeightWithoutGlass]
           ,[WeightGlass]
           ,[TotalWeight]
           ,[TotalWeightWithoutGlass]
           ,[TotalWeightGlass]
           ,[Area]
           ,[TotalArea]
           ,[Hours]
           ,[TotalHours]
           ,[MaterialCost]
           ,[LaborCost]
           ,[Cost]
           ,[TotalMaterialCost]
           ,[TotalLaborCost]
           ,[TotalCost]
           ,[Price]
           ,[TotalPrice]
           ,[Worksheet]
           ,[Line]
           ,[Column]
           ,[CreatedUTCDateTime]
           ,[ModifiedUTCDateTime]) 
            OUTPUT INSERTED .*
            VALUES  
            (@Id
           ,@OrderId
           ,@ItemName
           ,@SortOrder
           ,@Description
           ,@Quantity
           ,@Width
           ,@Height
           ,@Weight
           ,@WeightWithoutGlass
           ,@WeightGlass
           ,@TotalWeight
           ,@TotalWeightWithoutGlass
           ,@TotalWeightGlass
           ,@Area
           ,@TotalArea
           ,@Hours
           ,@TotalHours
           ,@MaterialCost
           ,@LaborCost
           ,@Cost
           ,@TotalMaterialCost
           ,@TotalLaborCost
           ,@TotalCost
           ,@Price
           ,@TotalPrice
           ,@Worksheet
           ,@Line
           ,@Column
           ,@CreatedUTCDateTime
           ,@ModifiedUTCDateTime)";

            return await _dapper.QuerySingleOrDefaultAsync<ItemEntity>(sql, item);
        }

        // READ BY ID
        public async Task<ItemEntity?> GetItemAsync(Guid id)
        {
            const string sql = "SELECT * FROM Uniwave_a2p_Materials WHERE Id = @id;";

            return await _dapper.QuerySingleOrDefaultAsync<ItemEntity>(sql, new { Id = id });
        }

        // READ BY ORDER ID
        public async Task<IEnumerable<ItemEntity>> GetOrderItemsAsync(Guid id)
        {
            const string sql = @"
                SELECT * FROM Uniwave_a2p_Materials WHERE OrderId = @id
                ORDER BY SortOrder";

            return await _dapper.QueryAsync<ItemEntity>(sql, new { Id = id });
        }

        // UPDATE 
        public async Task<int> UpdateItemAsync(ItemEntity item)
        {
            const string sql = @"INSERT INTO [dbo].[Uniwave_a2p_Items] 
             ([Id]
           ,[OrderId]
           ,[ItemName]
           ,[SortOrder]
           ,[Description]
           ,[Quantity]
           ,[Width]
           ,[Height]
           ,[Weight]
           ,[WeightWithoutGlass]
           ,[WeightGlass]
           ,[TotalWeight]
           ,[TotalWeightWithoutGlass]
           ,[TotalWeightGlass]
           ,[Area]
           ,[TotalArea]
           ,[Hours]
           ,[TotalHours]
           ,[MaterialCost]
           ,[LaborCost]
           ,[Cost]
           ,[TotalMaterialCost]
           ,[TotalLaborCost]
           ,[TotalCost]
           ,[Price]
           ,[TotalPrice]
           ,[Worksheet]
           ,[Line]
           ,[Column]
           ,[ModifiedUTCDateTime]
           )
            OUTPUT UNSERTED .*
            VALUES  
            (@Id
           ,@OrderId
           ,@ItemName
           ,@SortOrder
           ,@Description
           ,@Quantity
           ,@Width
           ,@Height
           ,@Weight
           ,@WeightWithoutGlass
           ,@WeightGlass
           ,@TotalWeight
           ,@TotalWeightWithoutGlass
           ,@TotalWeightGlass
           ,@Area
           ,@TotalArea
           ,@Hours
           ,@TotalHours
           ,@MaterialCost
           ,@LaborCost
           ,@Cost
           ,@TotalMaterialCost
           ,@TotalLaborCost
           ,@TotalCost
           ,@Price
           ,@TotalPrice
           ,@Worksheet
           ,@Line
           ,@Column
           ,@ModifiedUTCDateTime
           )";

            return await _dapper.ExecuteAsync(sql, item);
        }

        // DELETE BY ID
        public async Task<int> DeleteItemAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_Items WHERE Id = @id;";
            return await _dapper.ExecuteAsync(sql, new { Id = id });
        }

        // DELETE BY ORDER ID
        public async Task<int> DeleteOrderItemsAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_Items WHERE OrderId = @id;";
            return await _dapper.ExecuteAsync(sql, new { Id = id });
        }
    }
}