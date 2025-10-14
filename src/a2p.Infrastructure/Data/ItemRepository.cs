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
        private readonly ISQLService _sqlService;

        public ItemRepository(ISQLService sqlService, ILogService logService)
        {
            _sqlService = sqlService ?? throw new ArgumentNullException(nameof(sqlService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public async Task<ItemEntity> InsertItemAsync(ItemEntity item)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "INSERT INTO [dbo].[Uniwave_a2p_Items] " +
                    "(" +
                    "[RowId]," +
                    "[OrderNumber]," +
                    "[Worksheet]," +
                    "[Line]," +
                    "[Column]," +
                    "[Item]," +
                    "[SortOrder]," +
                    "[Description]," +
                    "[Quantity]," +
                    "[Width]," +
                    "[Height]," +
                    "[Weight]," +
                    "[WeightWithoutGlass]," +
                    "[WeightGlass]," +
                    "[TotalWeight]," +
                    "[TotalWeightWithoutGlass]," +
                    "[TotalWeightGlass]," +
                    "[Area]," +
                    "[TotalArea]," +
                    "[Hours]," +
                    "[TotalHours]," +
                    "[MaterialCost]," +
                    "[LaborCost]," +
                    "[Cost]," +
                    "[TotalMaterialCost]," +
                    "[TotalLaborCost]," +
                    "[TotalCost]," +
                    "[Price]," +
                    "[TotalPrice]," +
                    "[CurrencyCode]," +
                    "[ExchangeRateEUR]," +
                    "[MaterialCostEUR]," +
                    "[LaborCostEUR]," +
                    "[CostEUR]," +
                    "[TotalMaterialCostEUR]," +
                    "[TotalLaborCostEUR]," +
                    "[TotalCostEUR]," +
                    "[PriceEUR]," +
                    "[TotalPriceEUR]," +
                    "[WorksheetType]," +
                    "[CreatedUTCDateTime]," +
                    "[ModifiedUTCDateTime]" +
                    ") " +
                    "VALUES " +
                    "(" +
                    "@RowId," +
                    "@OrderNumber," +
                    "@Worksheet," +
                    "@Line," +
                    "@Column," +
                    "@Item," +
                    "@SortOrder," +
                    "@Description," +
                    "@Quantity," +
                    "@Width," +
                    "@Height," +
                    "@Weight," +
                    "@WeightWithoutGlass," +
                    "@WeightGlass," +
                    "@TotalWeight," +
                    "@TotalWeightWithoutGlass," +
                    "@TotalWeightGlass," +
                    "@Area," +
                    "@TotalArea," +
                    "@Hours," +
                    "@TotalHours," +
                    "@MaterialCost," +
                    "@LaborCost," +
                    "@Cost," +
                    "@TotalMaterialCost," +
                    "@TotalLaborCost," +
                    "@TotalCost," +
                    "@Price," +
                    "@TotalPrice," +
                    "@CurrencyCode," +
                    "@ExchangeRateEUR," +
                    "@MaterialCostEUR," +
                    "@LaborCostEUR," +
                    "@CostEUR," +
                    "@TotalMaterialCostEUR," +
                    "@TotalLaborCostEUR," +
                    "@TotalCostEUR," +
                    "@PriceEUR," +
                    "@TotalPriceEUR," +
                    "@WorksheetType," +
                    "@CreatedUTCDateTime," +
                    "@ModifiedUTCDateTime" +
                    ")",
                    CommandType = CommandType.Text
                };

                // Create parameters for the insert
                var parameters = CreateItemParameters(item);
                await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);

                // Query to get the inserted item
                cmd.CommandText = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_Items] WHERE [RowId] = @RowId";
                var queryParams = new SqlParameter[]
                {
                    new SqlParameter("@RowId", item.RowId)
                };

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, queryParams);
                if (result == null || result.Rows.Count == 0)
                    throw new InvalidOperationException($"Item with ID {item.RowId} was not found after insert.");

                return MapDataRowToItem(result.Rows[0]);
            }
            catch (SqlException sqlEx)
            {
                _logService.Error("SQL Error Inserting Item {0} in Order {1}. {2}", item.ItemName, item.OrderNumber, sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logService.Error("Error Inserting Item {0} in Order {1}. {2}", item.ItemName, item.OrderNumber, ex.Message);
                throw;
            }
        }

        public async Task<ItemEntity?> UpdateItemAsync(ItemEntity item)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "UPDATE [dbo].[Uniwave_a2p_Items] " +
                    "SET " +
                    "[OrderNumber] = @OrderNumber, " +
                    "[Worksheet] = @Worksheet, " +
                    "[Line] = @Line, " +
                    "[Column] = @Column, " +
                    "[Item] = @Item, " +
                    "[SortOrder] = @SortOrder, " +
                    "[Description] = @Description, " +
                    "[Quantity] = @Quantity, " +
                    "[Width] = @Width, " +
                    "[Height] = @Height, " +
                    "[Weight] = @Weight, " +
                    "[WeightWithoutGlass] = @WeightWithoutGlass, " +
                    "[WeightGlass] = @WeightGlass, " +
                    "[TotalWeight] = @TotalWeight, " +
                    "[TotalWeightWithoutGlass] = @TotalWeightWithoutGlass, " +
                    "[TotalWeightGlass] = @TotalWeightGlass, " +
                    "[Area] = @Area, " +
                    "[TotalArea] = @TotalArea, " +
                    "[Hours] = @Hours, " +
                    "[TotalHours] = @TotalHours, " +
                    "[MaterialCost] = @MaterialCost, " +
                    "[LaborCost] = @LaborCost, " +
                    "[Cost] = @Cost, " +
                    "[TotalMaterialCost] = @TotalMaterialCost, " +
                    "[TotalLaborCost] = @TotalLaborCost, " +
                    "[TotalCost] = @TotalCost, " +
                    "[Price] = @Price, " +
                    "[TotalPrice] = @TotalPrice, " +
                    "[CurrencyCode] = @CurrencyCode, " +
                    "[ExchangeRateEUR] = @ExchangeRateEUR, " +
                    "[MaterialCostEUR] = @MaterialCostEUR, " +
                    "[LaborCostEUR] = @LaborCostEUR, " +
                    "[CostEUR] = @CostEUR, " +
                    "[TotalMaterialCostEUR] = @TotalMaterialCostEUR, " +
                    "[TotalLaborCostEUR] = @TotalLaborCostEUR, " +
                    "[TotalCostEUR] = @TotalCostEUR, " +
                    "[PriceEUR] = @PriceEUR, " +
                    "[TotalPriceEUR] = @TotalPriceEUR, " +
                    "[WorksheetType] = @WorksheetType, " +
                    "[ModifiedUTCDateTime] = @ModifiedUTCDateTime " +
                    "WHERE [RowId] = @RowId",
                    CommandType = CommandType.Text
                };

                // Create parameters for the update
                var parameters = CreateItemParameters(item);
                await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);

                // Query to get the updated item
                cmd.CommandText = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_Items] WHERE [RowId] = @RowId";
                var queryParams = new SqlParameter[]
                {
                    new SqlParameter("@RowId", item.RowId)
                };

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, queryParams);
                if (result == null || result.Rows.Count == 0)
                    return null;

                return MapDataRowToItem(result.Rows[0]);
            }
            catch (SqlException sqlEx)
            {
                _logService.Error("SQL Error Updating Item {0} in Order {1}. {2}", item.ItemName, item.OrderNumber, sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logService.Error("Error Updating Item {0} in Order {1}. {2}", item.ItemName, item.OrderNumber, ex.Message);
                return null;
            }
        }

        public async Task<ItemEntity?> GetItemAsync(Guid rowId)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_Items] WHERE [RowId] = @RowId",
                    CommandType = CommandType.Text
                };

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@RowId", rowId)
                };

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);

                if (result == null || result.Rows.Count == 0)
                    return null;

                return MapDataRowToItem(result.Rows[0]);
            }
            catch (SqlException sqlEx)
            {
                _logService.Error("SQL Error Getting Item {0}. {1}", rowId, sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logService.Error("Error Getting Item {0}. {1}", rowId, ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<ItemEntity>?> GetOrderItemsAsync(Guid rowId)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT * FROM [dbo].[Uniwave_a2p_Items] WHERE [OrderId] = @OrderId",
                    CommandType = CommandType.Text
                };

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@OrderId", rowId)
                };

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);

                if (result == null || result.Rows.Count == 0)
                    return Array.Empty<ItemEntity>();

                List<ItemEntity> items = new();
                foreach (DataRow row in result.Rows)
                {
                    var item = MapDataRowToItem(row);
                    if (item != null)
                        items.Add(item);
                }

                return items;
            }
            catch (SqlException sqlEx)
            {
                _logService.Error("SQL Error Getting Order Items for Order {0}. {1}", rowId, sqlEx.Message);
                return Array.Empty<ItemEntity>();
            }
            catch (Exception ex)
            {
                _logService.Error("Error Getting Order Items for Order {0}. {1}", rowId, ex.Message);
                return Array.Empty<ItemEntity>();
            }
        }

        public async Task<IEnumerable<ItemEntity>?> GetItemsAsync()
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT * FROM [dbo].[Uniwave_a2p_Items]",
                    CommandType = CommandType.Text
                };
                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType);

                if (result == null || result.Rows.Count == 0)
                    return Array.Empty<ItemEntity>();

                List<ItemEntity> items = new();
                foreach (DataRow row in result.Rows)
                {
                    var item = MapDataRowToItem(row);
                    if (item != null)
                        items.Add(item);
                }
                return items;
            }
            catch (SqlException sqlEx)
            {
                _logService.Error("SQL Error Getting All Items. {0}", sqlEx.Message);
                return Array.Empty<ItemEntity>();
            }
            catch (Exception ex)
            {
                _logService.Error("Error Getting All Items. {0}", ex.Message);
                return Array.Empty<ItemEntity>();
            }
        }

        public async Task<Guid> DeleteItemAsync(Guid rowId)
        {

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "DELETE FROM [dbo].[Uniwave_a2p_Items] WHERE [RowId] = @RowId",
                    CommandType = CommandType.Text
                };
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@RowId", rowId)
                };
                int rowsAffected = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, parameters);
                if (rowsAffected > 0)
                    return rowId;
                return Guid.Empty;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
                return Guid.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Guid.Empty;
            }


        }
        private static ItemEntity MapDataRowToItem(DataRow row)
        {
            try
            {
                if (row == null)
                    return null;

                return new ItemEntity
                {
                    // Base entity properties
                    RowId = row.Field<Guid>("RowId"),

                    // Order related properties
                    OrderNumber = row.Field<string>("OrderNumber") ?? string.Empty,
                    Worksheet = row.Field<string>("Worksheet") ?? string.Empty,
                    Line = row.Field<int>("Line"),
                    Column = row.Field<int>("Column"),

                    // Item identification
                    ItemName = row.Field<string>("Item") ?? string.Empty,
                    SortOrder = row.Field<int>("SortOrder"),
                    Description = row.Field<string>("Description"),

                    // Quantity
                    Quantity = row.Field<int>("Quantity"),

                    // Dimensions
                    Width = row.Field<decimal>("Width"),
                    Height = row.Field<decimal>("Height"),

                    // Weight information
                    Weight = row.Field<decimal>("Weight"),
                    WeightWithoutGlass = row.Field<decimal>("WeightWithoutGlass"),
                    WeightGlass = row.Field<decimal>("WeightGlass"),
                    TotalWeight = row.Field<decimal>("TotalWeight"),
                    TotalWeightWithoutGlass = row.Field<decimal>("TotalWeightWithoutGlass"),
                    TotalWeightGlass = row.Field<decimal>("TotalWeightGlass"),

                    // Area information
                    Area = row.Field<decimal>("Area"),
                    TotalArea = row.Field<decimal>("TotalArea"),

                    // Hours information
                    Hours = row.Field<decimal>("Hours"),
                    TotalHours = row.Field<decimal>("TotalHours"),

                    // Cost information
                    MaterialCost = row.Field<decimal>("MaterialCost"),
                    LaborCost = row.Field<decimal>("LaborCost"),
                    Cost = row.Field<decimal>("Cost"),
                    TotalMaterialCost = row.Field<decimal>("TotalMaterialCost"),
                    TotalLaborCost = row.Field<decimal>("TotalLaborCost"),
                    TotalCost = row.Field<decimal>("TotalCost"),

                    // Price information
                    Price = row.Field<decimal>("Price"),
                    TotalPrice = row.Field<decimal>("TotalPrice"),

                    // Currency information
                    CurrencyCode = row.Field<string>("CurrencyCode"),
                    ExchangeRateEUR = row.Field<decimal>("ExchangeRateEUR"),

                    // EUR Cost information
                    MaterialCostEUR = row.Field<decimal>("MaterialCostEUR"),
                    LaborCostEUR = row.Field<decimal>("LaborCostEUR"),
                    CostEUR = row.Field<decimal>("CostEUR"),
                    TotalMaterialCostEUR = row.Field<decimal>("TotalMaterialCostEUR"),
                    TotalLaborCostEUR = row.Field<decimal>("TotalLaborCostEUR"),
                    TotalCostEUR = row.Field<decimal>("TotalCostEUR"),

                    // EUR Price information
                    PriceEUR = row.Field<decimal>("PriceEUR"),
                    TotalPriceEUR = row.Field<decimal>("TotalPriceEUR"),

                    // Type information
                    WorksheetType = (WorksheetType)row.Field<int>("WorksheetType"),

                    // Audit fields from base entity
                    CreatedUTCDateTime = row.Field<DateTime>("CreatedUTCDateTime"),
                    ModifiedUTCDateTime = row.Field<DateTime>("ModifiedUTCDateTime")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error mapping DataRow to ItemEntity: {ex.Message}");
                return null;
            }
        }

        private static SqlParameter[] CreateItemParameters(ItemEntity item)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@RowId", item.RowId),
                new SqlParameter("@OrderNumber", item.OrderNumber ?? string.Empty),
                new SqlParameter("@Worksheet", item.Worksheet ?? string.Empty),
                new SqlParameter("@Line", item.Line),
                new SqlParameter("@Column", item.Column),
                new SqlParameter("@Item", item.ItemName ?? (object)DBNull.Value),
                new SqlParameter("@SortOrder", item.SortOrder),
                new SqlParameter("@Description", item.Description ?? (object)DBNull.Value),
                new SqlParameter("@Quantity", item.Quantity),
                new SqlParameter("@Width", item.Width),
                new SqlParameter("@Height", item.Height),
                new SqlParameter("@Weight", item.Weight),
                new SqlParameter("@WeightWithoutGlass", item.WeightWithoutGlass),
                new SqlParameter("@WeightGlass", item.WeightGlass),
                new SqlParameter("@TotalWeight", item.TotalWeight),
                new SqlParameter("@TotalWeightWithoutGlass", item.TotalWeightWithoutGlass),
                new SqlParameter("@TotalWeightGlass", item.TotalWeightGlass),
                new SqlParameter("@Area", item.Area),
                new SqlParameter("@TotalArea", item.TotalArea),
                new SqlParameter("@Hours", item.Hours),
                new SqlParameter("@TotalHours", item.TotalHours),
                new SqlParameter("@MaterialCost", item.MaterialCost),
                new SqlParameter("@LaborCost", item.LaborCost),
                new SqlParameter("@Cost", item.Cost),
                new SqlParameter("@TotalMaterialCost", item.TotalMaterialCost),
                new SqlParameter("@TotalLaborCost", item.TotalLaborCost),
                new SqlParameter("@TotalCost", item.TotalCost),
                new SqlParameter("@Price", item.Price),
                new SqlParameter("@TotalPrice", item.TotalPrice),
                new SqlParameter("@CurrencyCode", item.CurrencyCode ?? (object)DBNull.Value),
                new SqlParameter("@ExchangeRateEUR", item.ExchangeRateEUR),
                new SqlParameter("@MaterialCostEUR", item.MaterialCostEUR),
                new SqlParameter("@LaborCostEUR", item.LaborCostEUR),
                new SqlParameter("@CostEUR", item.CostEUR),
                new SqlParameter("@TotalMaterialCostEUR", item.TotalMaterialCostEUR),
                new SqlParameter("@TotalLaborCostEUR", item.TotalLaborCostEUR),
                new SqlParameter("@TotalCostEUR", item.TotalCostEUR),
                new SqlParameter("@PriceEUR", item.PriceEUR),
                new SqlParameter("@TotalPriceEUR", item.TotalPriceEUR),
                new SqlParameter("@WorksheetType", (int)item.WorksheetType),
                new SqlParameter("@CreatedUTCDateTime", item.CreatedUTCDateTime),
                new SqlParameter("@ModifiedUTCDateTime", item.ModifiedUTCDateTime ?? DateTime.UtcNow)
            };
        }
    }
}
