// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.Services;
using a2p.Domain.Entities;
using a2p.Domain.Enums;
using a2p.Domain.Interfaces;

using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ILogService _logService;
        private readonly ISQLService _sqlService;

        public OrderRepository(ISQLService sqlService, ILogService logService)
        {
            _sqlService = sqlService ?? throw new ArgumentNullException(nameof(sqlService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
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
                _ = cmd.Parameters.AddWithValue("@Order", material.OrderNumber); //required

                _ = cmd.Parameters.AddWithValue("@Worksheet", material.Worksheet); //required
                _ = cmd.Parameters.AddWithValue("@Line", material.Line); //required
                _ = cmd.Parameters.AddWithValue("@Column", material.Column); //required 
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Item", material.ItemName ?? (object)DBNull.Value);
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

                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                _logService.Verbose("{$Class}.{$Method}. Order: {$Order}, worksheet {$Worksheet}, line {$Line}, reference {$Reference}, color {$Color}, successfully inserted into DB.",
                 nameof(_sqlService),
                      nameof(InsertOrderMaterialAsync),
                      material.OrderNumber,
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
                nameof(OrderRepository),
                nameof(InsertOrderMaterialAsync),
                material.OrderNumber,
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
                    OrderNumber = material.OrderNumber ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(OrderRepository)}.{nameof(InsertOrderMaterialAsync)}. Unhandled error." +
                   $"\nOrder {material.OrderNumber ?? string.Empty}," +
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
                _ = cmd.Parameters.AddWithValue("@Order", item.OrderNumber); //required
                _ = cmd.Parameters.AddWithValue("@Order", item.Worksheet); //required
                _ = cmd.Parameters.AddWithValue("@Line", item.Line); //required
                _ = cmd.Parameters.AddWithValue("@Column", item.Column); //required
                //=====================================================================================================================
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

                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
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
                nameof(OrderRepository),
                nameof(InsertOrderItemAsync),
                item.OrderNumber ?? string.Empty,
                item.Worksheet ?? string.Empty,
                item.Line,
                item.ItemName ?? string.Empty,
                item.Description ?? string.Empty,
                ex.Message ?? string.Empty
               );
                return new ErrorEntity()
                {
                    OrderNumber = item.OrderNumber ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(OrderRepository)}.{nameof(InsertOrderItemAsync)}. Unhandled error." +
                   $"\nOrder {item.OrderNumber ?? string.Empty}," +
                   $"\nWorksheet {item.Worksheet ?? string.Empty}," +
                   $"\nLine {item.Line}," +
                   $"\nReferenceBase {item.ItemName ?? string.Empty}, " +
                   $"\nReference {item.Description ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"
                };
            }

        }


    }
}
