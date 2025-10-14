// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.Interfaces;
using a2p.Domain.Entities;
using a2p.Domain.Enums;
using a2p.Domain.Interfaces;

using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Infrastructure.Data
{
    public class ItemRepository : IItemRepository
    {
        private readonly ILogService _logService;
        private readonly ISQLService _sqlRepository;

        public SQLRepository(ISQLService sqlRepository, ILogService logService)
        {
            _sqlRepository = sqlRepository ?? throw new ArgumentNullException(nameof(sqlRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }


        public async Task<ErrorEntity?> InsertOrderItemAsync(ItemEntity item, int number, int version, Guid idPos)
        {


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
                _ = cmd.Parameters.AddWithValue("@OrderNumber", item.OrderNumber); //required
                _ = cmd.Parameters.AddWithValue("@OrderNumber", item.Worksheet); //required
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



    }
    }
