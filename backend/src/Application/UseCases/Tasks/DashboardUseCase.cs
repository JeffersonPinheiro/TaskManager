using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;

namespace GerenciamentoDeTarefas.src.Application.UseCases.Tasks
{
    public class DashboardUseCase
    {
        private readonly ITaskService _taskService;
        public DashboardUseCase(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<DashboardDto> ExecuteAsync()
        {
            return await _taskService.GetDashboardAsync();
        }
    }
}
