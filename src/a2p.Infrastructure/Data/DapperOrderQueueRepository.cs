using a2p.Application.Interfaces;
using a2p.Domain.Entities;
using a2p.Domain.Interfaces;
using a2p.Domain.Models;
using a2p.Infrastructure.Services;

using Dapper;

using Microsoft.Data.SqlClient;

namespace a2p.Infrastructure.Data
{
    public class DapperOrderQueueRepository : IOrderQueueRepository
    {
        private readonly string _connectionString;
        private readonly ILogService _logService;
        private readonly DapperService _dapperService;

        public DapperOrderQueueRepository(string connectionString, ILogService logService, DapperService dapperService)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _dapperService = dapperService ?? throw new ArgumentNullException(nameof(dapperService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public async Task<Result<OrderQueueEntity>> GetOrderAsync(Guid id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT * FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [Id] = @Id";
                var order = await connection.QuerySingleOrDefaultAsync<OrderQueueEntity>(sql, new { Id = id });
                if (order == null)
                    return Result<OrderQueueEntity>.Failure("NotFound");
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
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [OrderNumber] = @OrderNumber";
                var order = await connection.QuerySingleOrDefaultAsync<OrderQueueEntity>(sql, new { OrderNumber = orderNumber });
                if (order == null)
                    return Result<OrderQueueEntity>.Failure("NotFound");
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
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT * FROM [dbo].[Uniwave_a2p_OrderQueue]";
                var orders = await connection.QueryAsync<OrderQueueEntity>(sql);
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
                using var connection = new SqlConnection(_connectionString);
                var sql = @"INSERT INTO [dbo].[Uniwave_a2p_OrderQueue] ([Id], [OrderNumber], [OrderId], [Number], [Version], [PayloadJson], [State], [CreatedUTCDateTime], [ModifiedUTCDateTime])
                            VALUES (@Id, @OrderNumber, @OrderId, @Number, @Version, @PayloadJson, @State, @CreatedUTCDateTime, @ModifiedUTCDateTime)";
                await connection.ExecuteAsync(sql, order);
                return Result<OrderQueueEntity>.Success(order);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error inserting order: {ex.Message}");
                return Result<OrderQueueEntity>.Failure(ex.Message);
            }
        }

        public async Task<Result<OrderQueueEntity>> UpdateOrdrAsync(OrderQueueEntity order)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"UPDATE [dbo].[Uniwave_a2p_OrderQueue] SET [OrderNumber] = @OrderNumber, [OrderId] = @OrderId, [Number] = @Number, [Version] = @Version, [PayloadJson] = @PayloadJson, [State] = @State, [ModifiedUTCDateTime] = @ModifiedUTCDateTime WHERE [Id] = @Id";
                var affected = await connection.ExecuteAsync(sql, order);
                if (affected == 0)
                    return Result<OrderQueueEntity>.Failure("NotFound");
                return Result<OrderQueueEntity>.Success(order);
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
                using var connection = new SqlConnection(_connectionString);
                var sql = "DELETE FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [Id] = @Id";
                var affected = await connection.ExecuteAsync(sql, new { Id = id });
                if (affected == 0)
                    return Result<Guid>.Failure("NotFound");
                return Result<Guid>.Success(id);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error deleting order: {ex.Message}");
                return Result<Guid>.Failure(ex.Message);
            }
        }
    }
}