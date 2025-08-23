using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Domain.Entities;
using TaskStatus = GerenciamentoDeTarefas.src.Domain.Entities.TaskStatus;

namespace GerenciamentoDeTarefas.src.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task AddAsync(TaskItem task);
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<List<TaskItem>> GetAllAsync();
        Task<List<TaskItem>> GetFilteredAsync(TaskFilter filter);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(Guid id);
        Task<int> CountByStatusAsync(TaskStatus status);
        Task<List<TaskItem>> GetLastCreatedAsync(int count);
    }
}
