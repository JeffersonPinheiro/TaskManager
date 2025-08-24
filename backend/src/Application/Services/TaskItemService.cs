using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;
using GerenciamentoDeTarefas.src.Domain.Entities;
using GerenciamentoDeTarefas.src.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using TaskStatus = GerenciamentoDeTarefas.src.Domain.Entities.TaskStatus;

namespace GerenciamentoDeTarefas.src.Application.Services
{
    public class TaskItemService
    {
        public class TaskService : ITaskService
        {
            private readonly ITaskRepository _taskRepo;
            private readonly IUserRepository _userRepo;
            public TaskService(ITaskRepository taskRepo, IUserRepository userRepo)
            {
                _taskRepo = taskRepo;
                _userRepo = userRepo;
            }

            public async Task<TaskDto> CreateTaskAsync(CreateTaskRequest req)
            {
                var user = await _userRepo.GetByIdAsync(req.ResponsibleId)
                    ?? throw new CannotUnloadAppDomainException("Responsável não encontrado.");

                var task = new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = req.Title,
                    Description = req.Description,
                    ResponsibleId = user.Id,
                    Status = TaskStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };
                await _taskRepo.AddAsync(task);

                return new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    ResponsibleName = user.Name,
                    Status = task.Status.ToString(),
                    CreatedAt = task.CreatedAt
                };
            }

            public async Task DeleteTaskAsync(Guid taskId)
            {
                var task =  await _taskRepo.GetByIdAsync(taskId)
                    ?? throw new CannotUnloadAppDomainException("Tarefa não encontrada.");

                await _taskRepo.DeleteAsync(task.Id);
            }

            public async Task<TaskDto> EditTaskAsync(Guid taskId, EditTaskRequest req)
            {
                var task = await _taskRepo.GetByIdAsync(taskId)
                    ?? throw new CannotUnloadAppDomainException("Tarefa não encontrada.");

                if (!string.IsNullOrWhiteSpace(req.Title))
                    task.Title = req.Title;

                if (!string.IsNullOrWhiteSpace(req.Description))
                    task.Description = req.Description;

                if (req.ResponsibleId.HasValue)
                {
                    var user = await _userRepo.GetByIdAsync(req.ResponsibleId.Value)
                        ?? throw new CannotUnloadAppDomainException("Responsável não encontrado.");
                    task.ResponsibleId = user.Id;
                }

                if (!string.IsNullOrWhiteSpace(req.Status))
                    task.Status = Enum.Parse<TaskStatus>(req.Status);

                await _taskRepo.UpdateAsync(task);

                var responsibleUser = await _userRepo.GetByIdAsync(task.ResponsibleId);

                return new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    ResponsibleName = responsibleUser?.Name ?? "Desconhecido", 
                    Status = task.Status.ToString(),
                    CreatedAt = task.CreatedAt
                };
            }

            public async Task<DashboardDto> GetDashboardAsync()
            {
                var dashboard = new DashboardDto();

                dashboard.PendingCount = await _taskRepo.CountByStatusAsync(TaskStatus.Pending);
                dashboard.InProgressCount = await _taskRepo.CountByStatusAsync(TaskStatus.InProgress);
                dashboard.CompletedCount = await _taskRepo.CountByStatusAsync(TaskStatus.Completed);

                var lastCreatedTasks = await _taskRepo.GetLastCreatedAsync(5);
                dashboard.LastCreatedTasks = lastCreatedTasks.Select(task => new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    ResponsibleName = task.Responsible?.Name ?? "Desconhecido",
                    Status = task.Status.ToString(),
                    CreatedAt = task.CreatedAt
                }).ToList();

                return dashboard;
            }

            public async Task<TaskDto?> GetTaskByIdAsync(Guid taskId)
            {
                var task = await _taskRepo.GetByIdAsync(taskId);
                
                if (task == null) return null;
                
                return new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    ResponsibleName = task.Responsible?.Name ?? "Desconhecido",
                    Status = task.Status.ToString(),
                    CreatedAt = task.CreatedAt
                };
            }

            public async Task<List<TaskDto>> ListTasksAsync(TaskFilter filter)
            {
                var tasks = await _taskRepo.GetFilteredAsync(filter);

                return tasks.Select(task => new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    ResponsibleName = task.Responsible?.Name ?? "Desconhecido",
                    Status = task.Status.ToString(),
                    CreatedAt = task.CreatedAt
                }).ToList();
            }
        }

        
    }
}
