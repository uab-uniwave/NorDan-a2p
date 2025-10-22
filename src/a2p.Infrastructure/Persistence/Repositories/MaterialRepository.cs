using Application.Interfaces.Repositories;

using Dapper;

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
           ,[OrderNumber]
           ,[ProjectNumber]
           ,[SalesDocumentNumber]
           ,[SalesDocumentVersion]
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
           ,@OrderNumber
           ,@ProjectNumber
           ,@SalesDocumentNumber
           ,@SalesDocumentVersion
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
        public async Task<(IEnumerable<MaterialEntity> Materials, int TotalCount)> GetOrderMaterialsAsync(Guid id, int page, int size)
        {
            const string sql = @"
                SELECT * FROM Uniwave_a2p_Materials WHERE OrderId = @id
                ORDER BY SortOrder
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                SELECT COUNT(*) FROM Uniwave_a2p_Material WHERE OrderId = @id;";

            SqlMapper.GridReader multi = await _dapper.QueryMultipleAsync(sql, new { OrderId = @id, Offset = (page - 1) * size, PageSize = size });
            IEnumerable<MaterialEntity> materials = await multi.ReadAsync<MaterialEntity>();
            int total = await multi.ReadSingleAsync<int>();
            return (materials, total);
        }

        // PAGED READ
        public async Task<(IEnumerable<MaterialEntity> Materials, int TotalCount)> GetMaterialsAsync(int page, int size)
        {
            const string sql = @"
                SELECT * FROM Uniwave_a2p_Materials
                ORDER BY OrderNumber DESC, SortOrder 
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                SELECT COUNT(*) FROM Uniwave_a2p_Material;";

            SqlMapper.GridReader multi = await _dapper.QueryMultipleAsync(sql, new { Offset = (page - 1) * size, PageSize = size });
            IEnumerable<MaterialEntity> materials = await multi.ReadAsync<MaterialEntity>();
            int total = await multi.ReadSingleAsync<int>();
            return (materials, total);
        }

        // UPDATE ALL ORDER DETAILS
        public async Task<int> UpdateMaterialAsync(MaterialEntity material)
        {
            const string sql = @"INSERT INTO Uniwave_a2p_Materials 
           ([Id]  
           ,[OrderId]
           ,[OrderNumber]
           ,[ProjectNumber]
           ,[SalesDocumentNumber]
           ,[SalesDocumentVersion]
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
           ,@OrderNumber
           ,@ProjectNumber
           ,@SalesDocumentNumber
           ,@SalesDocumentVersion
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
        public async Task<int> DeleteMaterialByOrderIdAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_Materials WHERE OrderId = @id;";

            return await _dapper.ExecuteAsync(sql, new { OrderId = id });
        }
    }
}