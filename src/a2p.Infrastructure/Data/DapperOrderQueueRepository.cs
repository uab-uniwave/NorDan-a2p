using a2p.Domain.Entities;
using a2p.Domain.Interfaces;
using a2p.Infrastructure.Services;

using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace a2p.Infrastructure.Data
{
    public class DapperOrderQueueRepository : IOrderQueueRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<DapperOrderQueueRepository> _logger;
        private readonly DapperService _dapperService;

        public DapperOrderQueueRepository(string connectionString, ILogger<DapperOrderQueueRepository> logger, DapperService dapperService)
        {

            _connectionString = connectionString ?? string.Empty;
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
            }
            _dapperService = dapperService ?? throw new ArgumentNullException(nameof(dapperService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));



        }

        public async Task<OrderQueueEntity?> GetOrderAsync(Guid id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT * FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [Id] = @Id";
                var order = await connection.QuerySingleOrDefaultAsync<OrderQueueEntity>(sql, new { Id = id });
                if (order == null)
                {
                    return null;
                }
                return order;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting OrderQueue by ID: {ex.Message}");
                return null;

            }
        }

        public async Task<OrderQueueEntity?> GetOrderByNumberAsync(string orderNumber)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [OrderNumber] = @OrderNumber";
                var OrderQueue = await connection.QuerySingleOrDefaultAsync<OrderQueueEntity>(sql, new { OrderNumber = orderNumber });
                if (OrderQueue == null)
                {
                    return null;
                }

                return OrderQueue;

            }
            catch (Exception ex)
            {

                _logger.LogError($"Error getting OrderQueue by OrderNumber: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<OrderQueueEntity>?> GetOrdersAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT * FROM [dbo].[Uniwave_a2p_OrderQueue]";
                var resut = await connection.QueryAsync<OrderQueueEntity>(sql);
                if (resut == null)
                {
                    _logger.LogWarning("No orders found in the database.");
                    return null;
                }

                return resut;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all orders: {ex.Message}");
                return null;
            }
        }

        public async Task<OrderQueueEntity?> InsertOrderAsync(OrderQueueEntity order)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"INSERT INTO [dbo].[Uniwave_a2p_OrderQueue] ([Id], [OrderNumber], [OrderId], [Number], [Version], [PayloadJson], [State], [CreatedUTCDateTime], [ModifiedUTCDateTime])
                            VALUES (@Id, @OrderNumber, @OrderId, @Number, @Version, @PayloadJson, @State, @CreatedUTCDateTime, @ModifiedUTCDateTime)";
                var result = await connection.ExecuteAsync(sql, order);
                if (result == 0)
                {
                    _logger.LogError("Failed to insert OrderQueue into the database.");
                    return null;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error inserting OrderQueue: {ex.Message}");
                return null;

            }



            try
            {

                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT * FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [Id] = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<OrderQueueEntity>(sql, new { Id = order.Id });

                if (result == null)
                {
                    _logger.LogError("Inserted OrderQueue not found.");
                    return null;
                }
                return result;
            }


            catch (Exception ex)
            {
                _logger.LogError($"Error inserting OrderQueue: {ex.Message}");

                return null;
            }
        }

        public async Task<OrderQueueEntity?> UpdateOrderAsync(OrderQueueEntity order)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"UPDATE [dbo].[Uniwave_a2p_OrderQueue] SET [OrderNumber] = @OrderNumber, [OrderId] = @OrderId, [Number] = @Number, [Version] = @Version, [PayloadJson] = @PayloadJson, [State] = @State, [ModifiedUTCDateTime] = @ModifiedUTCDateTime WHERE [Id] = @Id";
                var affected = await connection.ExecuteAsync(sql, order);
                if (affected == 0)
                {

                    return null;
                }
                return order;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating OrderQueue: {ex.Message}");
                return null;

            }
        }

        public async Task<Guid> DeleteOrderAsync(Guid id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "DELETE FROM [dbo].[Uniwave_a2p_OrderQueue] WHERE [Id] = @Id";
                var affected = await connection.ExecuteAsync(sql, new { Id = id });
                if (affected == 0)
                {
                    _logger.LogWarning($"No OrderQueue found with ID: {id} to delete.");
                    return Guid.Empty;

                }
                return id;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting OrderQueue by ID: {ex.Message}");
                return Guid.Empty;
            }

        }
    }
}
