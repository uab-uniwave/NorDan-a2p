using a2p.Application.Interfaces.Repositories;
using a2p.Application.Interfaces.Services;
using a2p.Domain.Entities;
using a2p.Domain.Enums;
using a2p.Domain.Shared;

using Microsoft.Extensions.Logging;

namespace a2p.Application.Services
{
    public class TaskQueueService : ITaskQueueService
    {
        private readonly ITaskQueueRepository _repo;
        private readonly ILogger<TaskQueueService> _logger;

        public TaskQueueService(ITaskQueueRepository repo, ILogger<TaskQueueService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Result<TaskEntity>> CreateTaskAsync(TaskEntity task)
        {
            try
            {
                task.CreatedUTCDateTime = DateTime.UtcNow;
                var created = await _repo.CreateTaskAsync(task);
                if (created == null || created.Id == Guid.Empty)
                    return Result<TaskEntity>.Failure("Failed to create task.");

                _logger.LogInformation("Task for Order {OrderNumber} created.", created.OrderNumber);
                return Result<TaskEntity>.Success(created, "Task created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task for order {OrderNumber}", task?.OrderNumber);
                return Result<TaskEntity>.Failure("Error creating task.");
            }
        }

        public async Task<Result<TaskEntity>> GetTaskByIdAsync(Guid id)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(id);
                return task == null
                    ? Result<TaskEntity>.Failure($"Task {id} not found.")
                    : Result<TaskEntity>.Success(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task {Id}", id);
                return Result<TaskEntity>.Failure("Error retrieving task.");
            }
        }

        public async Task<Result<TaskEntity>> GetTaskByOrderNumberAsync(string orderNumber)
        {
            try
            {
                var task = await _repo.GetTaskByOrderNumberAsync(orderNumber);
                return task == null
                    ? Result<TaskEntity>.Failure($"Task for order '{orderNumber}' not found.")
                    : Result<TaskEntity>.Success(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task for order {OrderNumber}", orderNumber);
                return Result<TaskEntity>.Failure("Error retrieving task.");
            }
        }

        public async Task<PagedResult<TaskEntity>> GetPagedTasksAsync(int page, int size)
        {
            try
            {
                var (tasks, total) = await _repo.GetPageTasksAsync(page, size);
                return PagedResult<TaskEntity>.Success(tasks, total, page, size);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged tasks.");
                return PagedResult<TaskEntity>.Failure("Error retrieving paged tasks.");
            }
        }

        public async Task<Result<bool>> UpdateTaskStateAsync(Guid id, OrderState state)
        {
            try
            {
                var rows = await _repo.UpdateTaskStateAsync(id, state);
                return rows == 0
                    ? Result<bool>.Failure("Failed to update task state.")
                    : Result<bool>.Success(true, "Task state updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task state for {Id}", id);
                return Result<bool>.Failure("Error updating task state.");
            }
        }

        public async Task<Result<bool>> DeleteTaskByIdAsync(Guid id)
        {
            try
            {
                var rows = await _repo.DeleteTaskByIdAsync(id);
                return rows == 0
                    ? Result<bool>.Failure("Failed to delete task.")
                    : Result<bool>.Success(true, "Task deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task {Id}", id);
                return Result<bool>.Failure("Error deleting task.");
            }
        }
    }
}
