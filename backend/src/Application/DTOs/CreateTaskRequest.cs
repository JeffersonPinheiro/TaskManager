namespace GerenciamentoDeTarefas.src.Application.DTOs
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid ResponsibleId { get; set; }
    }
}
