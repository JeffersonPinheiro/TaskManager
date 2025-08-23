﻿namespace GerenciamentoDeTarefas.src.Application.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ResponsibleName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
