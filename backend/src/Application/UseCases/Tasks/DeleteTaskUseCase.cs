using GerenciamentoDeTarefas.src.Application.Interfaces;

namespace GerenciamentoDeTarefas.src.Application.UseCases.Tasks
{
    public class DeleteTaskUseCase
    {
        private readonly ITaskService _taskService;
        public DeleteTaskUseCase(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task ExecuteAsync(Guid taskId)
        {
            await _taskService.DeleteTaskAsync(taskId);
        }
    }
}
