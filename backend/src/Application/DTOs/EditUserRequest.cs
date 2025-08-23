namespace GerenciamentoDeTarefas.src.Application.DTOs
{
    public class EditUserRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
