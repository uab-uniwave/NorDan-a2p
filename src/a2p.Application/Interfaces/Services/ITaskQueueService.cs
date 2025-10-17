using a2p.Domain.Entities;
using a2p.Domain.Enums;
using a2p.Domain.Shared;

namespace a2p.Application.Interfaces.Services
{
    public interface ITaskQueueService
    {
        Task<Result<TaskEntity>> CreateTaskAsync(TaskEntity task);
        Task<Result<TaskEntity>> GetTaskByIdAsync(Guid id);

        Task<Result<TaskEntity>> GetTaskByOrderNumberAsync(string orderNumber);

        Task<PagedResult<TaskEntity>> GetPagedTasksAsync(int page, int size);
        Task<Result<bool>> UpdateTaskStateAsync(Guid id, OrderState state);

        Task<Result<bool>> DeleteTaskByIdAsync(Guid id);
    }
}
