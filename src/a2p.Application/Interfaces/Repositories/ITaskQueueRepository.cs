using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories
{
    public interface ITaskQueueRepository
    {

        Task<TaskEntity?> CreateTaskAsync(TaskEntity order);

        // READ BY ID
        Task<TaskEntity?> GetTaskByIdAsync(Guid id);

        // READ BY NUMBER
        Task<TaskEntity?> GetTaskByOrderNumberAsync(string orderNumber);

        // PAGED READ
        Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetPageTasksAsync(int page, int size);

        // UPDATE FULL ORDER
        Task<int> UpdateTaskAsync(OrderEntity order);
        // UPDATE ONLY DELIVERY ADDRESS
        Task<int> UpdateTaskStateAsync(Guid id, OrderState orderState);

        // DELETE
        Task<int> DeleteTaskByIdAsync(Guid id);

    }
}
