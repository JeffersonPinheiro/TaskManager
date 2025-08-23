using GerenciamentoDeTarefas.src.Application.DTOs;

namespace GerenciamentoDeTarefas.src.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(CreateTaskRequest req);
        Task<TaskDto> EditTaskAsync(Guid taskId, EditTaskRequest req);
        Task DeleteTaskAsync(Guid taskId);
        Task<List<TaskDto>> ListTasksAsync(TaskFilter filter);
        Task<TaskDto?> GetTaskByIdAsync(Guid taskId);
        Task<DashboardDto> GetDashboardAsync();
    }
}
