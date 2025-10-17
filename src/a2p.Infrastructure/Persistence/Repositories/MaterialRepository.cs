using System.Data;

using a2p.Application.Interfaces.Repositories;
using a2p.Domain.Entities;

using Dapper;

namespace a2p.Infrastructure.Persistence.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly IDbConnectionFactory _factory;

        public MaterialRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        // CREATE
        public async Task<MaterialEntity?> CreateMaterialAsync(MaterialEntity material)
        {
            const string sql = @"
                INSERT INTO Uniwave_a2p_Materials (
                    Id, MaterialNumber, ProjectNumber, SalesDocumentNumber, SalesDocumentVersion, MaterialDate,
                    CustomerTitle, CustomerNumber, DeliveryAddress, CorrectionAvailableUnitil,
                    ResponsibleManager, SourceAppType, ItemCount, MaterialCount, ErrorCount, TotalQuantity,
                    TotalUnits, TotalWeight, TotalWeightWithoutGlass, TotalWeightGlass, TotalArea, TotalHours,
                    TotalMaterialCost, TotalLaborCost, TotalCost, TotalPrice, Currency, ExchangeRate, ExchangeRateDate,
                    CreatedUTCDateTime, CreatedBy
                )
                OUTPUT INSERTED.*
                VALUES (
                    @Id, @MaterialNumber, @ProjectNumber, @SalesDocumentNumber, @SalesDocumentVersion, @MaterialDate,
                    @CustomerTitle, @CustomerNumber, @DeliveryAddress, @CorrectionAvailableUnitil,
                    @ResponsibleManager, @SourceAppType, @ItemCount, @MaterialCount, @ErrorCount, @TotalQuantity,
                    @TotalUnits, @TotalWeight, @TotalWeightWithoutGlass, @TotalWeightGlass, @TotalArea, @TotalHours,
                    @TotalMaterialCost, @TotalLaborCost, @TotalCost, @TotalPrice, @Currency, @ExchangeRate, @ExchangeRateDate,
                    @CreatedUTCDateTime, @CreatedBy
                );";
            using IDbConnection db = _factory.CreateConnection();
            return await db.QuerySingleOrDefaultAsync<MaterialEntity>(sql, material);
        }

        // READ BY ID
        public async Task<MaterialEntity?> GetMaterialAsync(Guid id)
        {
            const string sql = "SELECT * FROM Uniwave_a2p_Materials WHERE Id = @id;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.QuerySingleOrDefaultAsync<MaterialEntity>(sql, new { Id = id });
        }

        // PAGED READ BY ORDER NUMBER
        public async Task<(IEnumerable<MaterialEntity> Materials, int TotalCount)> GetOrderMaterialsAsync(Guid id, int page, int size)
        {
            const string sql = @"
                SELECT * FROM Uniwave_a2p_Materials WHERE OrderId = @id
                ORDER BY SortOrder
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                SELECT COUNT(*) FROM Uniwave_a2p_Material WHERE OrderId = @id;";
            using IDbConnection db = _factory.CreateConnection();
            using SqlMapper.GridReader multi = await db.QueryMultipleAsync(sql, new { OrderId = @id, Offset = (page - 1) * size, PageSize = size });
            IEnumerable<MaterialEntity> materials = await multi.ReadAsync<MaterialEntity>();
            var total = await multi.ReadSingleAsync<int>();
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
            using IDbConnection db = _factory.CreateConnection();
            using SqlMapper.GridReader multi = await db.QueryMultipleAsync(sql, new { Offset = (page - 1) * size, PageSize = size });
            IEnumerable<MaterialEntity> materials = await multi.ReadAsync<MaterialEntity>();
            var total = await multi.ReadSingleAsync<int>();
            return (materials, total);
        }

        // UPDATE ALL ORDER DETAILS
        public async Task<int> UpdateMaterialAsync(MaterialEntity material)
        {
            const string sql = @"
                INSERT INTO Uniwave_a2p_Materials (
                    Id, MaterialNumber, ProjectNumber, SalesDocumentNumber, SalesDocumentVersion, MaterialDate,
                    CustomerTitle, CustomerNumber, DeliveryAddress, CorrectionAvailableUnitil,
                    ResponsibleManager, SourceAppType, ItemCount, MaterialCount, ErrorCount, TotalQuantity,
                    TotalUnits, TotalWeight, TotalWeightWithoutGlass, TotalWeightGlass, TotalArea, TotalHours,
                    TotalMaterialCost, TotalLaborCost, TotalCost, TotalPrice, Currency, ExchangeRate, ExchangeRateDate,
                    CreatedUTCDateTime, CreatedBy
                )
                OUTPUT UNSERTED.*
                VALUES (
                    @Id, @MaterialNumber, @ProjectNumber, @SalesDocumentNumber, @SalesDocumentVersion, @MaterialDate,
                    @CustomerTitle, @CustomerNumber, @DeliveryAddress, @CorrectionAvailableUnitil,
                    @ResponsibleManager, @SourceAppType, @ItemCount, @MaterialCount, @ErrorCount, @TotalQuantity,
                    @TotalUnits, @TotalWeight, @TotalWeightWithoutGlass, @TotalWeightGlass, @TotalArea, @TotalHours,
                    @TotalMaterialCost, @TotalLaborCost, @TotalCost, @TotalPrice, @Currency, @ExchangeRate, @ExchangeRateDate,
                    @CreatedUTCDateTime, @CreatedBy
                );";

            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteAsync(sql, material);
        }

        // DELETE BY ID
        public async Task<int> DeleteMaterialsdAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_Materials WHERE Id = @Id;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteAsync(sql, new { Id = id });
        }

        // DELETE BY ORDER ID
        public async Task<int> DeleteMaterialByOrderIdAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_Materials WHERE OrderId = @id;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteAsync(sql, new { OrderId = id });
        }
    }
}