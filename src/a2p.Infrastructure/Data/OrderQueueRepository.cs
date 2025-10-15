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

        public async Task<Result<OrderQueueEntity>> GetOrderAsync(Guid id)
        {
            try
            {
                var sql = "SELECT * FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [Id] = @Id";
                using var connection = new SqlConnection(_connectionString);
                var order = await connection.QueryFirstOrDefaultAsync<OrderQueueEntity>(sql, new { Id = id });
                if (order == null)
                    return Result<OrderQueueEntity>.Failure("NotFound");
                order.ProjectNumber ??= string.Empty;
                return Result<OrderQueueEntity>.Success(order);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error getting order by id: {ex.Message}");
                return Result<OrderQueueEntity>.Failure(ex.Message);
            }
        }

        public async Task<Result<OrderQueueEntity>> GetOrderByNumberAsync(string orderNumber)
        {
            try
            {
                var sql = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [OrderNumber] = @OrderNumber";
                using var connection = new SqlConnection(_connectionString);
                var order = await connection.QueryFirstOrDefaultAsync<OrderQueueEntity>(sql, new { OrderNumber = orderNumber });
                if (order == null)
                    return Result<OrderQueueEntity>.Failure("NotFound");
                order.ProjectNumber ??= string.Empty;
                return Result<OrderQueueEntity>.Success(order);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error getting order by number: {ex.Message}");
                return Result<OrderQueueEntity>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<OrderQueueEntity>>> GetOrdersAsync()
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
                return Result<IEnumerable<OrderQueueEntity>>.Success(orders);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error getting all orders: {ex.Message}");
                return Result<IEnumerable<OrderQueueEntity>>.Failure(ex.Message);
            }
        }

        public async Task<Result<OrderQueueEntity>> InsertOrderAsync(OrderQueueEntity order)
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
                return Result<OrderQueueEntity>.Success(order);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error inserting order: {ex.Message}");
                return Result<OrderQueueEntity>.Failure(ex.Message);
            }
        }

        public async Task<Result<OrderQueueEntity>> UpdateOrderAsync(OrderQueueEntity order)
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
                    return Result<OrderQueueEntity>.Failure("Failed to map updated order data");
                updated.ProjectNumber ??= string.Empty;
                return Result<OrderQueueEntity>.Success(updated);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating order: {ex.Message}");
                return Result<OrderQueueEntity>.Failure(ex.Message);
            }
        }

        public async Task<Result<Guid>> DeleteOrderAsync(Guid id)
        {
            try
            {
                var sql = "DELETE FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [Id] = @Id";
                using var connection = new SqlConnection(_connectionString);
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                if (rowsAffected > 0)
                    return Result<Guid>.Success(id);
                return Result<Guid>.Failure("NotFound");
            }
            catch (SqlException sqlEx)
            {
                _logService.Error(sqlEx.Message);
                return Result<Guid>.Failure(sqlEx.Message);
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message);
                return Result<Guid>.Failure(ex.Message);
            }
        }
    }
}