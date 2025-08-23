using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;

namespace GerenciamentoDeTarefas.src.Application.UseCases.Tasks
{
    public class CreateTaskUseCase
    {
        private readonly ITaskService _taskService;
        public CreateTaskUseCase(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<TaskDto> ExecuteAsync(CreateTaskRequest request)
        {
            return await _taskService.CreateTaskAsync(request);
        }
    }
}
