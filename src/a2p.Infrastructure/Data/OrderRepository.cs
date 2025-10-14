// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.Interfaces;
using a2p.Domain.Entities;
using a2p.Domain.Interfaces;

using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ISQLService _sqlService;
        private readonly ILogService _logService;

        public OrderRepository(ISQLService sqlService, ILogService logService)
        {
            _sqlService = sqlService ?? throw new ArgumentNullException(nameof(sqlService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public async Task<OrderEntity?> GetOrderAsync(Guid id)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT * FROM [dbo].[Uniwave_a2p_Orders] WHERE [RowId] = @RowId",
                    CommandType = CommandType.Text
                };

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@RowId", id)
                };

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);

                if (result.Rows.Count == 0)
                    return null;

                return MapDataRowToOrder(result.Rows[0]);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error getting order by id: {ex.Message}");
                return null;
            }
        }

        public async Task<OrderEntity?> GetOrderByNumberAsync(string orderNumber)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_Orders] WHERE [OrderNumber] = @OrderNumber",
                    CommandType = CommandType.Text
                };

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@OrderNumber", orderNumber)
                };

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);

                if (result.Rows.Count == 0)
                    return null;

                return MapDataRowToOrder(result.Rows[0]);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error getting order by number: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<OrderEntity>?> GetOrdersAsync()
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT * FROM [dbo].[Uniwave_a2p_Orders]",
                    CommandType = CommandType.Text
                };

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType);

                List<OrderEntity> orders = new();
                foreach (DataRow row in result.Rows)
                {
                    var order = MapDataRowToOrder(row);
                    if (order != null)
                        orders.Add(order);
                }

                return orders;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error getting all orders: {ex.Message}");
                return Array.Empty<OrderEntity>();
            }
        }

        public async Task<OrderEntity> InsertOrderAsync(OrderEntity order)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "INSERT INTO [dbo].[Uniwave_a2p_Orders] " +
                    "([RowId], [OrderNumber], [Project], [SalesDocumentNumber], [SalesDocumentVersion], " +
                    "[SourceAppType], [SalesDocumentState], [Currency], [ExchangeRate], [Import], [DeleteExisting], " +
                    "[CreatedUTCDateTime], [ModifiedUTCDateTime]) VALUES " +
                    "(@RowId, @OrderNumber, @Project, @SalesDocumentNumber, @SalesDocumentVersion, " +
                    "@SourceAppType, @SalesDocumentState, @Currency, @ExchangeRate, @Import, @DeleteExisting, " +
                    "@CreatedUTCDateTime, @ModifiedUTCDateTime)",
                    CommandType = CommandType.Text
                };

                var parameters = CreateOrderParameters(order);
                await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);
                return order;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error inserting order: {ex.Message}");
                throw;
            }
        }

        public async Task<OrderEntity?> UpdateOrdrAsync(OrderEntity order)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "UPDATE [dbo].[Uniwave_a2p_Orders] SET " +
                    "[OrderNumber] = @OrderNumber, [Project] = @Project, " +
                    "[SalesDocumentNumber] = @SalesDocumentNumber, [SalesDocumentVersion] = @SalesDocumentVersion, " +
                    "[SourceAppType] = @SourceAppType, [SalesDocumentState] = @SalesDocumentState, " +
                    "[Currency] = @Currency, [ExchangeRate] = @ExchangeRate, " +
                    "[Import] = @Import, [DeleteExisting] = @DeleteExisting, " +
                    "[ModifiedUTCDateTime] = @ModifiedUTCDateTime " +
                    "WHERE [RowId] = @RowId",
                    CommandType = CommandType.Text
                };

                var parameters = CreateOrderParameters(order);
                await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);
                cmd.CommandText = "SELECT * FROM [Uniwave_a2p_Orders] " +
                    "WHERE [RowId] = @RowId";

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);
                if (result == null)
                    return null;

                if (result.Rows.Count == 0)
                    return null;
                return MapDataRowToOrder(result.Rows[0]);

            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating order: {ex.Message}");
                throw;
            }
        }

        public async Task<Guid> DeleteOrderAsync(Guid rowId)
        {

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "DELETE FROM [dbo].[Uniwave_a2p_Order] WHERE [RowId] = @RowId",
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

        private static SqlParameter[] CreateOrderParameters(OrderEntity order)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@RowId", order.RowId),
                new SqlParameter("@OrderNumber", order.OrderNumber),
                new SqlParameter("@Project", order.Project ?? (object)DBNull.Value),
                new SqlParameter("@SalesDocumentNumber", order.SalesDocumentNumber),
                new SqlParameter("@SalesDocumentVersion", order.SalesDocumentVersion),
                new SqlParameter("@SourceAppType", (int)order.SourceAppType),
                new SqlParameter("@Currency", order.Currency ?? (object)DBNull.Value),
                new SqlParameter("@ExchangeRate", order.ExchangeRate),
                new SqlParameter("@CreatedUTCDateTime", order.CreatedUTCDateTime),
                new SqlParameter("@ModifiedUTCDateTime", order.ModifiedUTCDateTime)
            };
        }

        private static OrderEntity? MapDataRowToOrder(DataRow row)
        {
            try
            {
                return new OrderEntity
                {
                    RowId = (Guid)row["RowId"],
                    OrderNumber = (string)row["OrderNumber"],
                    Project = row["Project"] as string,
                    SalesDocumentNumber = (int)row["SalesDocumentNumber"],
                    SalesDocumentVersion = (int)row["SalesDocumentVersion"],
                    Currency = row["Currency"] as string,
                    ExchangeRate = (double)row["ExchangeRate"],
                    CreatedUTCDateTime = (DateTime)row["CreatedUTCDateTime"],
                    ModifiedUTCDateTime = (DateTime)row["ModifiedUTCDateTime"]
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error mapping data row to OrderEntity: {ex.Message}");
                return null;
            }
        }
    }
}
