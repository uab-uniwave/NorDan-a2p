// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.Interfaces;
using a2p.Domain.Entities;
using a2p.Domain.Interfaces;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using System.Data;

namespace a2p.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ISQLService _sqlService;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(ISQLService sqlService, ILogger<OrderRepository> logger)
        {
            _sqlService = sqlService ?? throw new ArgumentNullException(nameof(sqlService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OrderEntity?> GetOrderAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _logger.LogWarning("{Class}.{Method}. Input id is empty Guid.", nameof(OrderRepository), nameof(GetOrderAsync));
                    return null;
                }

                const string sql = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_Order] WHERE [Id] = @Id";
                var parameters = new SqlParameter[]
                {
                    new("@Id", id)
                };

                var result = await _sqlService.ExecuteQueryAsync(sql, CommandType.Text, parameters);
                if (result == null || result.Rows.Count == 0)
                    return null;

                return MapDataRowToOrder(result.Rows[0]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Class}.{Method}. Exception: {Message}", nameof(OrderRepository), nameof(GetOrderAsync), ex.Message);
                return null;
            }
        }

        public async Task<OrderEntity?> GetOrderByNumberAsync(string orderNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderNumber))
                {
                    _logger.LogWarning("{Class}.{Method}. Input orderNumber is null or empty.", nameof(OrderRepository), nameof(GetOrderByNumberAsync));
                    return null;
                }

                const string sql = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_Order] WHERE [OrderNumber] = @OrderNumber";
                var parameters = new SqlParameter[]
                {
                    new("@OrderNumber", orderNumber)
                };

                var result = await _sqlService.ExecuteQueryAsync(sql, CommandType.Text, parameters);
                if (result == null || result.Rows.Count == 0)
                    return null;

                return MapDataRowToOrder(result.Rows[0]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Class}.{Method}. Exception: {Message}", nameof(OrderRepository), nameof(GetOrderByNumberAsync), ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<OrderEntity>?> GetOrdersAsync()
        {
            try
            {
                const string sql = "SELECT * FROM [dbo].[Uniwave_a2p_Order]";
                var result = await _sqlService.ExecuteQueryAsync(sql, CommandType.Text);
                if (result == null || result.Rows.Count == 0)
                    return Enumerable.Empty<OrderEntity>();

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
                _logger.LogError(ex, "{Class}.{Method}. Exception: {Message}", nameof(OrderRepository), nameof(GetOrdersAsync), ex.Message);
                return Enumerable.Empty<OrderEntity>();
            }
        }

        public async Task<OrderEntity> InsertOrderAsync(OrderEntity order)
        {
            try
            {
                const string sql = @"INSERT INTO [dbo].[Uniwave_a2p_Order]
                ([Id],[OrderNumber],[OrderDate],[CustomerTitle],[CustomerNumber],[ProjectNumber],[DeliveryAddress],[CorrectionAvailableUnitil],[ResponsibleManager],
                 [SalesDocumentNumber],[SalesDocumentVersion],[CreatedUTCDateTime],[ModifiedUTCDateTime])
                VALUES
                (@Id,@OrderNumber,@OrderDate,@CustomerTitle,@CustomerNumber,@ProjectNumber,@DeliveryAddress,@CorrectionAvailableUnitil,@ResponsibleManager,
                 @SalesDocumentNumber,@SalesDocumentVersion,@CreatedUTCDateTime,@ModifiedUTCDateTime)";

                var parameters = CreateOrderParameters(order);
                var res = await _sqlService.ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
                if (res < 0)
                {
                    _logger.LogWarning("{Class}.{Method}. Insert returned < 0 for OrderNumber '{OrderNumber}'.", nameof(OrderRepository), nameof(InsertOrderAsync), order.OrderNumber);
                }

                // return freshly inserted order
                var selectSql = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_Order] WHERE [Id] = @Id";
                var selectParams = new SqlParameter[] { new("@Id", order.Id) };
                var result = await _sqlService.ExecuteQueryAsync(selectSql, CommandType.Text, selectParams);
                if (result == null || result.Rows.Count == 0)
                    return order;

                return MapDataRowToOrder(result.Rows[0]) ?? order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Class}.{Method}. Exception: {Message}", nameof(OrderRepository), nameof(InsertOrderAsync), ex.Message);
                return order;
            }
        }

        public async Task<OrderEntity?> UpdateOrdrAsync(OrderEntity order)
        {
            try
            {
                const string sql = @"UPDATE [dbo].[Uniwave_a2p_Order] SET
                [OrderNumber]=@OrderNumber,
                [OrderDate]=@OrderDate,
                [CustomerTitle]=@CustomerTitle,
                [CustomerNumber]=@CustomerNumber,
                [ProjectNumber]=@ProjectNumber,
                [DeliveryAddress]=@DeliveryAddress,
                [CorrectionAvailableUnitil]=@CorrectionAvailableUnitil,
                [ResponsibleManager]=@ResponsibleManager,
                [SalesDocumentNumber]=@SalesDocumentNumber,
                [SalesDocumentVersion]=@SalesDocumentVersion,
                [ModifiedUTCDateTime]=@ModifiedUTCDateTime
                WHERE [Id]=@Id";

                var parameters = CreateOrderParameters(order);
                var res = await _sqlService.ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
                if (res < 0)
                {
                    _logger.LogWarning("{Class}.{Method}. Update returned < 0 for OrderNumber '{OrderNumber}'.", nameof(OrderRepository), nameof(UpdateOrdrAsync), order.OrderNumber);
                }

                var selectSql = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_Order] WHERE [Id] = @Id";
                var selectParams = new SqlParameter[] { new("@Id", order.Id) };
                var result = await _sqlService.ExecuteQueryAsync(selectSql, CommandType.Text, selectParams);
                if (result == null || result.Rows.Count == 0)
                    return null;

                return MapDataRowToOrder(result.Rows[0]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Class}.{Method}. Exception: {Message}", nameof(OrderRepository), nameof(UpdateOrdrAsync), ex.Message);
                return null;
            }
        }

        public async Task<Guid> DeleteOrderAsync(Guid id)
        {
            try
            {
                const string sql = "DELETE FROM [dbo].[Uniwave_a2p_Order] WHERE [Id] = @Id";
                var parameters = new SqlParameter[] { new("@Id", id) };
                var rows = await _sqlService.ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
                return rows > 0 ? id : Guid.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Class}.{Method}. Exception: {Message}", nameof(OrderRepository), nameof(DeleteOrderAsync), ex.Message);
                return Guid.Empty;
            }
        }

        private static OrderEntity? MapDataRowToOrder(DataRow row)
        {
            try
            {
                if (row == null)
                    return null;

                var entity = new OrderEntity
                {
                    Id = row.Field<Guid>("Id"),
                    OrderNumber = row.Field<string>("OrderNumber") ?? string.Empty,
                    OrderDate = row.Table.Columns.Contains("OrderDate") && row["OrderDate"] != DBNull.Value
                        ? DateOnly.FromDateTime(row.Field<DateTime>("OrderDate"))
                        : null,
                    CustomerTitle = row.Table.Columns.Contains("CustomerTitle") ? (row.Field<string>("CustomerTitle") ?? string.Empty) : string.Empty,
                    CustomerNumber = row.Table.Columns.Contains("CustomerNumber") ? (row.Field<string>("CustomerNumber") ?? string.Empty) : string.Empty,
                    ProjectNumber = row.Table.Columns.Contains("ProjectNumber") ? row.Field<string>("ProjectNumber") : null,
                    DeliveryAddress = row.Table.Columns.Contains("DeliveryAddress") ? (row.Field<string>("DeliveryAddress") ?? string.Empty) : string.Empty,
                    CorrectionAvailableUnitil = row.Table.Columns.Contains("CorrectionAvailableUnitil") && row["CorrectionAvailableUnitil"] != DBNull.Value
                        ? DateOnly.FromDateTime(row.Field<DateTime>("CorrectionAvailableUnitil"))
                        : null,
                    ResponsibleManager = row.Table.Columns.Contains("ResponsibleManager") ? (row.Field<string>("ResponsibleManager") ?? string.Empty) : string.Empty,
                    SalesDocumentNumber = row.Table.Columns.Contains("SalesDocumentNumber") ? row.Field<int>("SalesDocumentNumber") : -1,
                    SalesDocumentVersion = row.Table.Columns.Contains("SalesDocumentVersion") ? row.Field<int>("SalesDocumentVersion") : -1,
                    CreatedUTCDateTime = row.Table.Columns.Contains("CreatedUTCDateTime") ? row.Field<DateTime>("CreatedUTCDateTime") : DateTime.UtcNow,
                    ModifiedUTCDateTime = row.Table.Columns.Contains("ModifiedUTCDateTime") ? row.Field<DateTime?>("ModifiedUTCDateTime") : null
                };

                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error mapping DataRow to OrderEntity: {ex.Message}");
                return null;
            }
        }

        private static SqlParameter[] CreateOrderParameters(OrderEntity order)
        {
            return new SqlParameter[]
            {
                new("@Id", order.Id),
                new("@OrderNumber", order.OrderNumber ?? string.Empty),
                new("@OrderDate", order.OrderDate.HasValue ? order.OrderDate.Value.ToDateTime(TimeOnly.MinValue) : (object)DBNull.Value),
                new("@CustomerTitle", (object?)order.CustomerTitle ?? DBNull.Value),
                new("@CustomerNumber", (object?)order.CustomerNumber ?? DBNull.Value),
                new("@ProjectNumber", (object?)order.ProjectNumber ?? DBNull.Value),
                new("@DeliveryAddress", (object?)order.DeliveryAddress ?? DBNull.Value),
                new("@CorrectionAvailableUnitil", order.CorrectionAvailableUnitil.HasValue ? order.CorrectionAvailableUnitil.Value.ToDateTime(TimeOnly.MinValue) : (object)DBNull.Value),
                new("@ResponsibleManager", (object?)order.ResponsibleManager ?? DBNull.Value),
                new("@SalesDocumentNumber", order.SalesDocumentNumber),
                new("@SalesDocumentVersion", order.SalesDocumentVersion),
                new("@CreatedUTCDateTime", order.CreatedUTCDateTime),
                new("@ModifiedUTCDateTime", (object?)order.ModifiedUTCDateTime ?? DBNull.Value)
            };
        }
    }
}
