using GerenciamentoDeTarefas.src.Domain.Entities;
using TaskStatus = GerenciamentoDeTarefas.src.Domain.Entities.TaskStatus;

namespace GerenciamentoDeTarefas.src.Application.DTOs
{
    public class EditTaskRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? ResponsibleId { get; set; }
        public string? Status { get; set; }
    }
}
