// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.Services;
using a2p.Domain.Entities;
using a2p.Domain.Enums;
using a2p.Domain.Respoitories;

using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Infrastructure.Services.SQLService
{
    public class SQLRepository : ISQLRepository
    {
        private readonly ILogService _logService;
        private readonly ISQLService _sqlRepository;

        public SQLRepository(ISQLService sqlRepository, ILogService logService)
        {
            _sqlRepository = sqlRepository ?? throw new ArgumentNullException(nameof(sqlRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public async Task<int> GetSalesDocumentStateAsync(int number, int version)
        {

            object? result;
            int state = 0;

            if (number < 1 || version < 1)
            {
                _logService.Verbose("{$Class}.{$Method}. Error getting sales document state. Number {$Number} or version {$Version} are wrong.",
                       nameof(SQLRepository),
                nameof(GetSalesDocumentStateAsync), number, version);
                return state;
            }

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT [dbo].[Uniwave_a2p_GetSalesDocumentState](@Number, @Version)",
                };

                _ = cmd.Parameters.AddWithValue("@Number", number);
                _ = cmd.Parameters.AddWithValue("@Version", version);

                result = await _sqlRepository.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                state = result != DBNull.Value ? (int)result! : 0;

                return state;

            }

            catch (Exception ex)
            {
                _logService.Verbose(
                "{$Class}.{$Method}. Unhandled error in {$Class}. {$Method}. Error getting order state for sales document. Exception: {Exception}.",
                nameof(SQLRepository),
                nameof(GetSalesDocumentStateAsync),
                 ex.Message
               );
                return state;
            }

        }

        public async Task<(int, int)> GetSalesDocumentAsync(string order)
        {
            (int, int) result;

            if (string.IsNullOrEmpty(order))
            {

                return (-1, -1);

            }
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = $"SELECT TOP 1 [Numero], [Version] FROM PAF WHERE Referencia like N'{order}%'"
                };

                result = await _sqlRepository.ExecuteQueryTupleValuesAsync(cmd.CommandText, cmd.CommandType);
                return result.Item1 < 1 || result.Item2 < 1 ? (-1, -1) : result;

            }

            catch (Exception ex)
            {
                _logService.Verbose(
                "{$Class}.{$Method}. Unhandled error getting sales document number and version. Exception: {Exception}.",
                nameof(SQLRepository),
                nameof(GetSalesDocumentAsync),
                ex.Message
               );
                return (-1, -1);

            }

        }
        public async Task<string?> GetGlassReferenceAsync(string description)
        {

            string? glassReference = null;

            if (string.IsNullOrEmpty(description))
            {

                _logService.Information("{$Class}.{$Method}. Error getting glass reference. Provided glass description is missing.",
                  nameof(SQLRepository),
                      nameof(GetGlassReferenceAsync));
                return null;
            }

            try
            {
                string sqlCommand = $"SELECT TOP 1 ReferenciaBase FROM MaterialesBase WHERE tipocalculo = 'Superficies' and Nivel1 = '03 Glass' and Descripcion = '{description}'";
                CommandType commandType = CommandType.Text;
                object? result = await _sqlRepository.ExecuteScalarAsync(sqlCommand, commandType);

                if (result == null)
                {
                    _logService.Verbose("{$Class}.{$Method}. Error getting glass reference. Glass with description {$Description} not found coresponding glass reference in PrefSuite DB.",
                      nameof(SQLRepository),
                      nameof(GetGlassReferenceAsync),
                      description);
                    return null;
                }

                glassReference = result.ToString();

                if (string.IsNullOrEmpty(glassReference))
                {
                    _logService.Verbose("{$Class}.{$Method}. Error getting glass reference. Glass with description {$Description} not found coresponding glass reference in PrefSuite DB.",
                      nameof(SQLRepository),
                      nameof(GetGlassReferenceAsync),
                      description);
                    return null;
                }

                _logService.Verbose
                    ("{$Class}.{$Method}. Glass with description {$Description}  found coresponding glass reference {$Reference} in PrefSuite DB.",
                     description,
                     glassReference,
                     nameof(SQLRepository),
                     nameof(GetGlassReferenceAsync),
                     description);

                return glassReference;

            }
            catch (Exception ex)
            {
                _logService.Verbose(
                  "{$Class}.{$Method}. Unhandled error inserting color configuration for color {$Color}. Exception: {$Exception}.",
                  nameof(SQLRepository),
                  nameof(GetGlassReferenceAsync),
                  ex.Message
                 );

                return glassReference;

            }
        }
        public async Task<int?> GetCommodityCode(string sourceReference)
        {
            if (string.IsNullOrEmpty(sourceReference))
            {
                _logService.Information("{$Class}.{$Method}. Error getting TechDesign commodity code. Provided sourceReference is missing.",
                    nameof(SQLRepository),
                    nameof(GetCommodityCode));
                return null;
            }

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT [dbo].[Uniwave_a2p_GetTechDesignCommodityCode](@SourceReference)",
                };

                _ = cmd.Parameters.AddWithValue("@SourceReference", sourceReference);

                object? result = await _sqlRepository.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                return result != null && result != DBNull.Value ? (int)result : null;
                ;
            }
            catch (Exception ex)
            {
                _logService.Verbose(
                    "{$Class}.{$Method}. Unhandled error in {$Class}. {$Method}. Error getting TechDesign commodity code. Exception: {Exception}.",
                    nameof(SQLRepository),
                    nameof(GetCommodityCode),
                    ex.Message
                );
                return null;
            }
        }

        public async Task<decimal> GetTechDesignWeight(string sourceReference)
        {
            if (string.IsNullOrEmpty(sourceReference))
            {
                _logService.Information("{$Class}.{$Method}. Error getting TechDesign Weight. Provided sourceReference is missing.",
                    nameof(SQLRepository),
                    nameof(GetTechDesignWeight));
                return 0;
            }

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT [dbo].[Uniwave_a2p_GetTechDesignWeight](@SourceReference)",
                };

                _ = cmd.Parameters.AddWithValue("@SourceReference", sourceReference);

                object? result = await _sqlRepository.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return result != null && result != DBNull.Value ? (decimal)result : 0;
            }
            catch (Exception ex)
            {
                _logService.Verbose(
                    "{$Class}.{$Method}. Unhandled error in {$Class}. {$Method}.Error getting TechDesign Weight. Exception: {Exception}.",
                    nameof(SQLRepository),
                    nameof(GetTechDesignWeight),
                    ex.Message
                );
                return 0;
            }
        }
        public async Task<string?> GetSapaColorAsync(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                _logService.Information("{$Class}.{$Method}. Error getting Sapa color. Provided color is missing.",
                    nameof(SQLRepository),
                    nameof(GetSapaColorAsync));
                return null;
            }

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT [dbo].[Uniwave_a2p_GetSapaColor](@TechDesignColor)",
                };

                _ = cmd.Parameters.AddWithValue("@TechDesignColor", color);

                object? result = await _sqlRepository.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return result != null && result != DBNull.Value ? result.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                _logService.Verbose(
                    "{$Class}.{$Method}. Unhandled error in {$Class}. {$Method}. Error getting order state for sales document. Exception: {Exception}.",
                    nameof(SQLRepository),
                    nameof(GetSapaColorAsync),
                    ex.Message
                );
                return string.Empty;
            }
        }

        public async Task<int> GetPrefSuiteColorConfigurationAsync(string color)
        {
            int result = -1;

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteColorConfiguration]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Color", color); //required

                //=====================================================================================================================
                result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Color configuration for color {$Color} successfully inserted into PrefSuite DB.",
                      nameof(SQLRepository),
                      nameof(GetPrefSuiteColorConfigurationAsync),
                      color);
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Color configuration for color {$Color} already exists in PrefSuite DB.",
                      nameof(SQLRepository),
                      nameof(GetPrefSuiteColorConfigurationAsync),
                     color);

                }
                return result;

            }
            catch (Exception ex)
            {
                _logService.Verbose(
                "{$Class}.{$Method}. Unhandled error inserting color configuration for color {$Color}. Exception: {$Exception}.",
                nameof(SQLRepository),
                nameof(GetPrefSuiteColorConfigurationAsync),
                color,
                ex.Message
               );
                return result;
            }

        }
        public async Task<ErrorEntity?> DeleteSalesDocumentDataAsync(int number, int version, bool deleteExisting)
        {

            if (number < 1 || version < 1)
            {
                _logService.Error("{$Class}.{$Method}. Error deleting sales document data. Number {$Number} or version {$Version} are wrong.",
                 nameof(SQLRepository),
                   nameof(DeleteSalesDocumentDataAsync),
                   number,
                   version);
                return new ErrorEntity()
                {
                    OrderNumber = string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"Error {nameof(SQLRepository)}.{nameof(DeleteSalesDocumentDataAsync)}.  "
                };
            }

            try
            {
                var delete = deleteExisting ? 1 : 0;

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_DeleteExistingData]",
                    CommandType = CommandType.StoredProcedure
                };

                _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", number);
                _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", version);
                _ = cmd.Parameters.AddWithValue("@DeleteExisting", delete);

                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return null;

            }
            catch (Exception ex)
            {
                _logService.Verbose(
                "{$Class}.{$Method}. Unhandled error in {$Class}. {$Method}. Error deleting sales document data for sales document {$Number}/{$Version} . Exception: {$Exception}.",
                nameof(SQLRepository),
                nameof(DeleteSalesDocumentDataAsync),
                number,
                version,
                ex.Message
               );
                return new ErrorEntity()
                {
                    OrderNumber = string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"Error {nameof(SQLRepository)}.{nameof(DeleteSalesDocumentDataAsync)}." +
                    $"\nError deleting sales document data for sales document {number}/{version}." +
                    $"\n{ex.Message}.  "
                };
            }

        }

        public async Task<ErrorEntity?> InsertOrderMaterialAsync(MaterialEntity material, int number, int version)
        {

            DateTime dateTime = DateTime.UtcNow;



            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertMaterial]",
                    CommandType = CommandType.StoredProcedure
                };
                _ = cmd.Parameters.AddWithValue("@RowId", material.RowId); //required

                _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", number); //required
                _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", version); //required
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Order", material.Order); //required
                _ = cmd.Parameters.AddWithValue("@Worksheet", material.Worksheet); //required
                _ = cmd.Parameters.AddWithValue("@Line", material.Line); //required
                _ = cmd.Parameters.AddWithValue("@Column", material.Column); //required 
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Item", material.Item ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@SortOrder", material.SortOrder);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase);
                _ = cmd.Parameters.AddWithValue("@Reference", material.Reference);
                _ = cmd.Parameters.AddWithValue("@Description", material.Description ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@Color", material.Color);
                _ = cmd.Parameters.AddWithValue("@ColorDescription", material.ColorDescription ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Width", Math.Round(material.Width, 4));
                _ = cmd.Parameters.AddWithValue("@Height", Math.Round(material.Height, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Quantity", material.Quantity);
                _ = cmd.Parameters.AddWithValue("@PackageQuantity", Math.Round(material.PackageQuantity, 4));
                _ = cmd.Parameters.AddWithValue("@TotalQuantity", Math.Round(material.TotalQuantity, 4));
                _ = cmd.Parameters.AddWithValue("@RequiredQuantity", Math.Round(material.RequiredQuantity, 4));
                _ = cmd.Parameters.AddWithValue("@LeftOverQuantity", Math.Round(material.LeftOverQuantity, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Weight", Math.Round(material.Weight, 4));
                _ = cmd.Parameters.AddWithValue("@TotalWeight", Math.Round(material.TotalWeight, 4));
                _ = cmd.Parameters.AddWithValue("@RequiredWeight", Math.Round(material.RequiredWeight, 4));
                _ = cmd.Parameters.AddWithValue("@LeftOverWeight", Math.Round(material.LeftOverWeight, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Area", Math.Round(material.Area, 4));
                _ = cmd.Parameters.AddWithValue("@TotalArea", Math.Round(material.TotalArea, 4));
                _ = cmd.Parameters.AddWithValue("@RequiredArea", Math.Round(material.RequiredArea, 4));
                _ = cmd.Parameters.AddWithValue("@LeftOverArea", Math.Round(material.LeftOverArea, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Waste", Math.Round(material.Waste, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Price", Math.Round(material.Price, 4));
                _ = cmd.Parameters.AddWithValue("@TotalPrice", Math.Round(material.TotalPrice, 4));
                _ = cmd.Parameters.AddWithValue("@RequiredPrice", Math.Round(material.RequiredPrice, 4));
                _ = cmd.Parameters.AddWithValue("@LeftOverPrice", Math.Round(material.LeftOverPrice, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@SquareMeterPrice", Math.Round(material.SquareMeterPrice, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Pallet", material.Pallet ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@MaterialType", material.MaterialType);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@WorksheetType", material.WorksheetType);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@CustomField1", material.CustomField1 ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@CustomField2", material.CustomField2 ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@CustomField3", material.CustomField3 ?? (object)DBNull.Value);
                //========================================================================================================;
                _ = cmd.Parameters.AddWithValue("@CustomField4", material.CustomField4 ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@CustomField5", material.CustomField5 ?? (object)DBNull.Value);
                //========================================================================================================    
                _ = cmd.Parameters.AddWithValue("@SourceReference", material.SourceReference ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@SourceDescription", material.SourceDescription ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@SourceColor", material.SourceColor ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@SourceColorDescription", material.SourceColorDescription ?? (object)DBNull.Value);
                //========================================================================================================    
                _ = cmd.Parameters.AddWithValue("@CreatedUTCDateTime", dateTime);
                _ = cmd.Parameters.AddWithValue("@ModifiedUTCDateTime", dateTime);

                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                _logService.Verbose("{$Class}.{$Method}. Order: {$Order}, worksheet {$Worksheet}, line {$Line}, reference {$Reference}, color {$Color}, successfully inserted into DB.",
                 nameof(SQLRepository),
                      nameof(InsertOrderMaterialAsync),
                      material.Order,
                      material.Worksheet,
                      material.Line,
                      material.Reference,
                      material.Color ?? "Without");

                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nReferenceBase {$ReferenceBase}, " +
                "\nReference {$Reference}," +
                "\nColor {$Color}, " +
                "\nColor {$ColorDescription}, " +
                "\nDescription {$Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(InsertOrderMaterialAsync),
                material.Order ?? string.Empty,
                material.Worksheet ?? string.Empty,
                material.Line,
                material.ReferenceBase ?? string.Empty,
                material.Reference ?? string.Empty,
                material.Color ?? string.Empty,
                 material.ColorDescription ?? string.Empty,
                material.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = material.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertOrderMaterialAsync)}. Unhandled error." +
                   $"\nOrder {material.Order ?? string.Empty}," +
                   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                   $"\nLine {material.Line}," +
                   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {material.Reference ?? string.Empty}," +
                   $"\nColor {material.Color ?? string.Empty}, " +
                   $"\nColorDescription {material.ColorDescription ?? string.Empty}, " +
                   $"\nDescription {material.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }
        }
        public async Task<ErrorEntity?> InsertOrderItemAsync(ItemEntity item, int number, int version, string idPos)
        {

            DateTime dateTime = DateTime.UtcNow;

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertItem]",
                    CommandType = CommandType.StoredProcedure
                };

                _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", number); //required
                _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", version);//required 
                _ = cmd.Parameters.AddWithValue("@SalesDocumentIdPos", idPos.ToString()); //required
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Order", item.Order); //required
                _ = cmd.Parameters.AddWithValue("@Worksheet", item.Worksheet); //require
                _ = cmd.Parameters.AddWithValue("@Line", item.Line); //required
                _ = cmd.Parameters.AddWithValue("@Column", item.Column); //required
                                                                         //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Project", item.Project ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@Item", item.ItemName ?? (object)DBNull.Value);//required
                _ = cmd.Parameters.AddWithValue("@SortOrder", item.SortOrder); //required
                _ = cmd.Parameters.AddWithValue("@Description", item.Description ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Width", Math.Round(item.Width, 4));
                _ = cmd.Parameters.AddWithValue("@Height", Math.Round(item.Height, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Weight", Math.Round(item.Weight, 4));
                _ = cmd.Parameters.AddWithValue("@WeightWithoutGlass", Math.Round(item.WeightWithoutGlass, 4));
                _ = cmd.Parameters.AddWithValue("@WeightGlass", Math.Round(item.WeightGlass, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@TotalWeight", Math.Round(item.TotalWeight, 4));
                _ = cmd.Parameters.AddWithValue("@TotalWeightWithoutGlass", Math.Round(item.TotalWeightWithoutGlass, 4));
                _ = cmd.Parameters.AddWithValue("@TotalWeightGlass", Math.Round(item.TotalWeightGlass, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Area", Math.Round(item.Area, 4));
                _ = cmd.Parameters.AddWithValue("@TotalArea", Math.Round(item.TotalArea, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Hours", Math.Round(item.Hours, 4));
                _ = cmd.Parameters.AddWithValue("@TotalHours", Math.Round(item.TotalHours, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@MaterialCost", Math.Round(item.MaterialCost, 4));
                _ = cmd.Parameters.AddWithValue("@LaborCost", Math.Round(item.LaborCost, 4));
                _ = cmd.Parameters.AddWithValue("@Cost", Math.Round(item.Cost, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@TotalMaterialCost", Math.Round(item.TotalMaterialCost, 4));
                _ = cmd.Parameters.AddWithValue("@TotalLaborCost", Math.Round(item.TotalLaborCost, 4));
                _ = cmd.Parameters.AddWithValue("@TotalCost", Math.Round(item.TotalCost, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Price", Math.Round(item.Price, 4));
                _ = cmd.Parameters.AddWithValue("@TotalPrice", Math.Round(item.TotalPrice, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@CurrencyCode", item.CurrencyCode ?? string.Empty);
                _ = cmd.Parameters.AddWithValue("@ExchangeRateEUR", Math.Round(item.ExchangeRateEUR, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@MaterialCostEUR", Math.Round(item.MaterialCostEUR, 4));
                _ = cmd.Parameters.AddWithValue("@LaborCostEUR", Math.Round(item.LaborCostEUR, 4));
                _ = cmd.Parameters.AddWithValue("@CostEUR", Math.Round(item.CostEUR, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@TotalMaterialCostEUR", Math.Round(item.TotalMaterialCostEUR, 4));
                _ = cmd.Parameters.AddWithValue("@TotalLaborCostEUR", Math.Round(item.TotalLaborCostEUR, 4));
                _ = cmd.Parameters.AddWithValue("@TotalCostEUR", Math.Round(item.TotalCostEUR, 4));
                //========================================================================================================    
                _ = cmd.Parameters.AddWithValue("@PriceEUR", Math.Round(item.PriceEUR, 4));
                _ = cmd.Parameters.AddWithValue("@TotalPriceEUR", Math.Round(item.TotalPriceEUR, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@WorksheetType", item.WorksheetType); //Required
                                                                                       //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@CreatedUTCDateTime", dateTime); //Required
                _ = cmd.Parameters.AddWithValue("@ModifiedUTCDateTime", dateTime); //Required

                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nItem {Item}, " +
                "\nDescription {Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(InsertOrderItemAsync),
                item.Order ?? string.Empty,
                item.Worksheet ?? string.Empty,
                item.Line,
                item.ItemName ?? string.Empty,
                item.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = item.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertOrderItemAsync)}. Unhandled error." +
                   $"\nOrder {item.Order ?? string.Empty}," +
                   $"\nWorksheet {item.Worksheet ?? string.Empty}," +
                   $"\nLine {item.Line}," +
                   $"\nReferenceBase {item.ItemName ?? string.Empty}, " +
                   $"\nReference {item.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"
                };
            }

        }
        public async Task<ErrorEntity?> InsertPrefSuiteColorAsync(MaterialEntity material)
        {
            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteColor]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Color", material.Color); //required
                _ = cmd.Parameters.AddWithValue("@ColorDescription", material.ColorDescription); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Color {$Color}, {$ColorDescription} successfully inserted into PrefSuite DB.", material.Color, material.ColorDescription ?? "Without");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Color {$Color}, {$ColorDescription} already exists in PrefSuite DB.", material.Color, material.ColorDescription ?? "Without");

                }
                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nReferenceBase {$ReferenceBase}, " +
                "\nReference {$Reference}," +
                "\nColor {$Color}, " +
                "\nColor {$ColorDescription}, " +
                "\nDescription {$Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteColorAsync),
                material.Order ?? string.Empty,
                material.Worksheet ?? string.Empty,
                material.Line,
                material.ReferenceBase ?? string.Empty,
                material.Reference ?? string.Empty,
                material.Color ?? string.Empty,
                 material.ColorDescription ?? string.Empty,
                material.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = material.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteColorAsync)}. Unhandled error." +
                   $"\nOrder {material.Order ?? string.Empty}," +
                   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                   $"\nLine {material.Line}," +
                   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {material.Reference ?? string.Empty}," +
                   $"\nColor {material.Color ?? string.Empty}, " +
                   $"\nColorDescription {material.ColorDescription ?? string.Empty}, " +
                   $"\nDescription {material.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<ErrorEntity?> InsertPrefSuiteColorConfigurationAsync(MaterialEntity material)
        {

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteColorConfiguration]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Color", material.Color); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Color configuration for color {$Color} successfully inserted into PrefSuite DB.", material.Color);
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Color configuration for color {$Color} already exists in PrefSuite DB.", material.Color);

                }
                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nReferenceBase {$ReferenceBase}, " +
                "\nReference {$Reference}," +
                "\nColor {$Color}, " +
                "\nColor {$ColorDescription}, " +
                "\nDescription {$Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteColorConfigurationAsync),
                material.Order ?? string.Empty,
                material.Worksheet ?? string.Empty,
                material.Line,
                material.ReferenceBase ?? string.Empty,
                material.Reference ?? string.Empty,
                material.Color ?? string.Empty,
                 material.ColorDescription ?? string.Empty,
                material.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = material.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteColorConfigurationAsync)}. Unhandled error." +
                   $"\nOrder {material.Order ?? string.Empty}," +
                   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                   $"\nLine {material.Line}," +
                   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {material.Reference ?? string.Empty}," +
                   $"\nColor {material.Color ?? string.Empty}, " +
                   $"\nColorDescription {material.ColorDescription ?? string.Empty}, " +
                   $"\nDescription {material.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<ErrorEntity?> InsertPrefSuiteMaterialBaseAsync(MaterialEntity material)
        {

            try

            {

                material.CommodityCode = await GetCommodityCode(material.SourceReference ?? string.Empty);


                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialBase]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Description", material.Description ?? ""); //required
                _ = cmd.Parameters.AddWithValue("@MaterialType", material.MaterialType); //required
                _ = cmd.Parameters.AddWithValue("@CommodityCode", material.CommodityCode ?? (object)DBNull.Value);

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Material Base {$ReferenceBase} {$Description} successfully inserted into PrefSuite DB",
                        nameof(SQLRepository),
                        nameof(InsertPrefSuiteMaterialBaseAsync),
                        material.ReferenceBase,
                        material.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Material {$Reference} {$Description} already exists in PrefSuite DB.",
                        nameof(SQLRepository),
                        nameof(InsertPrefSuiteMaterialBaseAsync),
                        material.ReferenceBase,
                        material.Description ?? "");

                }
                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nReferenceBase {$ReferenceBase}, " +
                "\nReference {$Reference}," +
                "\nColor {$Color}, " +
                "\nDescription {$Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialBaseAsync),
                material.Order ?? string.Empty,
                material.Worksheet ?? string.Empty,
                material.Line,
                material.ReferenceBase ?? string.Empty,
                material.Reference ?? string.Empty,
                material.Color ?? string.Empty,
                material.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = material.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialBaseAsync)}. Unhandled error." +
                   $"\nOrder {material.Order ?? string.Empty}," +
                   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                   $"\nLine {material.Line}," +
                   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {material.Reference ?? string.Empty}," +
                   $"\nColor {material.Color ?? string.Empty}, " +
                   $"\nDescription {material.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<ErrorEntity?> InsertPrefSuiteMaterialAsync(MaterialEntity material)
        {

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterial]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Reference", material.Reference); //required
                _ = cmd.Parameters.AddWithValue("@Color", material.Color); //required
                _ = cmd.Parameters.AddWithValue("@PackageQuantity", material.PackageQuantity); //required
                _ = cmd.Parameters.AddWithValue("@Weight", material.Weight); //required
                _ = cmd.Parameters.AddWithValue("@MaterialType", material.MaterialType); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialAsync),
                material.Reference,
                material.Color,
                material.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialAsync),
                material.Reference, material.Color,
                material.Description ?? "");

                }
                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nReferenceBase {$ReferenceBase}, " +
                "\nReference {$Reference}," +
                "\nColor {$Color}, " +
                "\nDescription {$Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialAsync),
                material.Order ?? string.Empty,
                material.Worksheet ?? string.Empty,
                material.Line,
                material.ReferenceBase ?? string.Empty,
                material.Reference ?? string.Empty,
                material.Color ?? string.Empty,
                material.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = material.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialAsync)}. Unhandled error." +
                   $"\nOrder {material.Order ?? string.Empty}," +
                   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                   $"\nLine {material.Line}," +
                   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {material.Reference ?? string.Empty}," +
                   $"\nColor {material.Color ?? string.Empty}, " +
                   $"\nDescription {material.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<ErrorEntity?> InsertPrefSuiteMaterialProfileAsync(MaterialEntity material)
        {

            try

            {
                if (material.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                    material.Weight = weight;
                }


                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialProfile]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@PackageQuantity", material.PackageQuantity); //required
                _ = cmd.Parameters.AddWithValue("@Weight", material.Weight); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Profile {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialProfileAsync),
                material.Reference,
                material.Color,
                material.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Profile {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialProfileAsync),
                material.Reference,
                material.Color,
                material.Description ?? "");

                }
                return null;

            }

            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nReferenceBase {$ReferenceBase}, " +
                "\nReference {$Reference}," +
                "\nColor {$Color}, " +
                "\nDescription {$Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialProfileAsync),
                material.Order ?? string.Empty,
                material.Worksheet ?? string.Empty,
                material.Line,
                material.ReferenceBase ?? string.Empty,
                material.Reference ?? string.Empty,
                material.Color ?? string.Empty,
                material.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = material.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialProfileAsync)}. Unhandled error." +
                   $"\nOrder {material.Order ?? string.Empty}," +
                   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                   $"\nLine {material.Line}," +
                   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {material.Reference ?? string.Empty}," +
                   $"\nColor {material.Color ?? string.Empty}, " +
                   $"\nDescription {material.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }


        public async Task<ErrorEntity?> InsertPrefSuiteMaterialMeterAsync(MaterialEntity material)
        {

            try

            {



                if (material.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                    material.Weight = weight;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialMeter]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Weight", material.Weight); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Meter material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialMeterAsync),
                material.Reference,
                material.Color,
                material.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Meter material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialMeterAsync),
                material.Reference,
                material.Color,
                material.Description ?? "");

                }
                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nReferenceBase {$ReferenceBase}, " +
                "\nReference {$Reference}," +
                "\nColor {$Color}, " +
                "\nDescription {$Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialMeterAsync),
                material.Order ?? string.Empty,
                material.Worksheet ?? string.Empty,
                material.Line,
                material.ReferenceBase ?? string.Empty,
                material.Reference ?? string.Empty,
                material.Color ?? string.Empty,
                material.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = material.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialMeterAsync)}. Unhandled error." +
                   $"\nOrder {material.Order ?? string.Empty}," +
                   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                   $"\nLine {material.Line}," +
                   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {material.Reference ?? string.Empty}," +
                   $"\nColor {material.Color ?? string.Empty}, " +
                   $"\nDescription {material.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<ErrorEntity?> InsertPrefSuiteMaterialPieceAsync(MaterialEntity material)
        {

            try
            {

                if (material.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                    material.Weight = weight;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialPiece]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Weight", material.Weight); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Piece material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                                     nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialPieceAsync),
                material.Reference,
                material.Color,
                material.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Piece material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                        nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialPieceAsync),
                material.Reference,
                material.Color,
                material.Description ?? "");

                }
                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nReferenceBase {$ReferenceBase}, " +
                "\nReference {$Reference}," +
                "\nColor {$Color}, " +
                "\nDescription {$Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialPieceAsync),
                material.Order ?? string.Empty,
                material.Worksheet ?? string.Empty,
                material.Line,
                material.ReferenceBase ?? string.Empty,
                material.Reference ?? string.Empty,
                material.Color ?? string.Empty,
                material.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = material.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialPieceAsync)}. Unhandled error." +
                   $"\nOrder {material.Order ?? string.Empty}," +
                   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                   $"\nLine {material.Line}," +
                   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {material.Reference ?? string.Empty}," +
                   $"\nColor {material.Color ?? string.Empty}, " +
                   $"\nDescription {material.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<ErrorEntity?> InsertPrefSuiteMaterialSurfaceAsync(MaterialEntity material)
        {

            try

            {
                if (material.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                    material.Weight = weight;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPreSuiteMaterialSurface]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required   
                _ = cmd.Parameters.AddWithValue("@Weight", material.Weight); //required
                _ = cmd.Parameters.AddWithValue("@MaterialType", material.MaterialType); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("($Class}.{$Method}. Surface material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                        nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialSurfaceAsync),
                material.Reference,
                material.Color,
                material.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("($Class}.{$Method}. Surface material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                        nameof(SQLRepository),
                        nameof(InsertPrefSuiteMaterialSurfaceAsync),
                        material.Reference,
                        material.Color,
                        material.Description ?? "");

                }
                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nReferenceBase {$ReferenceBase}, " +
                "\nReference {$Reference}," +
                "\nColor {$Color}, " +
                "\nDescription {$Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialSurfaceAsync),
                material.Order ?? string.Empty,
                material.Worksheet ?? string.Empty,
                material.Line,
                material.ReferenceBase ?? string.Empty,
                material.Reference ?? string.Empty,
                material.Color ?? string.Empty,
                material.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = material.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialSurfaceAsync)}. Unhandled error." +
                   $"\nOrder {material.Order ?? string.Empty}," +
                   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                   $"\nLine {material.Line}," +
                   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {material.Reference ?? string.Empty}," +
                   $"\nColor {material.Color ?? string.Empty}, " +
                   $"\nDescription {material.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }

        public async Task<ErrorEntity?> InsertPrefSuiteMaterialPurchaseDataAsync(MaterialEntity material)
        {



            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialPurchaseData]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Reference", material.Reference); //required   
                _ = cmd.Parameters.AddWithValue("@Package", material.PackageQuantity); //required
                _ = cmd.Parameters.AddWithValue("@Price", material.Price); //required
                _ = cmd.Parameters.AddWithValue("@Description", material.Description); //required
                _ = cmd.Parameters.AddWithValue("@Color", material.Color); //required
                _ = cmd.Parameters.AddWithValue("@SourceReference", material.SourceReference); //required
                _ = cmd.Parameters.AddWithValue("@SourceColor", material.SourceColor); //required
                _ = cmd.Parameters.AddWithValue("@MaterialType", material.MaterialType); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("($Class}.{$Method}. Purchase data material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                        nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialPurchaseDataAsync),
                material.Reference,
                material.Color,
                material.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("($Class}.{$Method}. Purchase data material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                        nameof(SQLRepository),
                        nameof(InsertPrefSuiteMaterialPurchaseDataAsync),
                        material.Reference,
                        material.Color,
                        material.Description ?? "");

                }
                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nReferenceBase {$ReferenceBase}, " +
                "\nReference {$Reference}," +
                "\nColor {$Color}, " +
                "\nDescription {$Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialPurchaseDataAsync),
                material.Order ?? string.Empty,
                material.Worksheet ?? string.Empty,
                material.Line,
                material.ReferenceBase ?? string.Empty,
                material.Reference ?? string.Empty,
                material.Color ?? string.Empty,
                material.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = material.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialPurchaseDataAsync)}. Unhandled error." +
                   $"\nOrder {material.Order ?? string.Empty}," +
                   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                   $"\nLine {material.Line}," +
                   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {material.Reference ?? string.Empty}," +
                   $"\nColor {material.Color ?? string.Empty}, " +
                   $"\nDescription {material.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }



        public async Task<ErrorEntity?> UpdateBCMapping(MaterialEntity material)
        {

            try

            {


                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_UpdateBCMapping]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase ?? (object)DBNull.Value); //required   
                _ = cmd.Parameters.AddWithValue("@Reference", material.Reference ?? (object)DBNull.Value); //required   
                _ = cmd.Parameters.AddWithValue("@SourceReference", material.SourceReference ?? (object)DBNull.Value); //required   
                _ = cmd.Parameters.AddWithValue("@SourceColor", material.SourceColor ?? (object)DBNull.Value); //required  
                _ = cmd.Parameters.AddWithValue("@SourceColor1", material.CustomField1 ?? (object)DBNull.Value); //required  
                _ = cmd.Parameters.AddWithValue("@SourceColor2", material.CustomField2 ?? (object)DBNull.Value); //required  

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("($Class}.{$Method}. BC Mapping  {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                        nameof(SQLRepository),
                nameof(UpdateBCMapping),
                material.Reference,
                material.Color,
                material.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("($Class}.{$Method}. BC Mapping {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                        nameof(SQLRepository),
                        nameof(UpdateBCMapping),
                        material.Reference,
                        material.Color,
                        material.Description ?? "");

                }
                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nWorksheet {$Worksheet}," +
                "\nLine {$Line}," +
                "\nReferenceBase {$ReferenceBase}, " +
                "\nReference {$Reference}," +
                "\nColor {$Color}, " +
                "\nDescription {$Description}," +
                "\nException: {$Exception}",
                nameof(SQLRepository),
                nameof(UpdateBCMapping),
                material.Order ?? string.Empty,
                material.Worksheet ?? string.Empty,
                material.Line,
                material.ReferenceBase ?? string.Empty,
                material.Reference ?? string.Empty,
                material.Color ?? string.Empty,
                material.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = material.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialSurfaceAsync)}. Unhandled error." +
                   $"\nOrder {material.Order ?? string.Empty}," +
                   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                   $"\nLine {material.Line}," +
                   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {material.Reference ?? string.Empty}," +
                   $"\nColor {material.Color ?? string.Empty}, " +
                   $"\nDescription {material.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }



        public async Task<ErrorEntity?> InsertPrefSuiteMaterialNeedsMasterAsync(string order, int number, int version)
        {

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialNeedsMaster]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Number", number); //required
                _ = cmd.Parameters.AddWithValue("@Version", version); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nSalesDocument {$Number}/{$Version}." +
                "\nException {$Exception}.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialNeedsMasterAsync),
                order ?? string.Empty,
                number,
                version,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialNeedsMasterAsync)}. Unhandled error." +
                    $"\nOrder {order ?? string.Empty}," +
                    $"\nSalesDocument {number}/{version}." +
                    $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<ErrorEntity?> InsertPrefSuiteMaterialNeedsAsync(string order, int number, int version)
        {

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialNeeds]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Number", number); //required
                _ = cmd.Parameters.AddWithValue("@Version", version); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nSalesDocument {$Number}/{$Version}." +
                "\nException {$Exception}.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialNeedsAsync),
                order ?? string.Empty,
                number,
                version,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialNeedsAsync)}. Unhandled error." +
                    $"\nOrder {order ?? string.Empty}," +
                    $"\nSalesDocument {number}/{version}." +
                    $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }


    }
}
