using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;

namespace GerenciamentoDeTarefas.src.Application.UseCases.Tasks
{
    public class ListTaskUseCase
    {
        private readonly ITaskService _taskService;
        public ListTaskUseCase(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<List<TaskDto>> ExecuteAsync(TaskFilter filter)
        {
            return await _taskService.ListTasksAsync(filter);
        }
    }
}
