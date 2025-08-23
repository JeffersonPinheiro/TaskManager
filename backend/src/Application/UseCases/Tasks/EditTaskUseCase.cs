using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;

namespace GerenciamentoDeTarefas.src.Application.UseCases.Tasks
{
    public class EditTaskUseCase
    {
        private readonly ITaskService _taskService;
        public EditTaskUseCase(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<TaskDto> ExecuteAsync(Guid taskId, EditTaskRequest request)
        {
            return await _taskService.EditTaskAsync(taskId, request);
        }
    }
}
