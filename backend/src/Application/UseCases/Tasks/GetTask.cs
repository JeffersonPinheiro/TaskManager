using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;

namespace GerenciamentoDeTarefas.src.Application.UseCases.Tasks
{
    public class GetTaskUseCase
    {
        private readonly ITaskService _taskService;
        public GetTaskUseCase(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<TaskDto?> ExecuteAsync(Guid taskId)
        {
            return await _taskService.GetTaskByIdAsync(taskId);
        }
    }
}
