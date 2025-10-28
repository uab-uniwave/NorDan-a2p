using Application.Interfaces.Repositories;
using Application.Models;

using Domain.Entities;

using Infrastructure.Data;

using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repository that provides PrefSuite related queries and data retrieval operations.
    /// Implements <see cref="IPrefSuiteRepository"/> and is used by higher-level services
    /// such as <see cref="Application.Interfaces.Services.IPrefSuiteDataService"/>.
    /// </summary>
    public class PrefSuiteRepository : IPrefSuiteRepository
    {
        private readonly DapperService _dapper;
        private readonly ILogger<PrefSuiteRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefSuiteRepository"/> class.
        /// </summary>
        /// <param name="dapper">A <see cref="DapperService"/> used to execute database queries.</param>
        /// <param name="logger">Logger for repository diagnostics.</param>
        public PrefSuiteRepository(DapperService dapper, ILogger<PrefSuiteRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<SalesDocument?> GetSalesDocumentByOrderAsync(string orderNumber)
        {
            const string sql = @"SELECT 
                                [PAF].[RowId] AS RowId, 
                                [PAF].[Numero] AS Number,  
                                [PAF].[Version], 
                                [PAF].[FechaSolicitud]AS  CreationDate, 
                                [PAF].[FechaSalidaTaller] AS ShopExitDate, 
                                [PAF].[FechaDespiece] AS BreakdownDate, 
                                [PAF].[Referencia] Reference, 
                                [PAF].[PriceCurrency] PriceCurrency,  
                                [PAF].[Nombre] CustomerName,  
                                [PAF].[CodigoCliente] CustomerCode, 
                                [PAF].[User1], 
                                [PAF].[Domicilio] AS CustomerAddress1, 
                                [PAF].[CodigoPostal] AS CustomerPostalCode, 
                                [PAF].[Localidad] AS CustomerCity, 
                                [PAF].[Provincia] AS CustomerProvince, 
                                [PAF].[Pais] AS CustomerCountry, 
                                (SELECT [dbo].[Uniwave_a2p_GetOrderState] (RowId)) AS State,
                                [PAF].[SalesmanCode],
                                [CO].[Nombre] SalesmanName 
                                FROM PAF  LEFT OUTER JOIN Comerciales CO ON PAF.SalesmanCode = CO.Codigo 
                                WHERE [PAF].[Referencia] = @OrderNumber";

            return await _dapper.QuerySingleOrDefaultAsync<SalesDocument>(sql, new { OrderNumber = orderNumber });
        }

        public async Task<SalesDocument?> GetSalesDocumentByRowIdAsync(Guid rowId)
        {
            const string sql = @"SELECT 
                                [PAF].[RowId] AS RowId, 
                                [PAF].[Numero] AS Number,  
                                [PAF].[Version], 
                                [PAF].[FechaSolicitud]AS  CreationDate, 
                                [PAF].[FechaSalidaTaller] AS ShopExitDate, 
                                [PAF].[FechaDespiece] AS BreakdownDate, 
                                [PAF].[Referencia] Reference, 
                                [PAF].[PriceCurrency] PriceCurrency,  
                                [PAF].[Nombre] CustomerName,  
                                [PAF].[CodigoCliente] CustomerCode, 
                                [PAF].[User1], 
                                [PAF].[Domicilio] AS CustomerAddress1, 
                                [PAF].[CodigoPostal] AS CustomerPostalCode, 
                                [PAF].[Localidad] AS CustomerCity, 
                                [PAF].[Provincia] AS CustomerProvince, 
                                [PAF].[Pais] AS CustomerCountry, 
                                (SELECT [dbo].[Uniwave_a2p_GetOrderState] (RowId)) AS State,
                                [PAF].[SalesmanCode],
                                [CO].[Nombre] SalesmanName 
                                FROM PAF  LEFT OUTER JOIN Comerciales CO ON PAF.SalesmanCode = CO.Codigo 
                                WHERE [PAF].[RowId] = @RowId";

            return await _dapper.QuerySingleOrDefaultAsync<SalesDocument>(sql, new { RowId = rowId });
        }

       
        public async Task<string?> GetGlassReferenceAsync(string description)
        {
            const string sql = "SELECT ReferenciaBase FROM MaterialesBase WHERE tipocalculo = 'Superficies' and Nivel1 = '03 Glass' and Descripcion = @Description";
            return await _dapper.QuerySingleOrDefaultAsync<string?>(sql, new { Description = description });
        }

        public async Task<int?> GetPrefSuiteColorConfigurationAsync(string color)
        {
            const string sql = "SELECT dbo.Uniwave_a2p_GetColorConfiguration(@Color)";
            return await _dapper.QuerySingleOrDefaultAsync<int?>(sql, new { Color = color });
        }

        public async Task<int?> GetCommodityCode(string sourceReference)
        {
            const string sql = "SELECT [dbo].[Uniwave_a2p_GetTechDesignCommodityCode](@SourceReference)";
            return await _dapper.QuerySingleOrDefaultAsync<int?>(sql, new { SourceReference = sourceReference });
        }

        public async Task<decimal?> GetTechDesignWeight(string sourceReference)
        {
            const string sql = "SELECT [dbo].[Uniwave_a2p_GetTechDesignWeight](@SourceReference)";
            return await _dapper.QuerySingleOrDefaultAsync<decimal?>(sql, new { SourceReference = sourceReference });
        }

        public async Task<string?> GetSapaColorAsync(string color)
        {
            const string sql = "SELECT dbo.Uniwave_a2p_GetSapaColor(@Color)";
            return await _dapper.QuerySingleOrDefaultAsync<string?>(sql, new { Color = color });
        }

        public async Task DeleteSalesDocumentDataAsync(int number, int version, bool deleteExisting)
        {
            const string sql = "EXEC [dbo].[Uniwave_a2p_DeleteExistingData] @Number, @Version";
            try
            {
                await _dapper.ExecuteAsync(sql, new { Number = number, Version = version });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sales document data for {Number}/{Version}", number, version);
                throw;
            }
        }

        public async Task InsertPrefSuiteColorAsync(MaterialEntity material)
        {
            const string sql = "EXEC [dbo].[Uniwave_a2p_InsertPrefSuiteColor] @Color, @ColorDescription";
            await _dapper.ExecuteAsync(sql, new { Color = material.Color, ColorDescription = material.ColorDescription });
        }

        public async Task InsertPrefSuiteColorConfigurationAsync(MaterialEntity material)
        {
            const string sql = "EXEC [dbo].[Uniwave_a2p_InsertPrefSuiteColorConfiguration] @Color";
            await _dapper.ExecuteAsync(sql, new { Color = material.Color });
        }

        public async Task InsertPrefSuiteMaterialBaseAsync(MaterialEntity material)
        {
            // ensure commodity code available like legacy service did
            int? commodityCode = await GetCommodityCode(material.SourceReference ?? string.Empty);
            const string sql = "EXEC [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialBase] @ReferenceBase, @Description, @MaterialType, @CommodityCode";
            await _dapper.ExecuteAsync(sql, new
            {
                ReferenceBase = material.ReferenceBase,
                Description = material.Description ?? string.Empty,
                MaterialType = material.MaterialType.ToString(),
                CommodityCode = commodityCode
            });
        }

        public async Task InsertPrefSuiteMaterialAsync(MaterialEntity material)
        {
            const string sql = "EXEC [dbo].[Uniwave_a2p_InsertPrefSuiteMaterial] @ReferenceBase, @Reference, @Color, @PackageQuantity, @Weight, @MaterialType";
            await _dapper.ExecuteAsync(sql, new
            {
                ReferenceBase = material.ReferenceBase,
                Reference = material.Reference,
                Color = material.Color,
                PackageQuantity = material.PackageQuantity,
                Weight = material.Weight,
                MaterialType = material.MaterialType.ToString()
            });
        }

        public async Task InsertPrefSuiteMaterialProfileAsync(MaterialEntity material)
        {
            if (material.Weight == 0)
            {
                decimal? weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                material.Weight = weight ?? material.Weight;
            }

            const string sql = "EXEC [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialProfile] @ReferenceBase, @PackageQuantity, @Weight";
            await _dapper.ExecuteAsync(sql, new
            {
                ReferenceBase = material.ReferenceBase,
                PackageQuantity = material.PackageQuantity,
                Weight = material.Weight
            });
        }

        public async Task InsertPrefSuiteMaterialMeterAsync(MaterialEntity material)
        {
            if (material.Weight == 0)
            {
                decimal? weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                material.Weight = weight ?? material.Weight;
            }

            const string sql = "EXEC [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialMeter] @ReferenceBase, @Weight";
            await _dapper.ExecuteAsync(sql, new
            {
                ReferenceBase = material.ReferenceBase,
                Weight = material.Weight
            });
        }

        public async Task InsertPrefSuiteMaterialPieceAsync(MaterialEntity material)
        {
            if (material.Weight == 0)
            {
                decimal? weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                material.Weight = weight ?? material.Weight;
            }

            const string sql = "EXEC [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialPiece] @ReferenceBase, @Weight";
            await _dapper.ExecuteAsync(sql, new
            {
                ReferenceBase = material.ReferenceBase,
                Weight = material.Weight
            });
        }

        public async Task InsertPrefSuiteMaterialSurfaceAsync(MaterialEntity material)
        {
            if (material.Weight == 0)
            {
                decimal? weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                material.Weight = weight ?? material.Weight;
            }

            // Note: legacy implementation used "[dbo].[Uniwave_a2p_InsertPreSuiteMaterialSurface]" (typo). Keep the same stored proc name here.
            const string sql = "EXEC [dbo].[Uniwave_a2p_InsertPreSuiteMaterialSurface] @ReferenceBase, @Weight, @MaterialType";
            await _dapper.ExecuteAsync(sql, new
            {
                ReferenceBase = material.ReferenceBase,
                Weight = material.Weight,
                MaterialType = material.MaterialType.ToString()
            });
        }

        public async Task InsertPrefSuiteMaterialPurchaseDataAsync(MaterialEntity material)
        {
            const string sql = "EXEC [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialPurchaseData] @Reference, @Package, @Price, @Description, @Color, @SourceReference, @SourceColor, @MaterialType";
            await _dapper.ExecuteAsync(sql, new
            {
                Reference = material.Reference,
                Package = material.PackageQuantity,
                Price = material.Price,
                Description = material.Description,
                Color = material.Color,
                SourceReference = material.SourceReference,
                SourceColor = material.SourceColor,
                MaterialType = material.MaterialType.ToString()
            });
        }

        public async Task UpdateBCMapping(MaterialEntity material)
        {
            const string sql = "EXEC [dbo].[Uniwave_a2p_UpdateBCMapping] @ReferenceBase, @Reference, @SourceReference, @SourceColor, @SourceColor1, @SourceColor2";
            await _dapper.ExecuteAsync(sql, new
            {
                ReferenceBase = material.ReferenceBase ?? (object)DBNull.Value,
                Reference = material.Reference ?? (object)DBNull.Value,
                SourceReference = material.SourceReference ?? (object)DBNull.Value,
                SourceColor = material.SourceColor ?? (object)DBNull.Value,
                SourceColor1 = material.CustomField1 ?? (object)DBNull.Value,
                SourceColor2 = material.CustomField2 ?? (object)DBNull.Value
            });
        }

        public async Task InsertPrefSuiteMaterialNeedsMasterAsync(string order, int number, int version)
        {
            const string sql = "EXEC [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialNeedsMaster] @Number, @Version";
            await _dapper.ExecuteAsync(sql, new { Number = number, Version = version });
        }

        public async Task InsertPrefSuiteMaterialNeedsAsync(string order, int number, int version)
        {
            const string sql = "EXEC [dbo].[Uniwave_a2p_InsertPrefSuiteMaterialNeeds] @Number, @Version";
            await _dapper.ExecuteAsync(sql, new { Number = number, Version = version });
        }
    }
}