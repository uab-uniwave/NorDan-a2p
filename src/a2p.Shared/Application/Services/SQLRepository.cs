// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Shared.Application.Services
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
        public async Task<A2PError?> DeleteSalesDocumentDataAsync(int number, int version)
        {

            if (number < 1 || version < 1)
            {
                _logService.Error("{$Class}.{$Method}. Error deleting sales document data. Number {$Number} or version {$Version} are wrong.",
                 nameof(SQLRepository),
                   nameof(DeleteSalesDocumentDataAsync),
                   number,
                   version);
                return new A2PError()
                {
                    Order = string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"Error {nameof(SQLRepository)}.{nameof(DeleteSalesDocumentDataAsync)}.  "
                };
            }

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_DeleteExistingData]",
                    CommandType = CommandType.StoredProcedure
                };

                _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", number);
                _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", version);

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
                return new A2PError()
                {
                    Order = string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"Error {nameof(SQLRepository)}.{nameof(DeleteSalesDocumentDataAsync)}." +
                    $"\nError deleting sales document data for sales document {number}/{version}." +
                    $"\n{ex.Message}.  "
                };
            }

        }

        public async Task<A2PError?> InsertOrderMaterialDTOAsync(MaterialDTO materialDTO, int number, int version)
        {

            DateTime dateTime = DateTime.UtcNow;



            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertMaterial]",
                    CommandType = CommandType.StoredProcedure
                };

                _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", number); //required
                _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", version); //required
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Order", materialDTO.Order); //required
                _ = cmd.Parameters.AddWithValue("@Worksheet", materialDTO.Worksheet); //required
                _ = cmd.Parameters.AddWithValue("@Line", materialDTO.Line); //required
                _ = cmd.Parameters.AddWithValue("@Column", materialDTO.Column); //required 
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Item", materialDTO.Item ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@SortOrder", materialDTO.SortOrder);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", materialDTO.ReferenceBase);
                _ = cmd.Parameters.AddWithValue("@Reference", materialDTO.Reference);
                _ = cmd.Parameters.AddWithValue("@Description", materialDTO.Description ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@Color", materialDTO.Color);
                _ = cmd.Parameters.AddWithValue("@ColorDescription", materialDTO.ColorDescription ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Width", Math.Round(materialDTO.Width, 4));
                _ = cmd.Parameters.AddWithValue("@Height", Math.Round(materialDTO.Height, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Quantity", materialDTO.Quantity);
                _ = cmd.Parameters.AddWithValue("@PackageQuantity", Math.Round(materialDTO.PackageQuantity, 4));
                _ = cmd.Parameters.AddWithValue("@TotalQuantity", Math.Round(materialDTO.TotalQuantity, 4));
                _ = cmd.Parameters.AddWithValue("@RequiredQuantity", Math.Round(materialDTO.RequiredQuantity, 4));
                _ = cmd.Parameters.AddWithValue("@LeftOverQuantity", Math.Round(materialDTO.LeftOverQuantity, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Weight", Math.Round(materialDTO.Weight, 4));
                _ = cmd.Parameters.AddWithValue("@TotalWeight", Math.Round(materialDTO.TotalWeight, 4));
                _ = cmd.Parameters.AddWithValue("@RequiredWeight", Math.Round(materialDTO.RequiredWeight, 4));
                _ = cmd.Parameters.AddWithValue("@LeftOverWeight", Math.Round(materialDTO.LeftOverWeight, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Area", Math.Round(materialDTO.Area, 4));
                _ = cmd.Parameters.AddWithValue("@TotalArea", Math.Round(materialDTO.TotalArea, 4));
                _ = cmd.Parameters.AddWithValue("@RequiredArea", Math.Round(materialDTO.RequiredArea, 4));
                _ = cmd.Parameters.AddWithValue("@LeftOverArea", Math.Round(materialDTO.LeftOverArea, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Waste", Math.Round(materialDTO.Waste, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Price", Math.Round(materialDTO.Price, 4));
                _ = cmd.Parameters.AddWithValue("@TotalPrice", Math.Round(materialDTO.TotalPrice, 4));
                _ = cmd.Parameters.AddWithValue("@RequiredPrice", Math.Round(materialDTO.RequiredPrice, 4));
                _ = cmd.Parameters.AddWithValue("@LeftOverPrice", Math.Round(materialDTO.LeftOverPrice, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@SquareMeterPrice", Math.Round(materialDTO.SquareMeterPrice, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Pallet", materialDTO.Pallet ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@MaterialType", materialDTO.MaterialType);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@WorksheetType", materialDTO.WorksheetType);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@CustomField1", materialDTO.CustomField1 ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@CustomField2", materialDTO.CustomField2 ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@CustomField3", materialDTO.CustomField3 ?? (object)DBNull.Value);
                //========================================================================================================;
                _ = cmd.Parameters.AddWithValue("@CustomField4", materialDTO.CustomField4 ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@CustomField5", materialDTO.CustomField5 ?? (object)DBNull.Value);
                //========================================================================================================    
                _ = cmd.Parameters.AddWithValue("@SourceReference", materialDTO.SourceReference ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@SourceDescription", materialDTO.SourceDescription ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@SourceColor", materialDTO.SourceColor ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@SourceColorDescription", materialDTO.SourceColorDescription ?? (object)DBNull.Value);
                //========================================================================================================    
                _ = cmd.Parameters.AddWithValue("@CreatedUTCDateTime", dateTime);
                _ = cmd.Parameters.AddWithValue("@ModifiedUTCDateTime", dateTime);

                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                _logService.Verbose("{$Class}.{$Method}. Order: {$Order}, worksheet {$Worksheet}, line {$Line}, reference {$Reference}, color {$Color}, successfully inserted into DB.",
                 nameof(SQLRepository),
                      nameof(InsertOrderMaterialDTOAsync),
                      materialDTO.Order,
                      materialDTO.Worksheet,
                      materialDTO.Line,
                      materialDTO.Reference,
                      materialDTO.Color ?? "Without");

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
                nameof(InsertOrderMaterialDTOAsync),
                materialDTO.Order ?? string.Empty,
                materialDTO.Worksheet ?? string.Empty,
                materialDTO.Line,
                materialDTO.ReferenceBase ?? string.Empty,
                materialDTO.Reference ?? string.Empty,
                materialDTO.Color ?? string.Empty,
                 materialDTO.ColorDescription ?? string.Empty,
                materialDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = materialDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertOrderMaterialDTOAsync)}. Unhandled error." +
                   $"\nOrder {materialDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {materialDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {materialDTO.Line}," +
                   $"\nReferenceBase {materialDTO.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {materialDTO.Reference ?? string.Empty}," +
                   $"\nColor {materialDTO.Color ?? string.Empty}, " +
                   $"\nColorDescription {materialDTO.ColorDescription ?? string.Empty}, " +
                   $"\nDescription {materialDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }
        }
        public async Task<A2PError?> InsertOrderItemDTOAsync(ItemDTO itemDTO, int number, int version, string idPos)
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
                _ = cmd.Parameters.AddWithValue("@Order", itemDTO.Order); //required
                _ = cmd.Parameters.AddWithValue("@Worksheet", itemDTO.Worksheet); //require
                _ = cmd.Parameters.AddWithValue("@Line", itemDTO.Line); //required
                _ = cmd.Parameters.AddWithValue("@Column", itemDTO.Column); //required
                                                                            //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Project", itemDTO.Project ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@Item", itemDTO.Item ?? (object)DBNull.Value);//required
                _ = cmd.Parameters.AddWithValue("@SortOrder", itemDTO.SortOrder); //required
                _ = cmd.Parameters.AddWithValue("@Description", itemDTO.Description ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Quantity", itemDTO.Quantity);
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Width", Math.Round(itemDTO.Width, 4));
                _ = cmd.Parameters.AddWithValue("@Height", Math.Round(itemDTO.Height, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Weight", Math.Round(itemDTO.Weight, 4));
                _ = cmd.Parameters.AddWithValue("@WeightWithoutGlass", Math.Round(itemDTO.WeightWithoutGlass, 4));
                _ = cmd.Parameters.AddWithValue("@WeightGlass", Math.Round(itemDTO.WeightGlass, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@TotalWeight", Math.Round(itemDTO.TotalWeight, 4));
                _ = cmd.Parameters.AddWithValue("@TotalWeightWithoutGlass", Math.Round(itemDTO.TotalWeightWithoutGlass, 4));
                _ = cmd.Parameters.AddWithValue("@TotalWeightGlass", Math.Round(itemDTO.TotalWeightGlass, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Area", Math.Round(itemDTO.Area, 4));
                _ = cmd.Parameters.AddWithValue("@TotalArea", Math.Round(itemDTO.TotalArea, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Hours", Math.Round(itemDTO.Hours, 4));
                _ = cmd.Parameters.AddWithValue("@TotalHours", Math.Round(itemDTO.TotalHours, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@MaterialCost", Math.Round(itemDTO.MaterialCost, 4));
                _ = cmd.Parameters.AddWithValue("@LaborCost", Math.Round(itemDTO.LaborCost, 4));
                _ = cmd.Parameters.AddWithValue("@Cost", Math.Round(itemDTO.Cost, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@TotalMaterialCost", Math.Round(itemDTO.TotalMaterialCost, 4));
                _ = cmd.Parameters.AddWithValue("@TotalLaborCost", Math.Round(itemDTO.TotalLaborCost, 4));
                _ = cmd.Parameters.AddWithValue("@TotalCost", Math.Round(itemDTO.TotalCost, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Price", Math.Round(itemDTO.Price, 4));
                _ = cmd.Parameters.AddWithValue("@TotalPrice", Math.Round(itemDTO.TotalPrice, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@CurrencyCode", itemDTO.CurrencyCode ?? string.Empty);
                _ = cmd.Parameters.AddWithValue("@ExchangeRateEUR", Math.Round(itemDTO.ExchangeRateEUR, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@MaterialCostEUR", Math.Round(itemDTO.MaterialCostEUR, 4));
                _ = cmd.Parameters.AddWithValue("@LaborCostEUR", Math.Round(itemDTO.LaborCostEUR, 4));
                _ = cmd.Parameters.AddWithValue("@CostEUR", Math.Round(itemDTO.CostEUR, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@TotalMaterialCostEUR", Math.Round(itemDTO.TotalMaterialCostEUR, 4));
                _ = cmd.Parameters.AddWithValue("@TotalLaborCostEUR", Math.Round(itemDTO.TotalLaborCostEUR, 4));
                _ = cmd.Parameters.AddWithValue("@TotalCostEUR", Math.Round(itemDTO.TotalCostEUR, 4));
                //========================================================================================================    
                _ = cmd.Parameters.AddWithValue("@PriceEUR", Math.Round(itemDTO.PriceEUR, 4));
                _ = cmd.Parameters.AddWithValue("@TotalPriceEUR", Math.Round(itemDTO.TotalPriceEUR, 4));
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@WorksheetType", itemDTO.WorksheetType); //Required
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
                nameof(InsertOrderItemDTOAsync),
                itemDTO.Order ?? string.Empty,
                itemDTO.Worksheet ?? string.Empty,
                itemDTO.Line,
                itemDTO.Item ?? string.Empty,
                itemDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = itemDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertOrderItemDTOAsync)}. Unhandled error." +
                   $"\nOrder {itemDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {itemDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {itemDTO.Line}," +
                   $"\nReferenceBase {itemDTO.Item ?? string.Empty}, " +
                   $"\nReference {itemDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"
                };
            }

        }
        public async Task<A2PError?> InsertPrefSuiteColorAsync(MaterialDTO materialDTO)
        {
            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteColor]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Color", materialDTO.Color); //required
                _ = cmd.Parameters.AddWithValue("@ColorDescription", materialDTO.ColorDescription); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Color {$Color}, {$ColorDescription} successfully inserted into PrefSuite DB.", materialDTO.Color, materialDTO.ColorDescription ?? "Without");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Color {$Color}, {$ColorDescription} already exists in PrefSuite DB.", materialDTO.Color, materialDTO.ColorDescription ?? "Without");

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
                materialDTO.Order ?? string.Empty,
                materialDTO.Worksheet ?? string.Empty,
                materialDTO.Line,
                materialDTO.ReferenceBase ?? string.Empty,
                materialDTO.Reference ?? string.Empty,
                materialDTO.Color ?? string.Empty,
                 materialDTO.ColorDescription ?? string.Empty,
                materialDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = materialDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteColorAsync)}. Unhandled error." +
                   $"\nOrder {materialDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {materialDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {materialDTO.Line}," +
                   $"\nReferenceBase {materialDTO.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {materialDTO.Reference ?? string.Empty}," +
                   $"\nColor {materialDTO.Color ?? string.Empty}, " +
                   $"\nColorDescription {materialDTO.ColorDescription ?? string.Empty}, " +
                   $"\nDescription {materialDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<A2PError?> InsertPrefSuiteColorConfigurationAsync(MaterialDTO materialDTO)
        {

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteColorConfiguration]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Color", materialDTO.Color); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Color configuration for color {$Color} successfully inserted into PrefSuite DB.", materialDTO.Color);
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Color configuration for color {$Color} already exists in PrefSuite DB.", materialDTO.Color);

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
                materialDTO.Order ?? string.Empty,
                materialDTO.Worksheet ?? string.Empty,
                materialDTO.Line,
                materialDTO.ReferenceBase ?? string.Empty,
                materialDTO.Reference ?? string.Empty,
                materialDTO.Color ?? string.Empty,
                 materialDTO.ColorDescription ?? string.Empty,
                materialDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = materialDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteColorConfigurationAsync)}. Unhandled error." +
                   $"\nOrder {materialDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {materialDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {materialDTO.Line}," +
                   $"\nReferenceBase {materialDTO.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {materialDTO.Reference ?? string.Empty}," +
                   $"\nColor {materialDTO.Color ?? string.Empty}, " +
                   $"\nColorDescription {materialDTO.ColorDescription ?? string.Empty}, " +
                   $"\nDescription {materialDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<A2PError?> InsertPrefSuiteMaterialBaseAsync(MaterialDTO materialDTO)
        {

            try

            {

                materialDTO.CommodityCode = await GetCommodityCode(materialDTO.SourceReference ?? string.Empty);


                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialBase]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", materialDTO.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Description", materialDTO.Description ?? ""); //required
                _ = cmd.Parameters.AddWithValue("@MaterialType", materialDTO.MaterialType); //required
                _ = cmd.Parameters.AddWithValue("@CommodityCode", materialDTO.CommodityCode ?? (object)DBNull.Value);

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Material Base {$ReferenceBase} {$Description} successfully inserted into PrefSuite DB",
                        nameof(SQLRepository),
                        nameof(InsertPrefSuiteMaterialBaseAsync),
                        materialDTO.ReferenceBase,
                        materialDTO.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Material {$Reference} {$Description} already exists in PrefSuite DB.",
                        nameof(SQLRepository),
                        nameof(InsertPrefSuiteMaterialBaseAsync),
                        materialDTO.ReferenceBase,
                        materialDTO.Description ?? "");

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
                materialDTO.Order ?? string.Empty,
                materialDTO.Worksheet ?? string.Empty,
                materialDTO.Line,
                materialDTO.ReferenceBase ?? string.Empty,
                materialDTO.Reference ?? string.Empty,
                materialDTO.Color ?? string.Empty,
                materialDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = materialDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialBaseAsync)}. Unhandled error." +
                   $"\nOrder {materialDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {materialDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {materialDTO.Line}," +
                   $"\nReferenceBase {materialDTO.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {materialDTO.Reference ?? string.Empty}," +
                   $"\nColor {materialDTO.Color ?? string.Empty}, " +
                   $"\nDescription {materialDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<A2PError?> InsertPrefSuiteMaterialAsync(MaterialDTO materialDTO)
        {

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterial]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", materialDTO.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Reference", materialDTO.Reference); //required
                _ = cmd.Parameters.AddWithValue("@Color", materialDTO.Color); //required
                _ = cmd.Parameters.AddWithValue("@PackageQuantity", materialDTO.PackageQuantity); //required
                _ = cmd.Parameters.AddWithValue("@Weight", materialDTO.Weight); //required
                _ = cmd.Parameters.AddWithValue("@MaterialType", materialDTO.MaterialType); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialAsync),
                materialDTO.Reference,
                materialDTO.Color,
                materialDTO.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialAsync),
                materialDTO.Reference, materialDTO.Color,
                materialDTO.Description ?? "");

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
                materialDTO.Order ?? string.Empty,
                materialDTO.Worksheet ?? string.Empty,
                materialDTO.Line,
                materialDTO.ReferenceBase ?? string.Empty,
                materialDTO.Reference ?? string.Empty,
                materialDTO.Color ?? string.Empty,
                materialDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = materialDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialAsync)}. Unhandled error." +
                   $"\nOrder {materialDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {materialDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {materialDTO.Line}," +
                   $"\nReferenceBase {materialDTO.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {materialDTO.Reference ?? string.Empty}," +
                   $"\nColor {materialDTO.Color ?? string.Empty}, " +
                   $"\nDescription {materialDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<A2PError?> InsertPrefSuiteMaterialProfileAsync(MaterialDTO materialDTO)
        {

            try

            {
                if (materialDTO.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(materialDTO.SourceReference ?? string.Empty);
                    materialDTO.Weight = weight;
                }


                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialProfile]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", materialDTO.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@PackageQuantity", materialDTO.PackageQuantity); //required
                _ = cmd.Parameters.AddWithValue("@Weight", materialDTO.Weight); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Profile {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialProfileAsync),
                materialDTO.Reference,
                materialDTO.Color,
                materialDTO.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Profile {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialProfileAsync),
                materialDTO.Reference,
                materialDTO.Color,
                materialDTO.Description ?? "");

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
                materialDTO.Order ?? string.Empty,
                materialDTO.Worksheet ?? string.Empty,
                materialDTO.Line,
                materialDTO.ReferenceBase ?? string.Empty,
                materialDTO.Reference ?? string.Empty,
                materialDTO.Color ?? string.Empty,
                materialDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = materialDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialProfileAsync)}. Unhandled error." +
                   $"\nOrder {materialDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {materialDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {materialDTO.Line}," +
                   $"\nReferenceBase {materialDTO.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {materialDTO.Reference ?? string.Empty}," +
                   $"\nColor {materialDTO.Color ?? string.Empty}, " +
                   $"\nDescription {materialDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }


        public async Task<A2PError?> InsertPrefSuiteMaterialMeterAsync(MaterialDTO materialDTO)
        {

            try

            {



                if (materialDTO.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(materialDTO.SourceReference ?? string.Empty);
                    materialDTO.Weight = weight;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialMeter]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", materialDTO.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Weight", materialDTO.Weight); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Meter material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialMeterAsync),
                materialDTO.Reference,
                materialDTO.Color,
                materialDTO.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Meter material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialMeterAsync),
                materialDTO.Reference,
                materialDTO.Color,
                materialDTO.Description ?? "");

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
                materialDTO.Order ?? string.Empty,
                materialDTO.Worksheet ?? string.Empty,
                materialDTO.Line,
                materialDTO.ReferenceBase ?? string.Empty,
                materialDTO.Reference ?? string.Empty,
                materialDTO.Color ?? string.Empty,
                materialDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = materialDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialMeterAsync)}. Unhandled error." +
                   $"\nOrder {materialDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {materialDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {materialDTO.Line}," +
                   $"\nReferenceBase {materialDTO.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {materialDTO.Reference ?? string.Empty}," +
                   $"\nColor {materialDTO.Color ?? string.Empty}, " +
                   $"\nDescription {materialDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<A2PError?> InsertPrefSuiteMaterialPieceAsync(MaterialDTO materialDTO)
        {

            try
            {

                if (materialDTO.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(materialDTO.SourceReference ?? string.Empty);
                    materialDTO.Weight = weight;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialPiece]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", materialDTO.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Weight", materialDTO.Weight); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Piece material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                                     nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialPieceAsync),
                materialDTO.Reference,
                materialDTO.Color,
                materialDTO.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Piece material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                        nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialPieceAsync),
                materialDTO.Reference,
                materialDTO.Color,
                materialDTO.Description ?? "");

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
                materialDTO.Order ?? string.Empty,
                materialDTO.Worksheet ?? string.Empty,
                materialDTO.Line,
                materialDTO.ReferenceBase ?? string.Empty,
                materialDTO.Reference ?? string.Empty,
                materialDTO.Color ?? string.Empty,
                materialDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = materialDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialPieceAsync)}. Unhandled error." +
                   $"\nOrder {materialDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {materialDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {materialDTO.Line}," +
                   $"\nReferenceBase {materialDTO.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {materialDTO.Reference ?? string.Empty}," +
                   $"\nColor {materialDTO.Color ?? string.Empty}, " +
                   $"\nDescription {materialDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<A2PError?> InsertPrefSuiteMaterialSurfaceAsync(MaterialDTO materialDTO)
        {

            try

            {
                if (materialDTO.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(materialDTO.SourceReference ?? string.Empty);
                    materialDTO.Weight = weight;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPreSuiteMaterialSurface]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", materialDTO.ReferenceBase); //required   
                _ = cmd.Parameters.AddWithValue("@Weight", materialDTO.Weight); //required
                _ = cmd.Parameters.AddWithValue("@MaterialType", materialDTO.MaterialType); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("($Class}.{$Method}. Surface material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                        nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialSurfaceAsync),
                materialDTO.Reference,
                materialDTO.Color,
                materialDTO.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("($Class}.{$Method}. Surface material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                        nameof(SQLRepository),
                        nameof(InsertPrefSuiteMaterialSurfaceAsync),
                        materialDTO.Reference,
                        materialDTO.Color,
                        materialDTO.Description ?? "");

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
                materialDTO.Order ?? string.Empty,
                materialDTO.Worksheet ?? string.Empty,
                materialDTO.Line,
                materialDTO.ReferenceBase ?? string.Empty,
                materialDTO.Reference ?? string.Empty,
                materialDTO.Color ?? string.Empty,
                materialDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = materialDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialSurfaceAsync)}. Unhandled error." +
                   $"\nOrder {materialDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {materialDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {materialDTO.Line}," +
                   $"\nReferenceBase {materialDTO.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {materialDTO.Reference ?? string.Empty}," +
                   $"\nColor {materialDTO.Color ?? string.Empty}, " +
                   $"\nDescription {materialDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }

        public async Task<A2PError?> InsertPrefSuiteMaterialPurchaseDataAsync(MaterialDTO materialDTO)
        {



            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialPurchaseData]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Reference", materialDTO.Reference); //required   
                _ = cmd.Parameters.AddWithValue("@Package", materialDTO.PackageQuantity); //required
                _ = cmd.Parameters.AddWithValue("@Price", materialDTO.Price); //required
                _ = cmd.Parameters.AddWithValue("@Description", materialDTO.Description); //required
                _ = cmd.Parameters.AddWithValue("@Color", materialDTO.Color); //required
                _ = cmd.Parameters.AddWithValue("@SourceReference", materialDTO.SourceReference); //required
                _ = cmd.Parameters.AddWithValue("@SourceColor", materialDTO.SourceColor); //required

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("($Class}.{$Method}. Purchase data material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                        nameof(SQLRepository),
                nameof(InsertPrefSuiteMaterialPurchaseDataAsync),
                materialDTO.Reference,
                materialDTO.Color,
                materialDTO.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("($Class}.{$Method}. Purchase data material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                        nameof(SQLRepository),
                        nameof(InsertPrefSuiteMaterialPurchaseDataAsync),
                        materialDTO.Reference,
                        materialDTO.Color,
                        materialDTO.Description ?? "");

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
                materialDTO.Order ?? string.Empty,
                materialDTO.Worksheet ?? string.Empty,
                materialDTO.Line,
                materialDTO.ReferenceBase ?? string.Empty,
                materialDTO.Reference ?? string.Empty,
                materialDTO.Color ?? string.Empty,
                materialDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = materialDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialPurchaseDataAsync)}. Unhandled error." +
                   $"\nOrder {materialDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {materialDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {materialDTO.Line}," +
                   $"\nReferenceBase {materialDTO.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {materialDTO.Reference ?? string.Empty}," +
                   $"\nColor {materialDTO.Color ?? string.Empty}, " +
                   $"\nDescription {materialDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }



        public async Task<A2PError?> UpdateBCMapping(MaterialDTO materialDTO)
        {

            try

            {


                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_UpdateBCMapping]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", materialDTO.ReferenceBase ?? (object)DBNull.Value); //required   
                _ = cmd.Parameters.AddWithValue("@Reference", materialDTO.Reference ?? (object)DBNull.Value); //required   
                _ = cmd.Parameters.AddWithValue("@SourceReference", materialDTO.SourceReference ?? (object)DBNull.Value); //required   
                _ = cmd.Parameters.AddWithValue("@SourceColor", materialDTO.SourceColor ?? (object)DBNull.Value); //required  
                _ = cmd.Parameters.AddWithValue("@SourceColor1", materialDTO.CustomField1 ?? (object)DBNull.Value); //required  
                _ = cmd.Parameters.AddWithValue("@SourceColor2", materialDTO.CustomField2 ?? (object)DBNull.Value); //required  

                //=====================================================================================================================
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("($Class}.{$Method}. BC Mapping  {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                        nameof(SQLRepository),
                nameof(UpdateBCMapping),
                materialDTO.Reference,
                materialDTO.Color,
                materialDTO.Description ?? "");
                }

                if (result == 0)
                {

                    _logService.Verbose("($Class}.{$Method}. BC Mapping {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                        nameof(SQLRepository),
                        nameof(UpdateBCMapping),
                        materialDTO.Reference,
                        materialDTO.Color,
                        materialDTO.Description ?? "");

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
                materialDTO.Order ?? string.Empty,
                materialDTO.Worksheet ?? string.Empty,
                materialDTO.Line,
                materialDTO.ReferenceBase ?? string.Empty,
                materialDTO.Reference ?? string.Empty,
                materialDTO.Color ?? string.Empty,
                materialDTO.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new A2PError()
                {
                    Order = materialDTO.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialSurfaceAsync)}. Unhandled error." +
                   $"\nOrder {materialDTO.Order ?? string.Empty}," +
                   $"\nWorksheet {materialDTO.Worksheet ?? string.Empty}," +
                   $"\nLine {materialDTO.Line}," +
                   $"\nReferenceBase {materialDTO.ReferenceBase ?? string.Empty}, " +
                   $"\nReference {materialDTO.Reference ?? string.Empty}," +
                   $"\nColor {materialDTO.Color ?? string.Empty}, " +
                   $"\nDescription {materialDTO.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }



        public async Task<A2PError?> InsertPrefSuiteMaterialNeedsMasterAsync(string order, int number, int version)
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
                return new A2PError()
                {
                    Order = order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(SQLRepository)}.{nameof(InsertPrefSuiteMaterialNeedsMasterAsync)}. Unhandled error." +
                    $"\nOrder {order ?? string.Empty}," +
                    $"\nSalesDocument {number}/{version}." +
                    $"\nException: {ex.Message ?? string.Empty}"

                };
            }

        }
        public async Task<A2PError?> InsertPrefSuiteMaterialNeedsAsync(string order, int number, int version)
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
                return new A2PError()
                {
                    Order = order ?? string.Empty,
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
