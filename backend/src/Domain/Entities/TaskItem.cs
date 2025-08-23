namespace GerenciamentoDeTarefas.src.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid ResponsibleId { get; set; }
        public User Responsible { get; set; } = null!;
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed,
    }
}
