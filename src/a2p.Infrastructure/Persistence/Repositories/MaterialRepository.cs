using Application.Interfaces.Repositories;

using Domain.Entities;

using Infrastructure.Data;

using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {

        private readonly DapperService _dapper;
        private readonly ILogger<MaterialRepository> _logger;

        public MaterialRepository(DapperService dapper, ILogger<MaterialRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        // CREATE
        public async Task<MaterialEntity?> CreateMaterialAsync(MaterialEntity material)
        {
            const string sql = @"INSERT INTO Uniwave_a2p_Materials 
 ([Id] 
 ,[OrderId]
 ,[ItemId]
 ,[ItemName]
 ,[SortOrder]
 ,[Reference]
 ,[ReferenceBase]
 ,[Description]
 ,[Color]
 ,[ColorDescription]
 ,[Width]
 ,[Height]
 ,[Quantity]
 ,[PackageQuantity]
 ,[TotalQuantity]
 ,[RequiredQuantity]
 ,[LeftOverQuantity]
 ,[Weight]
 ,[TotalWeight]
 ,[RequiredWeight]
 ,[LeftOverWeight]
 ,[Area]
 ,[TotalArea]
 ,[RequiredArea]
 ,[LeftOverArea]
 ,[Waste]
 ,[Price]
 ,[TotalPrice]
 ,[RequiredPrice]
 ,[LeftOverPrice]
 ,[SquareMeterPrice]
 ,[Pallet]
 ,[CustomField1]
 ,[CustomField2]
 ,[CustomField3]
 ,[CustomField4]
 ,[CustomField5]
 ,[MaterialType]
 ,[SourceReference]
 ,[SourceDescription]
 ,[SourceColor]
 ,[SourceColorDescription]
 ,[Worksheet]
 ,[Line]
 ,[Column]
 ,[CreatedUTCDateTime]
 ,[ModifiedUTCDateTime])
 
 OUTPUT INSERTED.*
 VALUES
 (@Id 
 ,@OrderId
 ,@ItemId
 ,@ItemName
 ,@SortOrder
 ,@Reference
 ,@ReferenceBase
 ,@Description
 ,@Color
 ,@ColorDescription
 ,@Width
 ,@Height
 ,@Quantity
 ,@PackageQuantity
 ,@TotalQuantity
 ,@RequiredQuantity
 ,@LeftOverQuantity
 ,@Weight
 ,@TotalWeight
 ,@RequiredWeight
 ,@LeftOverWeight
 ,@Area
 ,@TotalArea
 ,@RequiredArea
 ,@LeftOverArea
 ,@Waste
 ,@Price
 ,@TotalPrice
 ,@RequiredPrice
 ,@LeftOverPrice
 ,@SquareMeterPrice
 ,@Pallet
 ,@CustomField1
 ,@CustomField2
 ,@CustomField3
 ,@CustomField4
 ,@CustomField5
 ,@MaterialType
 ,@SourceReference
 ,@SourceDescription
 ,@SourceColor
 ,@SourceColorDescription
 ,@Worksheet
 ,@Line
 ,@Column
 ,GetUTCDate()
 ,GetUTCDate()
 )";

            return await _dapper.QuerySingleOrDefaultAsync<MaterialEntity>(sql, material);
        }

        // READ BY ID
        public async Task<MaterialEntity?> GetMaterialAsync(Guid id)
        {
            const string sql = "SELECT * FROM Uniwave_a2p_Materials WHERE Id = @id;";

            return await _dapper.QuerySingleOrDefaultAsync<MaterialEntity>(sql, new { Id = id });
        }

        // PAGED READ BY ORDER NUMBER
        public async Task<IEnumerable<MaterialEntity>> GetOrderMaterialsAsync(Guid id)
        {
            const string sql = @"
 SELECT * FROM Uniwave_a2p_Materials WHERE OrderId = @id
 ORDER BY SortOrder";

            return await _dapper.QueryAsync<MaterialEntity>(sql, new { Id = id });
        }

        // UPDATE ALL ORDER DETAILS
        public async Task<int> UpdateMaterialAsync(MaterialEntity material)
        {
            const string sql = @"INSERT INTO Uniwave_a2p_Materials 
 ([Id] 
 ,[OrderId]
 ,[ItemId]
 ,[ItemName]
 ,[SortOrder]
 ,[Reference]
 ,[ReferenceBase]
 ,[Description]
 ,[Color]
 ,[ColorDescription]
 ,[Width]
 ,[Height]
 ,[Quantity]
 ,[PackageQuantity]
 ,[TotalQuantity]
 ,[RequiredQuantity]
 ,[LeftOverQuantity]
 ,[Weight]
 ,[TotalWeight]
 ,[RequiredWeight]
 ,[LeftOverWeight]
 ,[Area]
 ,[TotalArea]
 ,[RequiredArea]
 ,[LeftOverArea]
 ,[Waste]
 ,[Price]
 ,[TotalPrice]
 ,[RequiredPrice]
 ,[LeftOverPrice]
 ,[SquareMeterPrice]
 ,[Pallet]
 ,[CustomField1]
 ,[CustomField2]
 ,[CustomField3]
 ,[CustomField4]
 ,[CustomField5]
 ,[MaterialType]
 ,[SourceReference]
 ,[SourceDescription]
 ,[SourceColor]
 ,[SourceColorDescription]
 ,[Worksheet]
 ,[Line]
 ,[Column]
 ,[ModifiedUTCDate])
 OUTPUT UNSERTED.*
 VALUES
 (@Id 
 ,@OrderId
 ,@ItemId
 ,@ItemName
 ,@SortOrder
 ,@Reference
 ,@ReferenceBase
 ,@Description
 ,@Color
 ,@ColorDescription
 ,@Width
 ,@Height
 ,@Quantity
 ,@PackageQuantity
 ,@TotalQuantity
 ,@RequiredQuantity
 ,@LeftOverQuantity
 ,@Weight
 ,@TotalWeight
 ,@RequiredWeight
 ,@LeftOverWeight
 ,@Area
 ,@TotalArea
 ,@RequiredArea
 ,@LeftOverArea
 ,@Waste
 ,@Price
 ,@TotalPrice
 ,@RequiredPrice
 ,@LeftOverPrice
 ,@SquareMeterPrice
 ,@Pallet
 ,@CustomField1
 ,@CustomField2
 ,@CustomField3
 ,@CustomField4
 ,@CustomField5
 ,@MaterialType
 ,@SourceReference
 ,@SourceDescription
 ,@SourceColor
 ,@SourceColorDescription
 ,@Worksheet
 ,@Line
 ,@Column
 ,GetUTCDate())";

            return await _dapper.ExecuteAsync(sql, material);
        }

        // DELETE BY ID
        public async Task<int> DeleteMaterialsdAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_Materials WHERE Id = @Id;";

            return await _dapper.ExecuteAsync(sql, new { Id = id });
        }

        // DELETE BY ORDER ID
        public async Task<int> DeleteOrderMaterialsAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_Materials WHERE OrderId = @id;";

            return await _dapper.ExecuteAsync(sql, new { OrderId = id });
        }
    }
}