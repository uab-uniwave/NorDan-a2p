// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.Interfaces;
using a2p.Domain.Entities;
using a2p.Domain.Interfaces;

using Dapper;

using Microsoft.Data.SqlClient;

namespace a2p.Infrastructure.Data
{
    public class OrderQueueRepository : IOrderQueueRepository
    {
        private readonly string _connectionString;
        private readonly ILogService _logService;

        public OrderQueueRepository(string connectionString, ILogService logService)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public async Task<OrderQueueEntity?> GetOrderAsync(Guid id)
        {
            try
            {
                var sql = "SELECT * FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [Id] = @Id";
                using var connection = new SqlConnection(_connectionString);
                var order = await connection.QueryFirstOrDefaultAsync<OrderQueueEntity>(sql, new { Id = id });
                if (order == null)
                    return null;
                order.ProjectNumber ??= string.Empty;
                return order;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error getting order by id: {ex.Message}");
                return null;
            }
        }

        public async Task<OrderQueueEntity?> GetOrderByNumberAsync(string orderNumber)
        {
            try
            {
                var sql = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [OrderNumber] = @OrderNumber";
                using var connection = new SqlConnection(_connectionString);
                var order = await connection.QueryFirstOrDefaultAsync<OrderQueueEntity>(sql, new { OrderNumber = orderNumber });
                if (order == null)
                    return null;
                order.ProjectNumber ??= string.Empty;
                return order;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error getting order by number: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<OrderQueueEntity>?> GetOrdersAsync()
        {
            try
            {
                var sql = "SELECT * FROM [dbo].[Uniwave_a2p_OrderQueue]";
                using var connection = new SqlConnection(_connectionString);
                var orders = await connection.QueryAsync<OrderQueueEntity>(sql);
                foreach (var o in orders)
                {
                    o.ProjectNumber ??= string.Empty;
                }
                return orders;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error getting all orders: {ex.Message}");
                return Enumerable.Empty<OrderQueueEntity>();
            }
        }

        public async Task<OrderQueueEntity> InsertOrderAsync(OrderQueueEntity order)
        {
            try
            {
                order.CreatedUTCDateTime = DateTime.UtcNow;
                var sql = @"INSERT INTO [dbo].[Uniwave_a2p_OrderQueue] 
                ([Id], [OrderNumber], [OrderId], [ProjectNumber], [SalesDocumentNumber], [SalesDocumentVersion], [PayloadJson], [State], 
                [CreatedUTCDateTime], [ModifiedUTCDateTime]) VALUES 
                (@Id, @OrderNumber, @OrderId, @ProjectNumber, @SalesDocumentNumber, @SalesDocumentVersion, @PayloadJson, @State, 
                @CreatedUTCDateTime, @ModifiedUTCDateTime)";
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters(order);
                if (string.IsNullOrEmpty(order.ProjectNumber))
                    parameters.Add("@ProjectNumber", DBNull.Value);
                await connection.ExecuteAsync(sql, parameters);
                return order;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error inserting order: {ex.Message}");
                throw;
            }
        }

        public async Task<OrderQueueEntity?> UpdateOrderAsync(OrderQueueEntity order)
        {
            try
            {
                order.ModifiedUTCDateTime = DateTime.UtcNow;
                var sql = @"UPDATE [dbo].[Uniwave_a2p_OrderQueue] SET 
                [OrderNumber] = @OrderNumber, [OrderId] = @OrderId, [ProjectNumber] = @ProjectNumber, 
                [SalesDocumentNumber] = @SalesDocumentNumber, [SalesDocumentVersion] = @SalesDocumentVersion, 
                [PayloadJson] = @PayloadJson, [State] = @State, 
                [ModifiedUTCDateTime] = @ModifiedUTCDateTime 
                WHERE [Id] = @Id";
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters(order);
                if (string.IsNullOrEmpty(order.ProjectNumber))
                    parameters.Add("@ProjectNumber", DBNull.Value);
                await connection.ExecuteAsync(sql, parameters);
                var selectSql = "SELECT * FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [Id] = @Id";
                var updated = await connection.QueryFirstOrDefaultAsync<OrderQueueEntity>(selectSql, new { Id = order.Id });
                if (updated == null)
                    return null;
                updated.ProjectNumber ??= string.Empty;
                return updated;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating order: {ex.Message}");
                return null;
            }
        }

        public async Task<Guid> DeleteOrderAsync(Guid id)
        {
            try
            {
                var sql = "DELETE FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [Id] = @Id";
                using var connection = new SqlConnection(_connectionString);
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                if (rowsAffected > 0)
                    return id;
                return Guid.Empty;
            }
            catch (SqlException sqlEx)
            {
                _logService.Error(sqlEx.Message);
                return Guid.Empty;
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message);
                return Guid.Empty;
            }
        }
    }
}