namespace GerenciamentoDeTarefas.src.Application.DTOs
{
    public class DashboardDto
    {
        public int PendingCount { get; set; }
        public int InProgressCount { get; set; }
        public int CompletedCount { get; set; }
        public List<TaskDto> LastCreatedTasks { get; set; } = new();
    }
}
