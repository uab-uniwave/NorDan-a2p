using System.Data;

using a2p.Application.Interfaces.Repositories;
using a2p.Domain.Entities;
using a2p.Domain.Enums;

using Dapper;

namespace a2p.Infrastructure.Persistence.Repositories
{
    public class TaskQueueRepository : ITaskQueueRepository
    {
        private readonly IDbConnectionFactory _factory;

        public TaskQueueRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        // CREATE
        public async Task<TaskEntity?> CreateTaskAsync(TaskEntity order)
        {
            const string sql = @"
                                INSERT INTO  Uniwave_a2p_TaskQueue
                                (Id 
                                , OrderId 
                                , OrderNumber 
                                , ProjectNumber 
                                , Number 
                                , Version 
                                , State 
                                , PayloadJson 
                                , ProcessedUTCDateTime 
                                , CreatedUTCDateTime 
                                , ModifiedUTCDateTime 
                                , CreatedBy 
                                , ModifiedBy)
                                OUTPUT INSERTED.*
                                VALUES
                                (@Id
                                ,@rderId 
                                ,@OrderNumber
                                ,@ProjectNumber 
                                ,@Number
                                ,@Version 
                                ,@State
                                ,@PayloadJson
                                ,@ProcessedUTCDateTime
                                ,@CreatedUTCDateTime
                                ,@ModifiedUTCDateTime
                                ,@CreatedBy
                                ,@ModifiedBy)";

            using IDbConnection db = _factory.CreateConnection();

            return await db.QuerySingleOrDefaultAsync<TaskEntity>(sql, order);
        }

        // READ BY ID
        public async Task<TaskEntity?> GetTaskByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM Uniwave_a2p_TaskQueue WHERE Id = @Id;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.QuerySingleOrDefaultAsync<TaskEntity>(sql, new { Id = id });
        }

        // READ BY NUMBER
        public async Task<TaskEntity?> GetTaskByOrderNumberAsync(string orderNumber)
        {
            const string sql = "SELECT * FROM Uniwave_a2p_TaskQueue WHERE OrderNumber = @OrderNumber;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.QuerySingleOrDefaultAsync<TaskEntity>(sql, new { OrderNumber = orderNumber });
        }

        // PAGED READ
        public async Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetPageTasksAsync(int page, int size)
        {
            const string sql = @"
                SELECT * FROM Uniwave_a2p_TaskQueue
                ORDER BY CreatedUTCDateTime DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                SELECT COUNT(*) FROM Uniwave_a2p_TaskQueue;";
            using IDbConnection db = _factory.CreateConnection();
            using SqlMapper.GridReader multi = await db.QueryMultipleAsync(sql, new { Offset = (page - 1) * size, PageSize = size });
            IEnumerable<TaskEntity> tasks = await multi.ReadAsync<TaskEntity>();
            var total = await multi.ReadSingleAsync<int>();
            return (tasks, total);
        }

        // UPDATE FULL ORDER
        public async Task<int> UpdateTaskAsync(OrderEntity order)
        {
            const string sql = @"
        
                                UINSERT INTO  Uniwave_a2p_TaskQueue
                                (Id
                                , OrderId
                                , OrderNumber
                                , ProjectNumber
                                , Number
                                , Version
                                , State
                                , PayloadJson
                                , ProcessedUTCDateTime
                                , CreatedUTCDateTime
                                , ModifiedUTCDateTime
                                , CreatedBy
                                , ModifiedBy)
                                OUTPUT INSERTED.*
                                VALUES
                                (@Id
                                , @rderId
                                , @OrderNumber
                                , @ProjectNumber
                                , @Number
                                , @Version
                                , @State
                                , @PayloadJson
                                , @ProcessedUTCDateTime
                                , @CreatedUTCDateTime
                                , @ModifiedUTCDateTime
                                , @CreatedBy
                                , @ModifiedBy)";
            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteAsync(sql, order);
        }

        // UPDATE ONLY DELIVERY ADDRESS
        public async Task<int> UpdateTaskStateAsync(Guid id, OrderState orderState)
        {
            const string sql = @"
                UPDATE Uniwave_a2p_TaskQueue
                SET OrderState = @State,
                    ModifiedUTCDateTime = GETUTCDATE()
                WHERE Id = @Id;";
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(sql, new { Id = id, OrderState = orderState.ToString() });
        }

        // DELETE
        public async Task<int> DeleteTaskByIdAsync(Guid id)
        {
            const string sql = "DELETE FROM Uniwave_a2p_TaskQueue WHERE Id = @Id;";
            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteAsync(sql, new { Id = id });
        }
    }
}